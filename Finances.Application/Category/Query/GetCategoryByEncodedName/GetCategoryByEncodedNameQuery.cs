using Finances.Application.Category;
using MediatR;

namespace Finances.Application.Category.Query.GetCategoryByEncodedName
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