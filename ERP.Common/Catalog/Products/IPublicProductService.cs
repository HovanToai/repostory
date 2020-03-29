using ERP.Common.Catalog.Products.Dtos;
using ERP.Common.Catalog.Products.Dtos.Public;
using ERP.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Common.Catalog.Products
{
   public interface IPublicProductService
    {
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetProductPagingRequest request);
    }
}
