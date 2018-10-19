namespace SIS.Framework.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class DependencyContainer : IDependencyContainer
    {
        private readonly IDictionary<Type, Type> dependencyMap;

        public DependencyContainer()
        {
            this.dependencyMap = new Dictionary<Type, Type>();
        }

        private Type this[Type key]
            => this.dependencyMap.ContainsKey(key) ? this.dependencyMap[key] : null;

        public void RegisterDependency<TSource, TDestination>()
        {
            this.dependencyMap[typeof(TSource)] = typeof(TDestination);
        }

        public T CreateInstance<T>()
            => (T) this.CreateInstance(typeof(T));

        public object CreateInstance(Type type)
        {
            Type instanceType = this[type] ?? type;

            if (instanceType.IsInterface || instanceType.IsAbstract)
            {
                throw new InvalidOperationException($"Type {instanceType.FullName} cannot be instantiated.");
            }

            ConstructorInfo constructor =
                instanceType.GetConstructors().OrderBy(x => x.GetParameters().Length).First();
            ParameterInfo[] constructorParameters = constructor.GetParameters();
            object[] constructorParameterObjects = new object[constructorParameters.Length];

            for (int i = 0; i < constructorParameters.Length; i++)
            {
                constructorParameterObjects[i] = this.CreateInstance(constructorParameters[i].ParameterType);
            }

            return constructor.Invoke(constructorParameterObjects);
        }
    }
}
