using System;
using System.Collections.Generic;
using System.Text;
using ERP.ViewModels.Catalog.Products;

namespace ERP.ViewModels.Common
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecord { get; set; }

        public static implicit operator PagedResult<T>(PagedResult<ProductViewModel> v)
        {
            throw new NotImplementedException();
        }
    }
}
