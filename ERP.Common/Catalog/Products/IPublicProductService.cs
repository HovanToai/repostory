
using ERP.ViewModels.Catalog.Products;
using ERP.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Common.Catalog.Products
{
   public interface IPublicProductService
    {
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);

        //Task<List<ProductViewModel>> GetAll(string languageId);
    }
}
