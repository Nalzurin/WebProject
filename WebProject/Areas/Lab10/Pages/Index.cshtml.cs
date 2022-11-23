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
    public class IndexModel : PageModel
    {
        private readonly WebProject.Areas.Lab10.Data.GameContext _context;

        public IndexModel(WebProject.Areas.Lab10.Data.GameContext context)
        {
            _context = context;
        }

        public IList<Game> Game { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Games != null)
            {
                Game = await _context.Games.ToListAsync();
            }
        }
    }
}
