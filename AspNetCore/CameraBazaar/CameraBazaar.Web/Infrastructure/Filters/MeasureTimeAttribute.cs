namespace CameraBazaar.Web.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    public class MeasureTimeAttribute : ActionFilterAttribute
    {
        private readonly Stopwatch stopwatch = new Stopwatch(); 

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.stopwatch.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            this.stopwatch.Stop();
            Task.Run(async () =>
            {
                using (var writer = new StreamWriter("action-times.txt", true))
                {
                    var dateTime = DateTime.UtcNow;
                    var controller = context.Controller.GetType().Name;
                    var action = context.RouteData.Values["action"];
                    var elapsedTime = this.stopwatch.Elapsed;

                    var logMessage = $"{dateTime} – {controller}.{action} – {elapsedTime}";

                    await writer.WriteLineAsync(logMessage);
                }
            })
            .GetAwaiter()
            .GetResult();
        }
    }
}
