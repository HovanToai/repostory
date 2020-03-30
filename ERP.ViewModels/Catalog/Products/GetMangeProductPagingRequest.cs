using ERP.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.ViewModels.Catalog.Products
{
   public class GetMangeProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
