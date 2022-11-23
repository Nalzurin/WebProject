using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages
{
    [Authorize]
    public class Lab8Model : PageModel
    {
        public void OnGet()
        {
            HttpContext.Response.Headers.Add("REMOTE_AGENT", "Data");
        }
        public void OnPost()
        {
            HttpContext.Response.Headers["REMOTE_AGENT"] = Request.Form["lab8"];
        }
    }
}
