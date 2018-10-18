using System.ComponentModel.DataAnnotations;

namespace SIS.Framework.Routers
{  
    using ActionResults.Contracts;
    using Attributes;
    using Attributes.Methods;
    using Controllers;
    using HTTP.Enums;
    using HTTP.Extensions;
    using HTTP.Headers;
    using HTTP.Requests.Contracts;
    using HTTP.Responses;
    using HTTP.Responses.Contracts;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using WebServer.Api;
    using WebServer.Results;

    public class ControllerRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            var response = this.TryHandleResourceRequest(request);
            if (response != null)
            {
                return response;
            }

            var controllerName = string.Empty;
            var actionName = string.Empty;
            var requestMethod = request.RequestMethod.ToString();

            if (request.Url == "/")
            {
                controllerName = MvcContext.Get.DefaulControllerName;
                actionName = MvcContext.Get.DefaultActionName;
            }
            else
            {
                var requestUrlSplit = request.Path.Split(
                    new [] {"/"},
                    StringSplitOptions.RemoveEmptyEntries);

                controllerName = requestUrlSplit[0].Capitalize();
                actionName = requestUrlSplit[1].Capitalize();
            }

            var controller = this.GetController(controllerName);
            controller.Request = request;
            var action = this.GetAction(requestMethod, controller, actionName);

            if (action is null)
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return this.PrepareResponse(controller, action);
        }

        private IHttpResponse PrepareResponse(Controller controller, MethodInfo action)
        {
            var response = new HttpResponse();
            controller.Response = response;

            var actionParameters = this.MapActionParameters(action, controller);
            IActionResult actionResult = (IActionResult)action.Invoke(controller, actionParameters);

            string invocationResult = actionResult.Invoke();

            if (actionResult is IViewable)
            {            
                response.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/html"));
                response.Content = Encoding.UTF8.GetBytes(invocationResult);
                response.StatusCode = HttpResponseStatusCode.Ok;
                return response;
            }

            if (actionResult is IRedirectable)
            {
                response.StatusCode = HttpResponseStatusCode.SeeOther;
                response.Headers.Add(new HttpHeader(HttpHeader.Location, invocationResult));
                return response;
            }

            throw new InvalidOperationException("The view result is not supported.");
        }

        private object[] MapActionParameters(MethodInfo action, Controller controller)
        {
            ParameterInfo[] actionParametersInfo = action.GetParameters();
            object[] mappedActionParameters = new object[actionParametersInfo.Length];

            for (int i = 0; i < mappedActionParameters.Length; i++)
            {
                ParameterInfo currentParameterInfo = actionParametersInfo[i];

                if (currentParameterInfo.ParameterType.IsPrimitive
                    || currentParameterInfo.ParameterType == typeof(string))
                {
                    mappedActionParameters[i] = ProcessPrimitiveParameter(currentParameterInfo, controller.Request);
                }
                else
                {
                    object bindingModel = ProcessBindingModelParameters(currentParameterInfo, controller.Request);
                    controller.ModelState.IsValid = this.IsValidModel(bindingModel, currentParameterInfo.ParameterType);
                    mappedActionParameters[i] = bindingModel;
                }
            }

            return mappedActionParameters;
        }

        private bool? IsValidModel(object bindingModel, Type bindingModelType)
        {
            var properties = bindingModelType.GetProperties();
            foreach (var property in properties)
            {
                var propertyValidationAttributes = property
                    .GetCustomAttributes()
                    .Where(a => a is ValidationAttribute)
                    .Cast<ValidationAttribute>()
                    .ToList();

                foreach (var validationAttribute in propertyValidationAttributes)
                {
                    var propertyValue = property.GetValue(bindingModel);
                    if (!validationAttribute.IsValid(propertyValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private object ProcessBindingModelParameters(ParameterInfo parameter, IHttpRequest request)
        {
            Type bindingModelType = parameter.ParameterType;
            var bindingModelInstance = Activator.CreateInstance(bindingModelType);
            var bindingModelProperties = bindingModelType.GetProperties();

            foreach (var property in bindingModelProperties)
            {
                try
                {
                    object value = this.GetParameterFromRequestData(request, property.Name.ToCamelCase());
                    property.SetValue(bindingModelInstance, Convert.ChangeType(value, property.PropertyType));
                }
                catch
                {
                    Console.WriteLine($"The {property.Name} field could not be mapped.");
                }
            }

            return Convert.ChangeType(bindingModelInstance, bindingModelType);
        }

        private object ProcessPrimitiveParameter(ParameterInfo parameter, IHttpRequest request)
        {
            object value = this.GetParameterFromRequestData(request, parameter.Name.ToCamelCase());
            return Convert.ChangeType(value, parameter.ParameterType);
        }

        private object GetParameterFromRequestData(IHttpRequest request, string parameterName)
        {
            if (request.QueryData.ContainsKey(parameterName))
            {
                return request.QueryData[parameterName];
            }

            if (request.FormData.ContainsKey(parameterName))
            {
                return request.FormData[parameterName];
            }

            return null;
        }

        private IHttpResponse TryHandleResourceRequest(IHttpRequest request)
        {
            var requestPath = request.Path;
            if (requestPath.Contains("."))
            {
                var filePath = $"{MvcContext.Get.RootDirectoryRelativePath}Resources{requestPath}";
                if (!File.Exists(filePath))
                {
                    return new HttpResponse(HttpResponseStatusCode.NotFound);
                }

                var fileContent = File.ReadAllBytes(filePath);

                return new InlineResourceResult(fileContent);
            }

            return null;
        }

        private MethodInfo GetAction(
           string requestMethod,
           Controller controller,
           string actionName)
        {
            var actions = this
                .GetSuitableMethods(controller, actionName)
                .ToList();

            if (!actions.Any())
            {
                return null;
            }

            foreach (var action in actions)
            {
                var httpMethodAttributes = action
                    .GetCustomAttributes()
                    .Where(ca => ca is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>()
                    .ToList();

                if (!httpMethodAttributes.Any() &&
                   requestMethod.ToUpper() == HttpRequestMethod.Get.ToString().ToUpper())
                {
                    return action;
                }

                foreach (var httpMethodAttribute in httpMethodAttributes)
                {
                    if (httpMethodAttribute.IsValid(requestMethod))
                    {
                        return action;
                    }
                }
            }

            return null;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(
           Controller controller,
           string actionName)
        {
            if (controller is null)
            {
                return new MethodInfo[0];
            }

            var routeMethods = controller
                .GetType()
                .GetMethods()
                .Where(mi =>
                    mi.GetCustomAttributes()
                        .Where(ca => ca is RouteAttribute)
                        .Cast<RouteAttribute>()
                        .Any(a => a.Path == $"/{actionName.ToLower()}"));

            
            return controller
               .GetType()
               .GetMethods()
               .Where(mi => mi.Name.ToLower() == actionName.ToLower())
               .Union(routeMethods);
        }

        private Controller GetController(string controllerName)
        {
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                return null;
            }

            var fullyQualifiedControllerName = string.Format("{0}.{1}.{2}{3}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ControllersFolder,
                controllerName,
                MvcContext.Get.ControllerSuffix);

            var controllerType = Type.GetType(fullyQualifiedControllerName);
            if (controllerType is null)
            {
                return null;
            }

            var controller = (Controller)Activator.CreateInstance(controllerType);
            return controller;
        }
    }
}
