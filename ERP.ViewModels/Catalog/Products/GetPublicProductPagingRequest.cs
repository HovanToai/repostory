using ERP.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.ViewModels.Catalog.Products
{
    public class GetPublicProductPagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
