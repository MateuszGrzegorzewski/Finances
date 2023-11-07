using MediatR;

namespace Finances.Application.Expense.Query.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
    {
    }
}