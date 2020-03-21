using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Model.Entity
{
    public class Cart
    {
        public int Id { set; get; }
        public int ProductId { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }
    }
}
