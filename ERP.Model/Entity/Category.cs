using ERP.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Model.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public bool IsShowOnHome { get; set; }
        public int? ParentId { get; set; }
        public Status Status { get; set; }

    }
}
