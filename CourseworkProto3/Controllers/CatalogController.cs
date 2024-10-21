using Library.Data.Repo;
using Library.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ProductRepository _productRepository;

        public CatalogController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: Catalog
        public async Task<ActionResult> Index()
        {
            var products = await _productRepository.GetAll();
            return View(products);
        }

    }
}
