using AllupMVC.Models;

namespace AllupMVC.ViewModels
{
    public class ShopVM
    {
        public List<ProductGetVM> Products { get; set; }
        public List<BrandGetVM>Brands { get; set; }
        public List<CategoryGetVM> Categories { get; set; }

        public string Search {  get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public double TotalPage { get; set; }
        public int CurrentPage { get; set; }
    }
}
