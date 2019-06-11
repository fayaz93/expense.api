#Welcome to the Serko.Expense.API wiki!

The solution is to parse raw text and return the legitimate contract to client

**Key Features**
* **NextGen Solution:** Developed in Visual Studio 2017 with .Net core 2.1
* **Pipelines:** Plug and play the middleware.
* **Extendable:** Future implementations can be added without altering the framework.
* **Contracts:** Added contracts instead of simple text.


**Assumptions**
* **Client/Server:** 
Web Api is hosted in inhouse server <br/>
Client is internal i.e., API is intranet <br/>
Agreed API Key for authentication <br/>

* **Input:** 
Every request should have only one Expense and duplicate xml nodes are not allowed <br/>
Every legitimate request should result in valid response <br/>
Total is valid decimal value <br/>

* **Output:** 
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
