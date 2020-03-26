﻿using ERP.Model.Entity;
using ERP.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Model.Extension
{
   public static class ModelBuilderExtensions
    {
        public static void seed(this ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<AppConfig>().HasData(
                new AppConfig() { Key = "HomeTitle", Value = "this is home page of eshopSolution"},
                new AppConfig() { Key = "HomeKeyword", Value = "this is keyword of eshopSolution" },
                new AppConfig() {  Key = "HomeDescription", Value = "this is description of eshopSolution"});
           
            modelBuilder.Entity<Language>().HasData(
            new Language(){ Id = "vi-VN", Name = "Tiếng Việt",IsDefault = true },
            new Language() { Id = "en-US", Name = "English", IsDefault = false});

            modelBuilder.Entity<Category>().HasData(
                new Category() 
                {
                    Id = 1,
                    ParentId = null,
                    IsShowOnHome = true,
                    SortOrder = 1, 
                    Status = Status.Active 
                },
                new Category()
                {
                    Id = 2,
                    ParentId = null,
                    IsShowOnHome = true,
                    SortOrder = 2,
                    Status = Status.Active
                });

            modelBuilder.Entity<CategoryTranslation>().HasData(
                new CategoryTranslation()
                {
                    Id = 1,
                    CategoryId = 1,
                    Name = "Áo nam",
                    LanguageId = "vi-VN",
                    SeoAlias = "ao-nam",
                    SeoDescription = "Sản phẩm áo thời trang nam",
                    SeoTitle = "Sản phẩm áo thời trang nam"
                },
                new CategoryTranslation()
                {
                    Id = 2,
                    CategoryId = 1,
                    Name = "Men Shirt",
                    LanguageId = "en-US",
                    SeoAlias = "men-shirt",
                    SeoDescription = "the shirt products for man",
                    SeoTitle = "the shirt products for man"
                },
                new CategoryTranslation()
                {
                    Id = 3,
                    CategoryId = 2,
                    Name = "Áo nữ",
                    LanguageId = "vi-VN",
                    SeoAlias = "ao-nu",
                    SeoDescription = "Sản phẩm áo thời trang nữ",
                    SeoTitle = "Sản phẩm áo thời trang nữ"
                },
                new CategoryTranslation()
                {
                    Id = 4,
                    CategoryId = 2,
                    Name = "women Shirt",
                    LanguageId = "en-US",
                    SeoAlias = "women-shirt",
                    SeoDescription = "the shirt products for momen",
                    SeoTitle = "the shirt products for women"
                });


             modelBuilder.Entity<Product>().HasData(
                new Product()
                {  
                    Id = 1,
                    DateCreated = DateTime.Now,
                    OriginalPrice = 100000, 
                    Price = 200000, 
                    Stock = 0, 
                    ViewCount = 0,     
                });
            modelBuilder.Entity<ProductTranslation>().HasData(
                 new ProductTranslation()
                 {
                     Id = 1,
                     ProductId = 1,
                     Name = "Áo sơ mi nam trắng Việt Tiến",
                     LanguageId = "vi-VN",
                     SeoAlias = "ao-so-mi-nam-trang-viet-tien",
                     SeoDescription = "Áo sơ mi nam trắng Việt Tiến",
                     SeoTitle = "Áo sơ mi nam trắng Việt Tiến",
                     Details = "Áo sơ mi nam trắng Việt Tiến",
                     Description = ""
                 },
                 new ProductTranslation()
                 {
                     Id = 2,
                     ProductId = 1,
                     Name = "Viet tien Men T-Shirt",
                     LanguageId = "en-US",
                     SeoAlias = "viet-tien-men-t-shirt",
                     SeoDescription = "Viet tien Men T-Shirt",
                     SeoTitle = "Viet tien Men T-Shirt",
                     Details = "Viet tien Men T-Shirt",
                     Description = ""
                 });
            modelBuilder.Entity<ProductIncategory>().HasData(
                new ProductIncategory() { CategoryId = 1 , ProductId = 1}
                );
        }
    }
}
