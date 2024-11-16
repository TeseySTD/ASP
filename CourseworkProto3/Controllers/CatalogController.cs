using Library.Data;
using Library.Data.Repo;
using Library.Models.DTO;
using Library.Models.Entities;
using Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly ProductService _productService;
        private readonly UserRepository _userRepository;
        private readonly ManyToManyService _manyToManyService;
        private readonly LibraryContext _context;

        public CatalogController(ProductRepository productRepository
                                , ProductService productService
                                , UserRepository userRepository
                                , ManyToManyService manyToManyService
                                , LibraryContext context)
        {
            _productRepository = productRepository;
            _productService = productService;
            _userRepository = userRepository;
            _manyToManyService = manyToManyService;
            _context = context;
        }

        // GET: Catalog
        public async Task<ActionResult> Index(string name, string type, string state, string owns)
        {
            List<Product> products = await _productRepository.GetFull();
            
            if(!string.IsNullOrEmpty(name))
                products = products.Where(p => p.Title.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList(); 

            if(!string.IsNullOrEmpty(type)){
                switch(type){
                    case "book":
                        products = products.Where(p => p.ProductType == ProductType.Book).ToList();
                        break;
                    case "movie":
                        products = products.Where(p => p.ProductType == ProductType.Disc && p.Disc != null && 
                                                    p.Disc.DiscType == DiscType.Movie && p.Disc.Movie != null).ToList();
                        break;
                    case "game":
                        products  = products.Where(p => p.ProductType == ProductType.Disc && p.Disc != null && 
                                                    p.Disc.DiscType == DiscType.Game && p.Disc.Game != null).ToList();
                        break;
                    case "music":
                        products = products.Where(p => p.ProductType == ProductType.Disc && p.Disc != null && 
                                                    p.Disc.DiscType == DiscType.Music && p.Disc.Music != null).ToList();
                        break;
                }
            }

            if(!string.IsNullOrEmpty(state)){
                switch(state){
                    case "borrowed":
                        var borrowedProducts = new List<Product>(products);
                        foreach(var product in products){
                            if(!await _productService.IsProductBorrowed(product.ProductId)){
                                borrowedProducts.Remove(product);
                            }
                        }
                        products = borrowedProducts;
                        break;
                    case "ordered":
                        var orderedProducts = new List<Product>(products);
                        foreach(var product in products){
                            if(!await _productService.IsProductOrdered(product.ProductId)){
                                orderedProducts.Remove(product);
                            }
                        }
                        products = orderedProducts;
                    break;
                }
            }

            if(!string.IsNullOrEmpty(owns) && owns == "on"){
                products = products.Where(p => p.OwnerId == JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"])).ToList();
            }

            return View(products);
        }
        
        [Authorize(Policy = "Operator")]
        public async Task<ActionResult> Delete(int id){
            await _productRepository.DeleteById(id);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "Operator")]
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

        [Authorize(Policy = "Operator")]
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

        [Authorize(Policy = "Operator")]
        [HttpGet]
        public IActionResult AddBook(){
            return View();
        }
        [Authorize(Policy = "Operator")]
        [HttpPost]
        public async Task<IActionResult> AddBook(ProductDto dto){
            dto.OwnerId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            await _productRepository.AddBook(dto);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "Operator")]
        [HttpGet]
        public IActionResult AddMovie(){
            return View();
        }
        [Authorize(Policy = "Operator")]
        [HttpPost]
        public async Task<IActionResult> AddMovie(ProductDto dto){
            dto.OwnerId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            await _productRepository.AddMovie(dto);
            return RedirectToAction("Index");
        }


        [Authorize(Policy = "Operator")]
        [HttpGet]
        public IActionResult AddMusic(){
            return View();
        }
        [Authorize(Policy = "Operator")]
        [HttpPost]
        public async Task<IActionResult> AddMusic(ProductDto dto){
            dto.OwnerId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            await _productRepository.AddMusic(dto);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "Operator")]
        [HttpGet]
        public IActionResult AddGame(){
            return View();
        }
        [Authorize(Policy = "Operator")]
        [HttpPost]
        public async Task<IActionResult> AddGame(ProductDto dto){
            dto.OwnerId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            await _productRepository.AddGame(dto);
            return RedirectToAction("Index");
        }

        [Authorize]
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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Borrow(BorrowDto dto){
            await _productService.BorrowProduct(dto);
            return RedirectToAction("Index");
        }

        [Authorize]
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
        [Authorize]
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

        [Authorize] 
        [HttpGet]
        public async Task<IActionResult> Return(int id){
            if(await _productService.IsProductBorrowedByUser(id, Request.Cookies["access-cookie"])){
                await _productService.ReturnProduct(id);
                return RedirectToAction("Index");
            }
            else
                return NotFound();
        }

        public async Task<IActionResult> ActorsGenres(){
            var tuple = await _manyToManyService.GetGenresAndActors();
            return View(tuple);
        }

        [HttpGet]
        public async Task<IActionResult> EditActorOrGenre(int id, string type)=> type switch{
            "actor" => await _context.Actors.AnyAsync(a => a.ActorId == id) ? 
                        View("EditActorOrGenre", 
                            new EditActorOrGenreRequest{Type = type, Id = id, Name = _context.Actors.FirstOrDefault(a => a.ActorId == id)!.Name})
                        : NotFound(),
            "book"=> await _context.BookGenres.AnyAsync(b => b.GenreId == id) ? 
                        View("EditActorOrGenre", 
                            new EditActorOrGenreRequest{Type = type, Id = id, Name = _context.BookGenres.FirstOrDefault(a => a.GenreId == id)!.Name})
                        : NotFound(),
            "movie" => await _context.MovieGenres.AnyAsync(m => m.GenreId == id) ?                         
                        View("EditActorOrGenre", 
                            new EditActorOrGenreRequest{Type = type, Id = id, Name = _context.MovieGenres.FirstOrDefault(a => a.GenreId == id)!.Name})
                        : NotFound(),
            "music" => await _context.MusicGenres.AnyAsync(m => m.GenreId == id) ?                      
                        View("EditActorOrGenre", 
                            new EditActorOrGenreRequest{Type = type, Id = id, Name = _context.MusicGenres.FirstOrDefault(a => a.GenreId == id)!.Name})
                        : NotFound(),
            "game" => await _context.GameGenres.AnyAsync(g => g.GenreId == id) ?                      
                        View("EditActorOrGenre", 
                            new EditActorOrGenreRequest{Type = type, Id = id, Name = _context.GameGenres.FirstOrDefault(a => a.GenreId == id)!.Name})
                        : NotFound(),
            _ => BadRequest()
        };
        
        [HttpPost]
        public async Task<IActionResult> EditActorOrGenre(EditActorOrGenreRequest request){
            await _manyToManyService.Update(request);
            return RedirectToAction("ActorsGenres");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteActorOrGenre(int id, string type){
            if(!await _manyToManyService.ActorOrGenreExists(id)) return NotFound();
            await _manyToManyService.Delete(id, type);
            return RedirectToAction("ActorsGenres");
        }

    }
}
