using AllupMVC.Models;
using Azure;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace AllupMVC.Areas.Admin.ViewModels
{
    public class UpdateProductVM
    {
        public IFormFile? MainPhoto { get; set; }
        public IFormFile? HoverPhoto { get; set; }
        public List<IFormFile>? AdditionalPhotos { get; set; }
        public string Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }

        [Required]
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }


        public List<int>? ImageIds { get; set; }

        public List<Category>? Categories { get; set; }
        public List<Brand>? Brands { get; set; }

        public List<ProductImage>? ProductImages { get; set; }


    }
}
