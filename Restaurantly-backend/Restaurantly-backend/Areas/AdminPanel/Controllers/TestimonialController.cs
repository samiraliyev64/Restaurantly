using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Restaurantly_backend.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Restaurantly_backend.Helpers;
using Restaurantly_backend.Models;

namespace Restaurantly_backend.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class TestimonialController : Controller
    {
        private AppDbContext _context { get; }
        private IWebHostEnvironment _env { get; }
        public TestimonialController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Read
        public IActionResult Index()
        {
            return View(_context.Testimonials.Where(t=>!t.isDeleted).ToList());
        }
        //Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }
            var testimonial = _context.Testimonials.Find(id);
            if(testimonial == null)
            {
                return NotFound();
            }
            var path = Helper.GetPath(_env.WebRootPath, "img","testimonials",testimonial.Url);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            testimonial.isDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Create (GET)
        public IActionResult Create()
        {
            return View();
        }
        //Create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testimonial testimonial)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!testimonial.Photo.CheckFileSize(500))
            {
                ModelState.AddModelError("Photo", "File size must be less than 500 KB");
                return View();
            }
            if (!testimonial.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File type must be image");
                return View();
            }
            testimonial.Url = await testimonial.Photo.SaveFileAsync(_env.WebRootPath, "img","testimonials");
            await _context.Testimonials.AddAsync(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Update (GET)
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var testimonial = _context.Testimonials.Find(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            return View(testimonial);
        }
        //Update (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Testimonial testimonial)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Testimonial testimonialDb = _context.Testimonials.Find(id);
            if(testimonialDb == null)
            {
                return NotFound();
            }
            testimonial.Url = await testimonial.Photo.SaveFileAsync(_env.WebRootPath, "img", "testimonials");
            var pathDb = Helper.GetPath(_env.WebRootPath, "img", "testimonials", testimonialDb.Url);
            testimonialDb.Name = testimonial.Name;
            testimonialDb.Position = testimonial.Position;
            testimonialDb.Description = testimonial.Description;
            if (System.IO.File.Exists(pathDb))
            {
                System.IO.File.Delete(pathDb);
            }
            testimonialDb.Url = testimonial.Url;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
