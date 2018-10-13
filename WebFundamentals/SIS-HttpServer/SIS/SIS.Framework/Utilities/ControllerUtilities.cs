namespace SIS.Framework.Utilities
{
    public static class ControllerUtilities
    {
        public static string GetControllerName(object controller)
        {
            return controller
                .GetType()
                .Name
                .Replace(MvcContext.Get.ControllerSuffix, string.Empty);
        }

        public static string GetViewPath(string controller, string action)
        {
            string rootDirectory = MvcContext.Get.RootDirectoryRelativePath;
            string viewsFolder = MvcContext.Get.ViewsFolder;
            return $"{rootDirectory}{viewsFolder}/{controller}/{action}.html";
        }

        public static string GetLayoutPath(string layoutName)
        {
            string rootDirectory = MvcContext.Get.RootDirectoryRelativePath;
            string viewsFolder = MvcContext.Get.ViewsFolder;
            string layoutsFolder = MvcContext.Get.LayoutsFolder;

            if (layoutsFolder == string.Empty)
            {
                return $"{rootDirectory}{viewsFolder}/{layoutName}.html";
            }
            
            return $"{rootDirectory}{viewsFolder}/{layoutsFolder}/{layoutName}.html";
        }
    }
}
