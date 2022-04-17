using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShortLinksApiApp.Data.Models;

namespace ShortLinksApiApp.Data.EntityTypeConfiguration
{
    public class ShortLinkConfiguration : IEntityTypeConfiguration<ShortLink>
    {
        public void Configure(EntityTypeBuilder<ShortLink> builder)
        {
            builder.HasKey(shortLink => shortLink.Id);
            builder.HasIndex(shortLink => shortLink.Id).IsUnique();
            builder.Property(shortLink => shortLink.Code).IsRequired().HasMaxLength(6);
            builder.Property(shortLink => shortLink.Link).IsRequired();

            builder.HasData(
            new ShortLink[]
            {
                new ShortLink { Id=1, Link="vk.com", Code="Qefdss"},
                new ShortLink { Id=2, Link="Kinopoisk.ru", Code="sdfsse"},
                new ShortLink { Id=3, Link="Ivi.ru", Code="sdfwer"}
            });
        }
    }
}
