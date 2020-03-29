using ERP.Common.Catalog.Products.Dtos;
using ERP.Common.Catalog.Products.Dtos.Manage;
using ERP.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Common.Catalog.Products
{
    //Admin
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);


        Task<int> Delete(int productId);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateStock(int productId, int addedQuantity);

        Task AddViewcount(int ProductId);


        Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);
    }
}
