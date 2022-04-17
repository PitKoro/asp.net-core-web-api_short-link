using Microsoft.EntityFrameworkCore;
using ShortLinksApiApp.Data.Context;
using ShortLinksApiApp.Data.Models;
using System.Data.Common;
using System.Text;

namespace ShortLinksApiApp.Data.Repositories
{
    public class ShortLinkRepository : IShortLinkRepository
    {
        private readonly AppDbContext _context;
        public ShortLinkRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ShortLink>> GetShortLinkAsync() => await _context.ShortLinks.ToListAsync();
        public async Task<ShortLink> GetShortLinkAsync(int shortLinkId) =>
            await _context.ShortLinks.FindAsync(new object[] { shortLinkId });
        public async Task UpdateShortLinkAsync(ShortLink shortLink)
        {
            var shortLinkFromDb = await _context.ShortLinks.FindAsync(new object[] { shortLink.Id });
            if (shortLinkFromDb == null) throw new DbUpdateException("database entry not found");
            shortLinkFromDb.Link = shortLink.Link;
            shortLinkFromDb.Code = shortLink.Code;
        }
        public async Task InsertShortLinkAsync(string link)
        {
            ShortLink shortLink = new ShortLink { Link = link, Code = RandomString(6) };
            await _context.ShortLinks.AddAsync(shortLink);
        }
        public async Task DeleteShortLinkAsync(int shortLinkId)
        {
            var shortLinkFromDb = await _context.ShortLinks.FindAsync(new object[] { shortLinkId });
            if (shortLinkFromDb == null) throw new DbUpdateException("database entry not found");
            _context.ShortLinks.Remove(shortLinkFromDb);
        }
        public async Task SaveAsync() => await _context.SaveChangesAsync();

        private bool _disposed = false;
        protected virtual void Dispose(bool disopsing)
        {
            if (!_disposed)
            {
                if (disopsing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private readonly Random _random = new Random();
        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):   
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
