namespace SIS.Framework.Attributes
{
    using System;

    public class RouteAttribute : Attribute
    {
        public RouteAttribute(string path)
        {
            this.Path = path;
        }

        public string Path { get; }
    }
}
