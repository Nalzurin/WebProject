using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebProject.Areas.Lab10.Data;

namespace WebProject.Areas.Lab10.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly WebProject.Areas.Lab10.Data.GameContext _context;

        public DetailsModel(WebProject.Areas.Lab10.Data.GameContext context)
        {
            _context = context;
        }

      public Game Game { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }
            else 
            {
                Game = game;
            }
            return Page();
        }
    }
}
