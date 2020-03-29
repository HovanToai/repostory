using ERP.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Common.Catalog.Products.Dtos.Manage
{
  public class GetProductPagingRequest : PagingRequestBase
    {
        public String Keyword { get; set;}

        public List<int> CategoryIds { get; set; }

    }
}
