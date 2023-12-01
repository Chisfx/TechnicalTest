using AspNetCoreHero.Abstractions.Domain;
namespace TechnicalTest.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Description { get; set; } = default!;
        public double Price { get; set; }
    }
}
