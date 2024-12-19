using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AllupMVC.Services.Interfaces
{
    public interface ILayoutService
    {
        Task<Dictionary<string, string>> GetSettingsAsync();
    }
}
