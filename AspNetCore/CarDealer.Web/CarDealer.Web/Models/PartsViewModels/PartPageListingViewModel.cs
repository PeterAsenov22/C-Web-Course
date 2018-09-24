namespace CarDealer.Web.Models.PartsViewModels
{
    using Services.Models.Parts;
    using System.Collections.Generic;

    public class PartPageListingViewModel
    {
        public IEnumerable<PartListingModel> Parts { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage => CurrentPage == 1 ? 1 : CurrentPage - 1;

        public int NextPage => CurrentPage == TotalPages ? CurrentPage : CurrentPage + 1;
    }
}
