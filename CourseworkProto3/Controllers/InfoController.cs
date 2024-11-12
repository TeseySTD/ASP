using Library.Data.Repo;
using Library.Models.DTO.InfoResponses;
using Library.Models.Entities;
using Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class InfoController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly ProductService _productService;
        private readonly UserRepository _userRepository;
        private readonly OrderService _orderService;

        public InfoController(ProductRepository productRepository,
                            UserRepository userRepository,
                            ProductService productService,
                            OrderService orderService)
        {
            _productRepository = productRepository;
            _productService = productService;
            _userRepository = userRepository;
            _orderService = orderService;
        }


        public IActionResult Index()
        {
            return View();
        }

        // Перелік знайомих, у яких на руках деяка продукція й знайомого, що раніше всіх її повинен повернути.
        public async Task<IActionResult> First()
        {
            var borrowers = await _userRepository.GetBorrowersWithBorrows();
            borrowers = borrowers.OrderBy(b => b.BorrowEndDate).ToList();
            return View(borrowers);
        }

        // Cписок і загальне число всіх знайомих-боржників, боржників із терміном більше 10 днів, по статевій ознаці.
        [HttpPost]
        public async Task<IActionResult> Second(string type, Gender gender)
        {
            var borrowers = await _userRepository.GetDebtorsWithBorrows();
            var response = new SecondInfoResponse();
            response.Type = type;
            switch (type)
            {
                case "10 днів":
                    response.Debtors = borrowers.Where(b => (DateTime.Now - b.BorrowEndDate).Days > 10).ToList();
                    break;
                case "Гендерна ознака":
                    response.Debtors = borrowers.Where(b => b.Borrower.Gender == gender).ToList();
                    break;
                default:
                    response.Debtors = borrowers;
                    break;
            }

            return View(response);
        }

        // Загальна кількість дисків за жанрами та типами, які знаходяться на даний момент у вас дома.
        [HttpPost]
        public async Task<IActionResult> Third(DiscType type, string genre)
        {
            var discs = await _productRepository.GetDiscs();
            var currentUserId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            discs = discs.Where(d => d.Product.OwnerId == currentUserId).ToList();

            if (type != 0)
            {
                discs = discs.Where(d => d.DiscType == type).ToList();
            }

            if (genre != null && genre != "")
            {
                switch (type)
                {
                    case DiscType.Movie:
                        discs = discs.Where(d => d.Movie.Genre.Any(g => g.Name == genre)).ToList();
                        break;
                    case DiscType.Music:
                        discs = discs.Where(d => d.Music.Genre.Any(g => g.Name == genre)).ToList();
                        break;
                    case DiscType.Game:
                        discs = discs.Where(d => d.Game.Genre.Any(g => g.Name == genre)).ToList();
                        break;
                    default:
                        discs = discs.Where(d =>
                            (d.Movie != null && d.Movie.Genre.Any(g => g.Name == genre)) ||
                            (d.Music != null && d.Music.Genre.Any(g => g.Name == genre)) ||
                            (d.Game != null && d.Game.Genre.Any(g => g.Name == genre))
                        ).ToList();

                        break;
                }
            }

            return View(discs);
        }

        // Перелік інформації про знайомих, яким ви нічого не заборгували та перелік знайомих чиї речі знаходяться у вас.
        [HttpGet]
        public async Task<IActionResult> Fourth()
        {
            var borrowers = await _userRepository.GetLendersWithBorrows();
            var currentUserId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            var response = new FourthInfoResponse();

            response.Owe = borrowers.Where(b => b.BorrowerId == currentUserId)
                                    .Select(b => b.Lender)
                                    .GroupBy(u => u.UserId)
                                    .Select(g => g.First())
                                    .ToList();

            var users = await _userRepository.GetAllUsers();
            response.NotOwe = users.Where(u => !response.Owe.Any(us => us.UserId == u.UserId) && u.UserId != currentUserId).ToList();

            return View(response);
        }

        // Перелік інформації про всі види друкованої та недрукованої продукції; за відповідним жанром; за відповідним форматом; про вказану продукцію.
        [HttpPost]
        public async Task<IActionResult> Fifth(ProductType productType, string genre, DiscFormat? format)
        {
            var products = await _productRepository.GetFull();

            if (productType != 0)
            {
                products = products.Where(p => p.ProductType == productType).ToList();
            }

            if (!string.IsNullOrEmpty(genre))
            {
                products = products.Where(p =>
                    (p.Book != null && p.Book.Genre.Any(g => g.Name == genre)) ||
                    (p.Disc != null && (
                        (p.Disc.Movie != null && p.Disc.Movie.Genre.Any(g => g.Name == genre)) ||
                        (p.Disc.Music != null && p.Disc.Music.Genre.Any(g => g.Name == genre)) ||
                        (p.Disc.Game != null && p.Disc.Game.Genre.Any(g => g.Name == genre))
                    ))
                ).ToList();
            }

            if (format.HasValue && productType == ProductType.Disc)
            {
                products = products.Where(p => p.Disc != null && p.Disc.Format == format.Value).ToList();
            }

            return View(products);
        }

        // Перелік жанрів фільмів, всі диски яких на даний момент знаходяться у вас (нікому не позичені).
        [HttpGet]
        public async Task<IActionResult> Sixth()
        {
            var currentUserId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            var discs = await _productRepository.GetDiscs();

            var ownedDiscs = discs.Where(d => d.Product.OwnerId == currentUserId && d.DiscType == DiscType.Movie).ToList();

            //Remove not ordered discs for root
            if (currentUserId == _userRepository.GetOwnerOfLibrary().UserId)
            {
                var discsToRemove = new List<Disc>();
                foreach (var d in ownedDiscs)
                {
                    if (await _productService.IsProductForOrder(d.Product.ProductId) &&
                        !await _productService.IsProductOrderedByUser(d.Product.ProductId, Request.Cookies["access-cookie"]))
                    {
                        discsToRemove.Add(d);
                    }
                }

                ownedDiscs = ownedDiscs.Except(discsToRemove).ToList();
            }

            //Set ordered product in result
            foreach (var d in discs)
            {
                if (await _productService.IsProductOrderedByUser(d.Product.ProductId, Request.Cookies["access-cookie"]))
                {
                    ownedDiscs.Add(d);
                }
            }


            var notBorrowed = new List<Disc>();
            foreach (var d in ownedDiscs)
            {
                if (!await _productService.IsProductBorrowed(d.ProductId) && !await _productService.IsProductWaitingForOrder(d.ProductId))
                    notBorrowed.Add(d);
            }

            var movieGenres = await _productRepository.GetMovieGenres();
            movieGenres = movieGenres
                .Select(g => new MovieGenre
                {
                    GenreId = g.GenreId,
                    Name = g.Name,
                    Movies = g.Movies.Where(m => notBorrowed.Any(nb => nb.DiscId == m.DiscId)).ToList()
                })
                .Where(g => g.Movies.Any())
                .ToList();

            return View(movieGenres);
        }

        // Отримати інформацію про заданого знайомого; про всіх знайомих, які брали у вас книжки і ще не повернули.
        [HttpGet]
        public async Task<IActionResult> Seventh()
        {
            var currentUserId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            var response = new SeventhInfoResponse();
            var borrowers = await _userRepository.GetBorrowersWithBorrows();
            borrowers = borrowers.Where(b => b.LenderId == currentUserId).ToList();
            foreach (var b in borrowers)
            {
                if ((await _productRepository.GetById(b.ProductId)).ProductType == ProductType.Book)
                {
                    response.DebtorsOfBooks.Add(b.Borrower);
                }
            }

            response.DebtorsOfBooks = response.DebtorsOfBooks.GroupBy(b => b.UserId)
                                                            .Select(g => g.First())
                                                            .ToList();

            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Seventh(string email)
        {
            var response = new SeventhInfoResponse();

            response.ConcreteUser = await _userRepository.GetUserByEmail(email);
            return View(response);
        }

        // Продукція, яка була замовлення користувачами бібліотеки; Чистий прибуток від надання в обмін продукції на замовлення за останній місяць.
        [HttpGet]
        public async Task<IActionResult> Eighth()
        {
            var orders = await _orderService.GetOrders();
            orders = orders.Where(o => o.Status != OrderStatus.Pending).ToList();
            return View(orders);
        }

        // Продукція, яка поповнила ваш каталог продукції за останній тиждень; за місяць.
        [HttpPost]
        public async Task<IActionResult> Ninth(string period)
        {
            DateTime startDate;
            var currentUserId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]);
            ViewBag.period = period;
            if (period == "week")
            {
                startDate = DateTime.Now.AddDays(-7);
            }
            else if (period == "month")
            {
                startDate = DateTime.Now.AddMonths(-1);
            }
            else
            {
                return BadRequest("Невідомий період часу.");
            }

            var recentProducts = await _productRepository.GetFull();
            recentProducts = recentProducts.Where(p => p.AddInCatalogDate >= startDate && 
                                                        p.AddInCatalogDate <= DateTime.Now &&    
                                                        p.OwnerId == currentUserId)
                                            .ToList();

            var productsToRemove = new List<Product>();
            foreach (var p in recentProducts)
            {
                if (await _productService.IsProductForOrder(p.ProductId) )
                {
                    productsToRemove.Add(p);
                }
            }

            recentProducts = recentProducts.Except(productsToRemove).ToList();
            
            return View(recentProducts);
        }

        // Інформація про всі диски і книги, які ви взяли в борг, і їх термін повернення вже минув.
        [HttpGet]
        public async Task<IActionResult> Tenth(){
            var currentUserId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]) ?? 0;
            var borrows = await _productService.GetDebtsWithFullProductsByUserId(currentUserId);

            return View(borrows);
        }
    }
}