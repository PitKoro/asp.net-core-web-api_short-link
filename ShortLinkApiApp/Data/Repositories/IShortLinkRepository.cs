using ShortLinksApiApp.Data.Models;

namespace ShortLinksApiApp.Data.Repositories
{
    public interface IShortLinkRepository : IDisposable
    {
        Task<List<ShortLink>> GetShortLinkAsync();
        Task<ShortLink> GetShortLinkAsync(int shortLinkId);
        Task InsertShortLinkAsync(string link);
        Task UpdateShortLinkAsync(ShortLink shortLink);
        Task DeleteShortLinkAsync(int shortLinkId);
        Task SaveAsync();
    }
}
