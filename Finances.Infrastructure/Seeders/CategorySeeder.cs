using Finances.Infrastructure.Persistence;

namespace Finances.Infrastructure.Seeders
{
    public class CategorySeeder
    {
        private readonly ExpenseDbContext _dbContext;

        public CategorySeeder(ExpenseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                if (!_dbContext.Categories.Any())
                {
                    var trainingCategory = new Domain.Entities.Category()
                    {
                        Name = "Training"
                    };
                    trainingCategory.EncodeName();

                    var foodCategory = new Domain.Entities.Category()
                    {
                        Name = "Food"
                    };
                    foodCategory.EncodeName();

                    var healthCategory = new Domain.Entities.Category()
                    {
                        Name = "Health"
                    };
                    healthCategory.EncodeName();

                    var billsCategory = new Domain.Entities.Category()
                    {
                        Name = "Bills"
                    };
                    billsCategory.EncodeName();

                    var houseCategory = new Domain.Entities.Category()
                    {
                        Name = "House"
                    };
                    houseCategory.EncodeName();

                    var clothesCategory = new Domain.Entities.Category()
                    {
                        Name = "Clothes"
                    };
                    clothesCategory.EncodeName();

                    var transportCategory = new Domain.Entities.Category()
                    {
                        Name = "Transport"
                    };
                    transportCategory.EncodeName();

                    var leisureCategory = new Domain.Entities.Category()
                    {
                        Name = "Leisure"
                    };
                    leisureCategory.EncodeName();

                    var othersCategory = new Domain.Entities.Category()
                    {
                        Name = "Training"
                    };
                    othersCategory.EncodeName();

                    _dbContext.Categories.Add(trainingCategory);
                    _dbContext.Categories.Add(foodCategory);
                    _dbContext.Categories.Add(healthCategory);
                    _dbContext.Categories.Add(billsCategory);
                    _dbContext.Categories.Add(houseCategory);
                    _dbContext.Categories.Add(clothesCategory);
                    _dbContext.Categories.Add(transportCategory);
                    _dbContext.Categories.Add(leisureCategory);
                    _dbContext.Categories.Add(othersCategory);

                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}