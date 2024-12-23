using AllupMVC.Models;
using AllupMVC.ViewModels;

namespace AllupMVC.Services.Interfaces
{
    public interface IBasketService
    {
       Task<List<BasketItemVM>>GetBasketAsync();
    }
}
