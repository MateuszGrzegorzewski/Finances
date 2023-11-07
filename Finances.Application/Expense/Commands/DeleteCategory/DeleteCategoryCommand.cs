using MediatR;

namespace Finances.Application.Expense.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : CategoryDto, IRequest
    {
    }
}