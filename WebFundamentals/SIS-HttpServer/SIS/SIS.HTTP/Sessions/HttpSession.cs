namespace SIS.HTTP.Sessions
{
    using Contracts;
    using Common;
    using System.Collections.Generic;
    
    public class HttpSession : IHttpSession
    {
        private readonly IDictionary<string, object> parameters;

        public HttpSession(string id)
        {
            this.Id = id;
            this.parameters = new Dictionary<string, object>();
        }

        public string Id { get; }

        public object GetParameter(string name)
        {
            if (this.ContainsParameter(name))
            {
                return this.parameters[name];
            }

            return null;
        }

        public bool ContainsParameter(string name)
        {
            CoreValidator.ThrowIfNull(name, nameof(name));

            return this.parameters.ContainsKey(name);
        }

        public void AddParameter(string name, object parameter)
        {
            CoreValidator.ThrowIfNull(name, nameof(name));
            CoreValidator.ThrowIfNull(parameter, nameof(parameter));

            if (!this.ContainsParameter(name))
            {
                this.parameters.Add(name, parameter);
            }
        }

        public void ClearParameters()
        {
            this.parameters.Clear();
        }
    }
}
