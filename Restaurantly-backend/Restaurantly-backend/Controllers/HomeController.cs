using Microsoft.AspNetCore.Mvc;
using Restaurantly_backend.DAL;
using Restaurantly_backend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurantly_backend.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context { get; }
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeViewModel home = new HomeViewModel
            {
                Testimonials = _context.Testimonials.Where(t => !t.isDeleted).ToList()
            };
            return View(home);
        }
    }
}
