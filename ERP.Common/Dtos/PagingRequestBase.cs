using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Common.Dtos
{
  public class PagingRequestBase
    {
        public int PageIndex { get; set; }
        public int Pagesize { get; set; }
    }
}
