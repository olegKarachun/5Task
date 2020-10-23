using _5Task.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Diagnostics;
using _5Task.Controllers;

namespace _5Task.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        MainContext db;
        IHubContext<GameHub> hubContext;
        public GameController(MainContext context, IHubContext<GameHub> hubContext)
        {
            db = context;
            this.hubContext = hubContext;
        }
        public IActionResult Field(string name, string move, string join, string[] tags)
        {
            ViewBag.Tags = tags;
            ViewBag.Join = join;
            ViewBag.Name = name;
            ViewBag.TypeOfMove = move;            
            return View();
        }         
    }
}
