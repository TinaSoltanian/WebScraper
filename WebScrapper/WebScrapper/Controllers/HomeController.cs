using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebScrapper.Data;
using WebScrapper.Helper;
using WebScrapper.Models;

namespace WebScrapper.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        string url = "https://www.dir.ca.gov/dwc/pharmfeesched/pfs.asp";
       
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index() 
        {
            HtmlDocument doc = HelperClass.GetPage(url);

            ViewBag.Title = doc.DocumentNode.CssSelect("h1").First().InnerText;

            HtmlNode node = doc.DocumentNode.CssSelect(".shaded").First();
            string html = node.InnerHtml.Replace("pfs.asp", "");
            html = html.Replace("type=\"submit\"", "type =\"submit\" id=\"button\"");

            ViewBag.EncodedHtml = MvcHtmlString.Create(html);

             return View();
        }       

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CalculatePrice(string NDCno, string MDUnits, 
                                                string PriceBilled, string DateOfService,
                                                string NursingHome, string Brand)
        {

           //first I have to register the request
            Request req = new Request();
            req.NDCNo = NDCno;
            req.MDunits = MDUnits;
            req.price = PriceBilled;
            req.ServiceDate = DateOfService;
            req.NursingHome = NursingHome.ToUpper() == "TRUE" ? true : false;
            req.Brand = Brand.ToUpper() == "TRUE" ? true : false;              

            string data = string.Format("NDCno={0}&MDUnits={1}&PriceBilled={2}&DateOfService={3}",
                NDCno, MDUnits, PriceBilled, DateOfService);

            if (NursingHome.ToUpper() == "TRUE")
            {
                data += "&NursingHome=on";
            }
            if (Brand.ToUpper() == "TRUE")
            {
                data += "&Brand=on";
            }

            byte[] dataStream = Encoding.UTF8.GetBytes(data);
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = dataStream.Length;  
            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(dataStream,0,dataStream.Length);
            newStream.Close();
            WebResponse webResponse = webRequest.GetResponse();

            HtmlDocument responseDoc = HelperClass.PrepareHtmlDocumentFromWebResponse(webResponse);

            string html = "";
            Response res = new Models.Response();
            if (responseDoc.DocumentNode.CssSelect(".tabborder").Any())
            {
                HtmlNode node = responseDoc.DocumentNode.CssSelect(".tabborder").First();
                res = HelperClass.PrepareResponse(node);

                req.Response = res;
                //res.Request = req;

                html = node.OuterHtml;
            }

            
            db.Requests.Add(req);
            db.SaveChanges();

            return Content(html, "text/html");
        }

        public ActionResult Report()
        {
            
            return View(db.Requests.ToList());
        }

        public ActionResult ReportDetail(Response model)
        {

            return PartialView("ReportDetail", model);
        }
    }
}