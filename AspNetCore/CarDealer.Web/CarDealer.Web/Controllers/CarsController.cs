using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarDealer.Web.Controllers
{
    using Infrastructure.Extensions;
    using Models.CarsViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    [Route("cars")]
    public class CarsController : Controller
    {
        private readonly ICarService cars;
        private readonly IPartService parts;

        public CarsController(ICarService cars, IPartService parts)
        {
            this.cars = cars;
            this.parts = parts;
        }

        [Route(nameof(All))]
        public IActionResult All()
        {
            return View(this.cars.All());
        }

        [Authorize]
        [Route(nameof(Create))]
        public IActionResult Create()
        {
            var carFormViewModel = new CarFormViewModel()
            {
                AllParts = this.GetPartsSelectItems()
            };

            return View(carFormViewModel);
        }

        [Authorize]
        [Route(nameof(Create))]
        [HttpPost]
        public IActionResult Create(CarFormViewModel carModel)
        {
            if (!ModelState.IsValid)
            {
                carModel.AllParts = this.GetPartsSelectItems();
                return View(carModel);
            }

            this.cars.Create(
                carModel.Make,
                carModel.Model,
                carModel.TravelledDistance,
                carModel.SelectedParts);

            return RedirectToAction(nameof(All));
        }

        [Route("{make}")]
        public IActionResult ByMake(string make)
        {
            var carsByMake = this.cars.ByMake(make);
            var carsByMakeViewModel = new CarsByMakeViewModel
            {
                Make = make,
                Cars = carsByMake
            };

            return View(carsByMakeViewModel);
        }

        [Route("{id}/parts")]
        public IActionResult CarWithParts(int id)
        {
            return this.ViewOrNotFound(this.cars.WithParts(id));
        }

        private IEnumerable<SelectListItem> GetPartsSelectItems()
        {
            return this.parts
                .All()
                .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                });
        }
    }
}
