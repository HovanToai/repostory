using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Model.Entity
{
   public class ProductIncategory
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
