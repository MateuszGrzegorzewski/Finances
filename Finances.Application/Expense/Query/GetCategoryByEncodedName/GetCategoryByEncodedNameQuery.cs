using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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