namespace CarDealer.Web.Models.CarsViewModels
{
    using System.Collections.Generic;
    using Services.Models.Cars;

    public class CarsByMakeViewModel
    {
        public string Make { get; set; }

        public IEnumerable<CarModel> Cars { get; set; }
    }
}
