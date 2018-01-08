using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using WebScrapper.Models;
using System.Text.RegularExpressions;

namespace WebScrapper.Helper
{
    public static class HelperClass
    {
        // thi is returning the html of the url
        public static HtmlDocument GetPage(string url)
        {
            Uri uri = new Uri(url);
            WebRequest request = WebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;

            return PrepareHtmlDocumentFromWebResponse(request.GetResponse());
        }

        // this method is returning the html of the web response
        public static HtmlDocument PrepareHtmlDocumentFromWebResponse(WebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            Stream oStream = response.GetResponseStream();
            StreamReader oStreamReader = new StreamReader(oStream, System.Text.Encoding.UTF8);
            String pageHtml = oStreamReader.ReadToEnd();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageHtml);

            return doc;
        }

        // after reading the values inside <td> there are some extra charachters 
        // and just one decimal number in each <td>
        // but I wanted to save the numbers 
        // so I tried to get just teh numbers out of all thecharachters
        public static string ReturnNUmbersOfString(string text)
        {
            var array = Regex.Split(text, @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "");
            if (array.Count() > 0)
                return array.First();
            else
                return string.Empty;
        }

        public static Response PrepareResponse(HtmlNode table)
        {
            Response res = new Models.Response();
            List<string> cells = new List<string>();

            // looping through <tr> of teh table
            foreach (var row in table.SelectNodes("tbody/tr"))
            {
                // each <tr> has couple of <td> which text are there
                foreach (var cell in row.SelectNodes("td"))
                {
                    // tried to remove extra charachaters and $ sign and trim the text
                    cells.Add(cell.InnerText.Replace("\n\r", "").Replace("$","").Trim());
                }
            }

            // just this two cells had string and decimal value so I need just the decimal value
            cells[8] = ReturnNUmbersOfString(cells[8]);
            cells[14] = ReturnNUmbersOfString(cells[14]);

            // saving the data in the response model 
            // but I need to convert them to matching data types
            res.LabelName = cells[1];
            res.PriceDate = Convert.ToDateTime(cells[2]);
            res.MDunit = Convert.ToInt32(cells[3]);
            res.UnitPrice = Convert.ToDouble(cells[4]);
            res.Product = Convert.ToDouble(cells[5]);
            res.TotalOfIngrediant = Convert.ToDouble(cells[7]);
            res.MediCalDespensingFeeOf = Convert.ToDouble(cells[8]);
            res.MediCalDespensingFee = Convert.ToDouble(cells[9]);
            res.EqualsSubtotal = Convert.ToDouble(cells[11]);
            res.CustomaryPrice = Convert.ToDouble(cells[13]);
            res.reduction = Convert.ToDouble(cells[14]);
            res.ProductTotal = Convert.ToDouble(cells[15]);

            return res;
        }
    }
}