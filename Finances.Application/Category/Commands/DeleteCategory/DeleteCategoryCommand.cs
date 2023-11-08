using Finances.Application.Category;
using MediatR;

namespace Finances.Application.Category.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : CategoryDto, IRequest
    {
    }
}