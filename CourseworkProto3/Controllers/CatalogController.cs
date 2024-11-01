using Library.Data.Repo;
using Library.Models.DTO;
using Library.Models.Entities;
using Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly ProductService _productService;
        private readonly UserRepository _userRepository;

        public CatalogController(ProductRepository productRepository
                                , ProductService productService
                                , UserRepository userRepository)
        {
            _productRepository = productRepository;
            _productService = productService;
            _userRepository = userRepository;
        }

        // GET: Catalog
        public async Task<ActionResult> Index()
        {
            var products = await _productRepository.GetFull();
            return View(products);
        }

        public async Task<ActionResult> Delete(int id){
            await _productRepository.DeleteById(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetFullById(id);

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

        [HttpGet]
        public async Task<IActionResult> Borrow(int id){
            if(await _productRepository.ProductExists(id) && !await _productService.IsProductBorrowed(id) 
                && !await _productService.IsUserOwner(id, Request.Cookies["access-cookie"])){
                var product = await _productRepository.GetById(id);
                var dto = new BorrowDto{
                    ProductId = id,  
                    BorrowerId = (int)JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]),
                    LenderId = product.OwnerId,
                    BorrowStartDate = DateTime.Now,
                    BorrowEndDate = DateTime.Now.AddDays(1)
                };
                return View(dto);
            }
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Borrow(BorrowDto dto){
            await _productService.BorrowProduct(dto);
            return RedirectToAction("Index");
        }

        [HttpGet] 
        public async Task<IActionResult> Give(int id){
            if(await _productRepository.ProductExists(id) && !await _productService.IsProductBorrowed(id) 
                && await _productService.IsUserOwner(id, Request.Cookies["access-cookie"])){
                var product = await _productRepository.GetById(id);
                var dto = new GiveDTO{
                    ProductId = id,
                    LenderId = product.OwnerId,
                    BorrowStartDate = DateTime.Now,
                    BorrowEndDate = DateTime.Now.AddDays(1)
                };
                return View(dto);
            }
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Give(GiveDTO dto){
            var borrower = await _userRepository.GetUserByEmail(dto.Email);
            var borrowDto = new BorrowDto{
                ProductId = dto.ProductId,
                LenderId = dto.LenderId,
                BorrowerId = borrower.UserId,
                BorrowStartDate = dto.BorrowStartDate,
                BorrowEndDate = dto.BorrowEndDate
            };
            
            await _productService.BorrowProduct(borrowDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Return(int id){
            if(await _productService.IsProductBorrowedByUser(id, Request.Cookies["access-cookie"])){
                await _productService.ReturnProduct(id);
                return RedirectToAction("Index");
            }
            else
                return NotFound();
        }

        
    }
}
