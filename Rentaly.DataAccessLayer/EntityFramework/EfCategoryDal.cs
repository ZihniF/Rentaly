using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.EntityFramework;

public class EfCategoryDal : GenericRepository<Category>, ICategoryDal
{
    private readonly RentalyContext _context;

    public EfCategoryDal(RentalyContext context) : base(context) => _context = context;

    public Task<List<Category>> GetActiveListAsync() => _context.Categories
        .AsNoTracking()
        .Where(x => x.IsActive)
        .OrderBy(x => x.CategoryName)
        .ToListAsync();

    public async Task ArchiveAsync(int id)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var category = await _context.Categories
            .FirstOrDefaultAsync(x => x.CategoryId == id && x.IsActive)
            ?? throw new KeyNotFoundException("Kategori kaydı bulunamadı.");

        category.IsActive = false;
        var categoryCars = await _context.Cars.Where(x => x.CategoryId == id).ToListAsync();
        foreach (var car in categoryCars)
        {
            car.IsActive = false;
            car.IsAvailable = false;
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}
