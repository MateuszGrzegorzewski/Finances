using Finances.Application.Category;
using MediatR;

namespace Finances.Application.Category.Commands.CreateCategory
{
    public class CreateCategoryCommand : CategoryDto, IRequest
    {
    }
}