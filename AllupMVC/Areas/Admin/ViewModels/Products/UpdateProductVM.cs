using AllupMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace AllupMVC.Areas.Admin.ViewModels
{
    public class UpdateProductVM
    {
        [Required]
        public IFormFile? MainPhoto { get; set; }
        public IFormFile? HoverPhoto { get; set; }
        public List<IFormFile>? AdditionalPhotos { get; set; }
        public string Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }

        [Required]
        public int? CategoryId { get; set; }
        [Required]
        public int? BrandId { get; set; }

        public List<int>? ImageIds {  get; set; }

        public List<Category>? Categories { get; set; }
        public List<Brand>? Brands { get; set; }
        public List<ProductImage>? ProductImages { get; set; }

    }
}
