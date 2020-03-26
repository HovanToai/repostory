using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Model.Entity
{
    public enum OrderStatus
    {
        InProgress,
        Confirmed,
        Shipping,
        Success,
        Canceled
    }
}
