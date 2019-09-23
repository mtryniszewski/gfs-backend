using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GFS.Core;
using GFS.Data.EFCore;
using GFS.Domain.Core;
using GFS.Transfer.Order.Queries;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace GFS.Domain.Impl
{
    public class OrderPdfService : IOrderPdfService
    {
        private readonly GfsDbContext _context;
        private readonly Dictionary _dictionary;
        private readonly IOrderService _orderService;

        public OrderPdfService(GfsDbContext context, IOptions<Dictionary> dictionary, IOrderService orderService)
        {
            _context = context;
            _dictionary = dictionary.Value;
            _orderService = orderService;
        }

        public async Task<string> GetHtmlString(GetOrderQuery query)
        {
            var details = await _orderService.ShowOrderDetailsAsync(new GetOrderQuery
            {
                Id = query.Id
            });

            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                            <div class='header'>");
            sb.AppendFormat(@"
                                <img src='C:\Users\Administrator\source\repos\Marek\GFS.Web\assets\smalllogo.png'/>
                                <h1>Zamówienie nr {0}</h1>
                                <p>Zamawiający: {1} {2}</p>
                                <p>Data zamówienia: {3}</p>
                            </div>", details.Id,details.User.Name, details.User.Surname, details.Date.ToShortDateString());

            if (details.FurnituresDetailsDtos.Any())
            {

                foreach (var det in details.FurnituresDetailsDtos)
                {
                    sb.AppendFormat(@"<div class='furnitureInfo'><fieldset>
                                        <legend>Wymiary podawane są w mm</legend>
                                        Szafka - {0} typu {1}
                                        </fieldset></div>", det.Name, _dictionary.GetFurnitureType(det.FurnitureType));

                    if (det.RectangularFormatterDtos.Any())
                    {
                      
                        sb.Append(@"
                        <h2>Formatki prostokątne</h2>
                        <table class='rectangularFormatter'>
                                    <tr class='items'>
                                        <th rowspan='2'>ELEMENT</th>
                                        <th rowspan='2'>ILOŚĆ</th>
                                        <th rowspan='2'>SZEROKOŚĆ</th>
                                        <th rowspan='2'>DŁUGOŚĆ</th>
                                        <th rowspan='2'>MATERIAŁ</th>
                                        <th colspan='4'>OKLEJANIE</th>  
                                        <th colspan='4'>FREZOWANIE</th>
                                    </tr>
                                    <tr class='items'>
                                         <td>Góra</td>
                                         <td>Dół</td>
                                         <td>Lewa</td>
                                         <td>Prawa</td>
                                         <td>Typ</td>
                                         <td>Długość</td>
                                         <td>Głębokość</td>
                                         <td>Szerokość</td>
                                    </tr>");
                        
                        foreach (var rectangularFormatter in det.RectangularFormatterDtos)
                        {
                            sb.AppendFormat(@"<tr>
                                                <td>{0}</td>
                                                <td>{1}</td>
                                                <td>{2}</td>
                                                <td>{3}</td>
                                                <td>({4}) {5} {6}mm</td>
                                                <td>{7}</td>
                                                <td>{8}</td>
                                                <td>{9}</td>
                                                <td>{10}</td>
                                                <td>{11}</td>
                                                <td>{12}</td>
                                                <td>{13}</td>
                                                <td>{14}</td>
                                      </tr>", rectangularFormatter.Name, rectangularFormatter.Count,
                                rectangularFormatter.Width, rectangularFormatter.Length,
                                rectangularFormatter.Fabric.ProducerCode, rectangularFormatter.Fabric.Name,
                                rectangularFormatter.Fabric.Thickness, rectangularFormatter.TopBorderThickness,
                                rectangularFormatter.BottomBorderThickness, rectangularFormatter.LeftBorderThickness,
                                rectangularFormatter.RightBorderThickness,
                                _dictionary.GetMillingType(rectangularFormatter.Milling),
                                rectangularFormatter.CutterLength,
                                rectangularFormatter.CutterDepth, rectangularFormatter.CutterWidth);
                        }
                        sb.Append(@"</table>");
                    }

                    if (det.LFormatterDtos.Any())
                    {
                        sb.Append(@"
                        <h2>Formatki w kształce litery L</h2>
                        <table class='lFormatter'>
                                    <tr class='items'>
                                        <th rowspan='2'>ELEMENT</th>
                                        <th rowspan='2'>ILOŚĆ</th>
                                        <th colspan='2'>SZEROKOŚĆ</th>
                                        <th colspan='2'>GŁĘBOKOŚĆ</th>
                                        <th colspan='2'>WCIĘCIE</th>
                                        <th rowspan='2'>MATERIAŁ</th>
                                        <th colspan='6'>OKLEJANIE</th>
                                        <th colspan='4'>FREZOWANIE</th>
                                    </tr>
                                    <tr class='items'>
                                         <td>A</td>
                                         <td>B</td>
                                         <td>C</td>
                                         <td>D</td>
                                         <td>E</td>
                                         <td>F</td>
                                         <td>A</td>
                                         <td>B</td>
                                         <td>C</td>
                                         <td>D</td>
                                         <td>E</td>
                                         <td>F</td>
                                         <td>Typ</td>
                                         <td>Długość</td>
                                         <td>Głębokość</td>
                                         <td>Szerokość</td>
                                    </tr>");

                        foreach (var lFormatter in det.LFormatterDtos)
                        {
                            sb.AppendFormat(@"<tr>
                                                <td>{0}</td>
                                                <td>{1}</td>
                                                <td>{2}</td>
                                                <td>{3}</td>
                                                <td>{4}</td>
                                                <td>{5}</td>
                                                <td>{6}</td>
                                                <td>{7}</td>
                                                <td>({8}) {9} {10}mm</td>
                                                <td>{11}</td>
                                                <td>{12}</td>
                                                <td>{13}</td>
                                                <td>{14}</td>
                                                <td>{15}</td>
                                                <td>{16}</td>
                                                <td>{17}</td>
                                                <td>{18}</td>
                                                <td>{19}</td>
                                                <td>{20}</td>
                                      </tr>", lFormatter.Name, lFormatter.Count, lFormatter.Width1, lFormatter.Width2,
                                lFormatter.Depth1, lFormatter.Depth2, lFormatter.Indentation1, lFormatter.Indentation2,
                                lFormatter.Fabric.ProducerCode, lFormatter.Fabric.Name, lFormatter.Fabric.Thickness,
                                lFormatter.Width1BorderThickness, lFormatter.Width2BorderThickness, lFormatter.Depth1BorderThickness,
                                lFormatter.Depth2BorderThickness, lFormatter.Indentation1BorderThickness,
                                lFormatter.Indentation2BorderThickness,
                                _dictionary.GetMillingType(lFormatter.Milling), lFormatter.CutterLength,
                                lFormatter.CutterDepth, lFormatter.CutterWidth);

                        }
                        sb.Append(@"</table>");
                    }

                    if (det.PentagonFormatterDtos.Any())
                    {
                        sb.Append(@"
                        <h2>Formatki pięciokątne</h2>
                        <table class='pentagonFormatter'>
                                    <tr class='items'>
                                        <th rowspan='2'>ELEMENT</th>
                                        <th rowspan='2'>ILOŚĆ</th>
                                        <th colspan='2'>SZEROKOŚĆ</th>
                                        <th colspan='2'>GŁĘBOKOŚĆ</th>
                                        <th rowspan='2'>GRUBOŚĆ</th>
                                        <th rowspan='2'>MATERIAŁ</th>
                                        <th colspan='4'>OKLEJANIE</th>  
                                    </tr>
                                    <tr class='items'>
                                         <td>A</td>
                                         <td>B</td>
                                         <td>C</td>
                                         <td>D</td>
                                         <td>Góra</td>
                                         <td>Dół</td>
                                         <td>Lewa</td>
                                         <td>Prawa</td>
                                    </tr>");

                        foreach (var pentagonFormatter in det.PentagonFormatterDtos)
                        {
                            sb.AppendFormat(@"<tr>
                                      <td>{0}</td>
                                      <td>{1}</td>
                                      <td>{2}</td>
                                      <td>{3}</td>
                                      <td>{4}</td>
                                      <td>{5}</td>
                                      <td>{6}</td>
                                      <td>({7}) {8} {9}mm</td>
                                      <td>{10}</td>
                                      <td>{11}</td>
                                      <td>{12}</td>
                                      <td>{13}</td>
                                        </tr>", pentagonFormatter.Name, pentagonFormatter.Count,
                                pentagonFormatter.Width1, pentagonFormatter.Width2, pentagonFormatter.Depth1,
                                pentagonFormatter.Depth2, pentagonFormatter.Thickness,
                                pentagonFormatter.Fabric.ProducerCode, pentagonFormatter.Fabric.Name,
                                pentagonFormatter.Fabric.Thickness, pentagonFormatter.TopBorderThickness,
                                pentagonFormatter.BottomBorderThickness, pentagonFormatter.LeftBorderThickness,
                                pentagonFormatter.RightBorderThickness);
                        }
                        sb.Append(@"</table>");
                    }
                }
            }

            sb.Append(@"                            </body>
                        </html>");

            return sb.ToString();
        }

    }
}
