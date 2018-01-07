# WebScraper

This app is scraping https://www.dir.ca.gov/dwc/pharmfeesched/pfs.asp web site
and showing the response table

valid data to try 

NDC number: 55111068405
Metric decimal number of units: 50
Usual and customary price: $13.12
Date of service: Current Date
Nursing home: Leave Blank
No substitutions: Leave Blank

its storing data in database in two tables
Request and Response

Request I designed table fields mostly as string because I wanted to store users data entry as it is,
the original web site has to validation so if I'm trying to have the log os request and response 
I have to see what exactly user did that that the response is null.

relation of the tables are one to one which some request may not have response

I Send the request with WebRequest for the first time get the html, then retrieve the data entery table and show on my page
after clicking on Price button im not calling the action of controller from form that is refreshing the whole page and I'm losing the data entry table so I called the action with ajax to refresh just partial <div>
  
I did check the request with https://www.telerik.com/fiddler to specify what exactly I have to send to web to get the correct response
then I did regenerate the same request write bytes on request stream adn send the request

after getting the response againg I retrieved the result table and show that on my from and save the information in database.
