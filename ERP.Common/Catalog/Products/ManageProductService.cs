using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ERP.Common.Common;
using ERP.Model.EF;
using ERP.Model.Entity;
using ERP.Utilities.Exceptions;
using ERP.ViewModels.Catalog.ProductImages;
using ERP.ViewModels.Catalog.Products;
using ERP.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ERP.Common.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ManageProductService(EShopDbContext context,IStorageService storageService)
        {

            _context = context;
            _storageService = storageService;
        }

        public async Task AddViewcount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                new ProductTranslation()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Details = request.Details,
                    SeoDescription = request.SeoDescription,
                    SeoAlias = request.SeoAlias,
                    SeoTitle = request.SeoTitle,
                    LanguageId = request.LanguageId
                
                }
             }
            };
            // save image
            if (request.ThumBnaiImage != null)
            {
                product.ProductImages = new List<ProductImage>()
            {
                new ProductImage()
                {
                    Caption = "Thumbnail image",
                    DateCreated = DateTime.Now,
                    FileSize = request.ThumBnaiImage.Length,
                    ImagePath = await this.SaveFile(request.ThumBnaiImage),
                    IsDefault = true,
                    SortOrder = 1
                }
            };
            }
            _context.Products.Add(product);
           await _context.SaveChangesAsync();
           return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EshopException($"cannot find a product: {productId}");

            var images =  _context.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
              await _storageService.DeleteFileAsync(image.ImagePath);

            }
            _context.Products.Remove(product);

            //delete File rác
            

            return await _context.SaveChangesAsync();

        }
        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetMangeProductPagingRequest request)
        {
            //1. select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductIncategories on pt.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id

                        select new { p, pt, pic };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            if (request.CategoryIds.Count > 0) 
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryId));
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            var data =  query.Skip((request.PageIndex - 1)* request.Pagesize)
                .Take(request.Pagesize)
                .Select(x=>new ProductViewModel()
               {
                Id = x.p.Id,
                Name = x.pt.Name,
                DateCreated = x.p.DateCreated,
                Description = x.pt.Description,
                Details = x.pt.Details,
                LanguageId = x.pt.LanguageId,
                OriginalPrice = x.p.OriginalPrice,
                Price = x.p.Price,
                SeoAlias = x.pt.SeoAlias,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount

               }).ToListAsync();

            // 4. select and projecttion

            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items =  await data
            };
            return pagedResult;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);

            var ProductTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id 
            && x.LanguageId == request.LanguageId);
            if (product == null || ProductTranslations == null) throw new EshopException($"cannot find a product: {request.Id}");

            ProductTranslations.Name = request.Name;
            ProductTranslations.SeoAlias = request.SeoAlias;
            ProductTranslations.SeoDescription = request.SeoDescription;
            ProductTranslations.SeoTitle = request.SeoTitle;
            ProductTranslations.Description = request.Description;
            ProductTranslations.Details = request.Details;

            // save image
            if (request.ThumBnaiImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                  thumbnailImage.FileSize = request.ThumBnaiImage.Length;
                  thumbnailImage.ImagePath = await this.SaveFile(request.ThumBnaiImage);
                    _context.ProductImages.Update(thumbnailImage);

                }
            
            }

           return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EshopException($"cannot find a product: {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() >0;

        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EshopException($"cannot find a product: {productId}");
            product.Stock  += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }

        // Image
        public async Task<String> SaveFile(IFormFile file)
        {
            var originaFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originaFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);

            return fileName;
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && languageId == languageId);
            
            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation.LanguageId,
                Details = productTranslation != null ? productTranslation.Details : null,
                Name = productTranslation != null ? productTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount
            };
            return productViewModel;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;

        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EshopException($"cannot find an image with id {imageId}");
            _context.ProductImages.Remove(productImage);
           return  await _context.SaveChangesAsync();

        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EshopException($"cannot find an image with id {imageId}");
            
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel() 
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new EshopException($"cannot find an image with id {imageId}");
            var viewModel = new ProductImageViewModel()
                {
                    Caption = image.Caption,
                    DateCreated = image.DateCreated,
                    FileSize = image.FileSize,
                    Id = image.Id,
                    ImagePath = image.ImagePath,
                    IsDefault = image.IsDefault,
                    ProductId = image.ProductId,
                    SortOrder = image.SortOrder
                };
            return viewModel;
        }
    }
}
