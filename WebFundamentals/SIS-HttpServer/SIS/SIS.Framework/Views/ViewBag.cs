namespace SIS.Framework.Views
{
    using System.Collections.Generic;

    public class ViewBag
    {
        public IDictionary<string, string> Data { get; private set; }

        public ViewBag()
        {
            this.Data = new Dictionary<string, string>();
        }

        public object this[string key]
        {
            get => this.Data[key];
            set => this.Data[key] = value.ToString();
        }

        public void Clear()
        {
            if (this.Data.Count > 0)
            {
                this.Data = new Dictionary<string, string>();
            }
        }
    }
}
