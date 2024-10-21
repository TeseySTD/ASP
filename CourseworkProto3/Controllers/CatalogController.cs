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

        public async Task<ActionResult> Delete(int id){
            await _productRepository.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            await _productRepository.Update(product);
            return RedirectToAction("Index");
        }
    }
}
