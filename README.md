# WebScraper
web scraper

This is simple web scraper on https://www.dir.ca.gov/dwc/pharmfeesched/pfs.asp

You can see the demo here http://webscrapper20180107012215.azurewebsites.net/

Try this data for getting back the value

    •	NDC number:   55111068405     
    •	Metric decimal number of units:  50 
    •	Usual and customary price:  $13.12 
    •	Date of service:  Current Date
    •	Nursing home:  Leave Blank
    •	No substitutions:  Leave Blank

  ● The web site first is getting the html with WebRequest then retrieving the form which has the class of shaded then rendering the same html on the view.
  
  ● When the user click on price button I’m preparing the post request data, writing them on WebRequest stream and send it back with WebRequest and this time showing the result on the same view.
  
  ● Because I wanted to show the result on the same page and not refreshing the whole page again I call the Post method from controller with jQuery.
  
  ● For specifying the required data for the post request I used fiddler  
  ● For working with the nodes after reading the html string I used ScrapySharp and HtmlAgilityPack.
  
  ● After each request I’m sending user’s requested data in table Request which I decided to have the columns as string because I wanted to log the user’s data entry even if it is wrong data and saved the response if there is any on Response table with the matching data types because if I get the data back they are all in acceptable syntax.
  
● There is report link in the main menu that shows a quick report from data stored in database. 
● There might be some request with no response data if user entered wrong or invalid data

● I used Entity Framework for stroign data in database

