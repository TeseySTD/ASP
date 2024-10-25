using Library.Data.Repo;
using Library.Models.DTO;
using Library.Models.Entities;
using Library.Services;
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
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            var dto = new ProductDto
            {
                ProductId = product.ProductId,
                Title = product.Title
            };

            if (product.Book != null)
            {
                dto.Book = new BookDto
                {
                    Author = product.Book.Author,
                    Genre = string.Join(", ", product.Book.Genre.Select(g => g.Name)),
                    PublicationYear = product.Book.PublicationYear
                };
            }

            if (product.Disc != null)
            {
                dto.Disc = new DiscDto
                {
                    Format = product.Disc.Format,
                    Year = product.Disc.Year
                };

                if (product.Disc.Movie != null)
                {
                    dto.Disc.Movie = new MovieDto
                    {
                        Director = product.Disc.Movie.Director,
                        Duration = product.Disc.Movie.Duration,
                        Genre = string.Join(", ", product.Disc.Movie.Genre.Select(g => g.Name)), 
                        Actors = string.Join(", ", product.Disc.Movie.Actors.Select(a => a.Name))
                    };
                }

                if (product.Disc.Music != null)
                {
                    dto.Disc.Music = new MusicDto
                    {
                        Artist = product.Disc.Music.Artist,
                        Genre = string.Join(", ", product.Disc.Music.Genre.Select(g => g.Name)), 
                    };
                }

                if (product.Disc.Game != null)
                {
                    dto.Disc.Game = new GameDto
                    {
                        Publisher = product.Disc.Game.Publisher,
                        Developer = product.Disc.Game.Developer,
                        Genre = string.Join(", ", product.Disc.Game.Genre.Select(g => g.Name)), 
                    };
                }
            }

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDto dto)
        {
            if (!await _productRepository.ProductExists(dto.ProductId))
            {
                return NotFound();
            }
            await _productRepository.Update(dto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddBook(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBook(ProductDto dto){
            dto.OwnerId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            await _productRepository.AddBook(dto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddMovie(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMovie(ProductDto dto){
            dto.OwnerId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            await _productRepository.AddMovie(dto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddMusic(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMusic(ProductDto dto){
            dto.OwnerId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            await _productRepository.AddMusic(dto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddGame(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddGame(ProductDto dto){
            dto.OwnerId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            await _productRepository.AddGame(dto);
            return RedirectToAction("Index");
        }
    }
}
