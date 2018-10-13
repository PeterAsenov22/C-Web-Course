namespace SIS.Framework.Views
{
    using ActionResults.Contracts;
    using System.IO;
    using System.Collections.Generic;

    public class View : IRenderable
    {
        private readonly string viewPath;

        private readonly string layoutPath;

        private readonly IDictionary<string, string> viewBag;

        public View(string viewPath, string layoutPath, IDictionary<string, string> viewBag)
        {
            this.viewPath = viewPath;
            this.layoutPath = layoutPath;
            this.viewBag = viewBag;
        }

        public string Render()
        {
            string layoutHtml = this.ReadLayoutView();
            string viewHtml = this.ReadView();

            string content = layoutHtml.Replace(MvcContext.TemplatePlaceholder, viewHtml);
            if (viewBag != null)
            {
                foreach (var item in viewBag)
                {
                    content = content.Replace("@Model." + item.Key, item.Value);
                }
            }

            return content;
        }

        private string ReadLayoutView()
        {
            if (!File.Exists(layoutPath))
            {
                throw new FileNotFoundException($"Layout view does not exist at {layoutPath}.");
            }

            return File.ReadAllText(layoutPath);
        }

        private string ReadView()
        {
            if (!File.Exists(viewPath))
            {
                throw new FileNotFoundException($"View does not exist at {viewPath}.");
            }

            return File.ReadAllText(viewPath);
        }
    }
}
