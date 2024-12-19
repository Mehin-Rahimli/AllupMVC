
using AllupMVC.Models;

namespace AllupMVC.ViewModels
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Slide> Slides { get; set; }
        public List<Product> NewProducts { get; set; }
    }
}
