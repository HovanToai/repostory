
using ERP.ViewModels.Catalog.ProductImages;
using ERP.ViewModels.Catalog.Products;
using ERP.ViewModels.Common;
using Microsoft.AspNetCore.Http;
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

        Task<ProductViewModel> GetById( int productId, string languageId);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateStock(int productId, int addedQuantity);

        Task AddViewcount(int ProductId);


        Task<PagedResult<ProductViewModel>> GetAllPaging(GetMangeProductPagingRequest request);

        // Thêm Riềng ảnh
        Task<int> AddImage(int productId, ProductImageCreateRequest request);
        // xóa anh vừa mới thêm
        Task<int> RemoveImage(int imageId);
        // cập nhật ảnh
        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);
        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductImageViewModel>> GetListImage(int productId);
    }

}
