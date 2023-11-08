using Finances.Application.Category;
using MediatR;

namespace Finances.Application.Category.Query.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
    {
    }
}