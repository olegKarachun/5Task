using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using _5Task.Models;
using Microsoft.AspNetCore.Mvc;

namespace _5Task.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AutocompleteController : Controller
    {
        private readonly MainContext db;
        
        public AutocompleteController(MainContext context)
        {
            db = context;
        }

        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {            
            string term = HttpContext.Request.Query["term"].ToString();
            var Tags = db.Games.Where(p => p.Tags.Contains(term)).Select(p => p.Tags).ToList();            
            return Ok(SplitBy(Tags, term));            
        }
        public List<string> SplitBy(List<string> Tags, string term)
        {            
            List<string> tags = new List<string>();
            foreach(string s in Tags)
            {
                var TagsInGame = s.Split("#").ToList();
                foreach(string tag in TagsInGame)
                {
                    if (tag.ToLower().Contains(term.ToLower())){
                        tags.Add(tag);
                    }                    
                }                
            }
            return tags;
        }
    }
}
