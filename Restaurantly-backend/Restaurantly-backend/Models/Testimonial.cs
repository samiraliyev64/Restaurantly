using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurantly_backend.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public bool isDeleted { get; set; }
        [Required,NotMapped]
        public IFormFile Photo { get; set; }
    }
}
