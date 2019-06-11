#Welcome to the Serko.Expense.API wiki!

The solution is to parse raw text and return the legitimate contract to client

**Key Features**
* **NextGen Solution:** Developed in Visual Studio 2017 with .Net core 2.1
* **Pipelines:** Plug and play the middleware.
* **Extendable:** Future implementations can be added without altering the framework.
* **Contracts:** Added contracts instead of simple text.

**How to run the application**
* Open the solution in Visual studio
* Using Client <br/>
Run F5 or Ctrl F5 <br/>
See the URL and Port <br/>
Open the client, preferably Postman and adjust the URL as observed above <br/>
Enter the input <br/>
* Using Test Project <br/>
Change the input  <br/>
Debug and see the response <br/>


**Assumptions**
* **Client/Server:** <br/>
Web Api is hosted in inhouse server <br/>
Client is internal i.e., API is intranet <br/>
Agreed API Key for authentication <br/>

* **Input:** <br/>
Every request should have only one Expense and duplicate xml nodes are not allowed <br/>
Every legitimate request should result in valid response <br/>
Total is valid decimal value <br/>

* **Output:** <br/>
GST value <br/>
Decimal rounding to two positions <br/>
Client will check execution result -> status and errors for further analysis <br/>


**Diagrams** <br/>
*Component Interaction  <br/>
![](https://github.com/fayaz93/expense.api/blob/master/ComponentInteraction.png)

*Postman <br/> <br/>
Headers <br/>
![](https://github.com/fayaz93/expense.api/blob/master/AuthHeader.png)

Request <br/>
![](https://github.com/fayaz93/expense.api/blob/master/Request.png)

Response <br/>
![](https://github.com/fayaz93/expense.api/blob/master/Response.png)

**Future**
* JWT Authentication
* Key/Token Hashed and Salted
* Logging extension to DB
* Integrating code coverage
