using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _5Task.Models;
using Microsoft.AspNetCore.Authorization;
using _5Task.ViewModels;

namespace _5Task.Controllers
{
    public class HomeController : Controller
    {
        static List<Game> games = new List<Game>();
        private readonly ILogger<HomeController> _logger;
        readonly MainContext db;        

        public HomeController(ILogger<HomeController> logger, MainContext context)
        {
            _logger = logger;
            db = context;            
        }

        [Authorize]
        public IActionResult Index()
        {                       
            ViewBag.Games = db.Games;
            return View();
        }
        
        public List<string> SplitBy(List<string> Tags)
        {            
            List<string> tags = new List<string>();
            foreach(var s in Tags)
            {
                if (s != null)
                {
                    var TagsInGame = s.Split("#").ToList();
                    foreach (string tag in TagsInGame)
                    {
                        tags.Add(tag);
                        Trace.WriteLine(tag);
                    }
                }                             
            }
            return tags;
        }
        
        [Authorize]
        [HttpGet]
        public string[] GetTags()
        {
            var Tags = SplitBy(db.Games.Select(p => p.Tags).ToList()).ToArray();
            Tags = Tags.Where(val => val != "").ToArray();
            foreach (var i in Tags)
            {
                Trace.WriteLine(i);
            }
            return Tags;            
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNewGame(NewGameModel model)
        {
            if (ModelState.IsValid)
            {
                Game game = new Game { Name = model.Name, Tags = model.Tags };
                db.Games.Add(game);
                games.Add(game);
                await db.SaveChangesAsync();                
                return AddPlayer(game, true);
            }
            return RedirectToAction("Index", "Home", model);
        }

        [Authorize]
        public IActionResult AddPlayer(Game game, bool flag)
        {
            var Tags = db.Games.Select(p => p.Tags).ToArray();            
            string TypeOfMove;            
            AddPlayerToGame(game, User.Identity.Name, out TypeOfMove);
            if (flag)
            {
                return RedirectToAction("Field", "Game", new { name = game.Name, move = TypeOfMove, join = "new", tags = Tags }); ;
            }
            else
            {
                return RedirectToAction("Field", "Game", new { name = game.Name, move = TypeOfMove, join = "join", tags = Tags });
            }
            
        }

        [Authorize]
        public async Task<IActionResult> JoinTheGame(string name)
        {
            Game game = db.Games.FirstOrDefault(u => u.Name == name);
            if (game.Player1 == null || game.Player2 == null)
            {                
                return AddPlayer(game, false);
            }
            else
            {
                return RedirectToAction("index");
            }
        }

        [Authorize]
        public void AddPlayerToGame(Game game, string userEmail, out string TypeOfMove)
        {            
            if(game.Player1 == null)
            {
                game.Player1 = userEmail;
                TypeOfMove = "x";                            
            }
            else if (game.Player2 == null)
            {
                game.Player2 = userEmail;
                TypeOfMove = "o";                
            }
            else
            {
                TypeOfMove = "";
            }
            db.SaveChanges();
        }

        [Authorize]        
        public IActionResult RemovePlayerFromGame(string name)
        {            
            Game CurrentGame = db.Games.FirstOrDefault(g => g.Name == name);
            if(CurrentGame.Player1 == User.Identity.Name)
            {
                CurrentGame.Player1 = null;                
            } else if (CurrentGame.Player2 == User.Identity.Name)
            {
                CurrentGame.Player2 = null;                
            }
            db.SaveChanges();
            CheckOnEmptyGame(CurrentGame);            
            return RedirectToAction("Index", "Home");
        }        

        public void CheckOnEmptyGame(Game game)
        {
            if(game.Player1 == null && game.Player2 == null)
            {
                db.Games.Remove(game);
                games.Remove(game);
                db.SaveChanges();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
