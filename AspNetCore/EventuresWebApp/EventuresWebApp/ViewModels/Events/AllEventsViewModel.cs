namespace EventuresWebApp.Web.ViewModels.Events
{
    using System.Collections.Generic;

    public class AllEventsViewModel
    {
        public IEnumerable<EventViewModel> Events { get; set; }
    }
}
