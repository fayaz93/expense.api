#Welcome to the Serko.Expense.API wiki!

The solution is to parse raw text and return the legitimate contract to client

**Key Features**
* **NextGen Solution:** Developed in Visual Studio 2019 with .Net core 2.1
* **Pipelines** Plug and play the middleware.
* **Extendable:** Future implementations can be added without altering the framework.
* **Contracts:** Added contracts instead of simple text.


**Assumptions**
* **Client/Server:** 
Web Api is hosted in inhouse server
Client is internal i.e., API is intranet
Agreed API Key for authentication

* **Input:** 
Every request should have only one Expense and duplicate xml nodes are not allowed
Every legitimate request should result in valid response
Total is valid decimal value

* **Output:** 
GST value
Decimal rounding to two positions
Client will check execution result -> status and errors for further analysis


**Diagrams**
*Component Interaction 
![](https://https://github.com/fayaz93/expense.api/blob/master/ComponentInteraction.png)

*Postman
Headers
![](https://https://github.com/fayaz93/expense.api/blob/master/AuthHeader.png)

Request
![](https://https://github.com/fayaz93/expense.api/blob/master/Request.png)

Response
![](https://https://github.com/fayaz93/expense.api/blob/master/Response.png)

**Future**
* JWT Authentication
* Key/Token Hashed and Salted
* Logging extension to DB
* Integrating code coverage
