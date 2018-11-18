namespace ChushkaWebApp.Web.Controllers
{
    using ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Diagnostics;
    using System.Linq;
    using Services.Interfaces;

    public class HomeController : Controller
    {
        private readonly IProductService products;

        public HomeController(IProductService products)
        {
            this.products = products;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var indexViewModel = new IndexViewModel
                {
                    Products = this.products.All().Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price
                    })
                };

                return View("IndexAuth", indexViewModel);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
