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
        // this is the database context variable
        private DatabaseContext db = new DatabaseContext();

        string url = "https://www.dir.ca.gov/dwc/pharmfeesched/pfs.asp";
       
        // this is get method which is sending request to the url and 
        // selecting the form from returned html and showing the table inside
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index() 
        {
            // sending url to the method and grting back the html document
            HtmlDocument doc = HelperClass.GetPage(url);

            // I wanted to havethe same itle with the original web site
            ViewBag.Title = doc.DocumentNode.CssSelect("h1").First().InnerText;

            // selecting the form with the css class
            HtmlNode node = doc.DocumentNode.CssSelect(".shaded").First();

            // there is a clear form link in the page which is refreshing the same page but 
            // after rendring the html in my view it has to refresh my view so I replaced the href with 
            // empty string
            string html = node.InnerHtml.Replace("pfs.asp", "");

            // I set an id for the button so later I can find the button with jQuery on click event
            html = html.Replace("type=\"submit\"", "type =\"submit\" id=\"button\"");

            //store the html text on Viewbag so I can have it in view
            ViewBag.EncodedHtml = MvcHtmlString.Create(html);

             return View();
        }       

        // this is the post method which is sending the users's data to original web site
        // and geting back the result
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CalculatePrice(string NDCno, string MDUnits, 
                                                string PriceBilled, string DateOfService,
                                                string NursingHome, string Brand)
        {

            // first I have to create request model the request
            Request req = new Request();
            req.NDCNo = NDCno;
            req.MDunits = MDUnits;
            req.price = PriceBilled;
            req.ServiceDate = DateOfService;
            req.NursingHome = NursingHome.ToUpper() == "TRUE" ? true : false;
            req.Brand = Brand.ToUpper() == "TRUE" ? true : false;              

            // here we need to prepared data to send with the post request
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

            // here i'm setting the request header information
            byte[] dataStream = Encoding.UTF8.GetBytes(data);
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = dataStream.Length;  

            // i have to writethe data on request stream to send
            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(dataStream,0,dataStream.Length);
            newStream.Close();
            WebResponse webResponse = webRequest.GetResponse();

            // this method is preparing th html doc from the response from web site
            HtmlDocument responseDoc = HelperClass.PrepareHtmlDocumentFromWebResponse(webResponse);

            // if the user's data is valid so there will be a new table on page with the class of
            // tabborder
            string html = "";
            Response res = new Models.Response();

            // here can not check the Response status code because it is always OK
            if (responseDoc.DocumentNode.CssSelect(".tabborder").Any())
            {                
                HtmlNode node = responseDoc.DocumentNode.CssSelect(".tabborder").First();

                // this method is reading the <td> nodes os the table and fetching data out of it
                // and return the response model with the data 
                res = HelperClass.PrepareResponse(node);

                // here I have to set the response for the request
                req.Response = res;
                //res.Request = req;

                html = node.OuterHtml;
            }
            else
            {
                html = "<p id=\"notfound\"> The requested information not found <p>";
            }

            // saving the request and response data in database
            db.Requests.Add(req);
            db.SaveChanges();

            return Content(html, "text/html");
        }

        // this is for sending back all data for report
        // of course this is not the good choice to send all the data but it is just a quick report
        // for demo
        public ActionResult Report()
        {
            
            return View(db.Requests.ToList());
        }

    }
}