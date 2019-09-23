using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GFS.Transfer.Order.Queries;

namespace GFS.Domain.Core
{
    public interface IOrderPdfService
    {
        Task<string> GetHtmlString(GetOrderQuery query);
    }

}
