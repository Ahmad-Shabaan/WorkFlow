using Application.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors
{
    public static class OrderErrors
    {
        public static readonly NotFoundError NotFound = new("Order.NotFound", "Order was not found");
    }
}


// or can cresate record for each error type and ignore creating types of errors like NotFoundError & ValidationError
//public sealed record OrderNotFound()
//    : Error(
//        "Order.NotFound",
//        "Order was not found.");

//public sealed record OrderAlreadyPaid()
//    : Error(
//        "Order.AlreadyPaid",
//        "Order has already been paid.");