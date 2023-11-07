using MediatR;

namespace Finances.Application.Expense.Query.GetByEncodedName
{
    public class GetCategoryByEncodedNameQuery : IRequest<CategoryDto>
    {
        public string EncodedName { get; set; }

        public GetCategoryByEncodedNameQuery(string encodedName)
        {
            EncodedName = encodedName;
        }
    }
}