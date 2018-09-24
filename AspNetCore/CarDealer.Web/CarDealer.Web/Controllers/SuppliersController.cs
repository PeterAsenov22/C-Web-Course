namespace CarDealer.Web.Controllers
{
    using Models.SuppliersViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    public class SuppliersController : Controller
    {
        private const string SuppliersView = "Suppliers";

        private readonly ISupplierService suppliers;

        public SuppliersController(ISupplierService suppliers)
        {
            this.suppliers = suppliers;
        }

        public IActionResult Local()
        {
            var local = GetSuppliersViewModel(false);
            return View(SuppliersView, local);
        }

        public IActionResult Importers()
        {
            var importers = GetSuppliersViewModel(true);
            return View(SuppliersView, importers);
        }

        private SuppliersViewModel GetSuppliersViewModel(bool importers)
        {
            var type = importers ? "Importers" : "Local";

            return new SuppliersViewModel
            {
                Type = type,
                Suppliers = this.suppliers.All(importers)
            };
        }
    }
}
