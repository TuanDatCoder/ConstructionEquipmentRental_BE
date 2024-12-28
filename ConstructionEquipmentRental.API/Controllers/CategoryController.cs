using Microsoft.AspNetCore.Mvc;

namespace ConstructionEquipmentRental.API.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
