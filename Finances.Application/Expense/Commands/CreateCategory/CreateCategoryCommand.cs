using MediatR;

namespace Finances.Application.Expense.Commands.CreateCategory
{
    public class CreateCategoryCommand : CategoryDto, IRequest
    {
    }
}