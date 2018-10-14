namespace SIS.Framework
{
    public class MvcContext
    {
        private static MvcContext Instance;

        private MvcContext() { }

        public static MvcContext Get => Instance ?? (Instance = new MvcContext());

        public string AssemblyName { get; set; }

        public string ControllerSuffix { get; set; } = "Controller";

        public string ControllersFolder { get; set; } = "Controllers";

        public string ViewsFolder { get; set; } = "Views";

        public string ModelsFolder { get; set; } = "Models";

        public string DefaulControllerName { get; set; } = "Home";

        public string DefaultActionName { get; set; } = "Index";

        public string RootDirectoryRelativePath { get; set; } = "../../../";

        public string DefaultLayoutViewName { get; set; } = "Layout";

        public string LayoutsFolder { get; set; } = string.Empty;

        public int Port { get; set; } = 8000;

        internal const string TemplatePlaceholder = "@RenderBody";
    }
}
