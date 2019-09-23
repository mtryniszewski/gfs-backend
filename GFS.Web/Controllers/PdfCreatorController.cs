using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using GFS.Domain.Core;
using GFS.Transfer.Order.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GFS.Web.Controllers
{
    [Route("api/[controller]")]
    public class PdfCreatorController : Controller
    {
        private readonly IConverter _converter;
        private readonly IOrderPdfService _orderPdfService;
        private readonly IOrderService _orderService;

        public PdfCreatorController(IConverter converter, IOrderPdfService orderPdfService, IOrderService orderService)
        {
            _converter = converter;
            _orderPdfService = orderPdfService;
            _orderService = orderService;
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> CreatePDF(int id)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Zamówienie z TesarStudio",
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = await _orderPdfService.GetHtmlString(new GetOrderQuery
                {
                    Id = id
                }),
                Page = "https://localhost:5000/api/PdfCreator",
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "style.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Strona [page] z [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Wygenerowano za pomocą systemu Tesar Studio" }
            };


            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);
            return File(file, "application/pdf", "Zamowienie.pdf");

        }
    }

}
