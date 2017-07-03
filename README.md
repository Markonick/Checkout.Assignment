-- Checkout.com Test --

​Checkout.com would like you to create a web service using 
the .NET framework of your choice that will enable us to 
manage a shopping list for our office.

1 - Service
-----------
The endpoints are defined as follows:

- ​HTTP ​POST request ​for adding a drink to ​the shopping list with quantity, e.g. name of drink (Pepsi) and quantity (1).

- HTTP ​PUT request for updating a drink's quantity.

- HTTP ​DELETE request for removing a drink from the shopping list.

- HTTP ​GET request for retrieving a drink by its name and displaying its quantity so we can see how many have been ordered.

- HTTP ​GET request for retrieving what we have in the shopping list.

Notes
- Your solution can be served from http://localhost.

- ​This service does not have to use a ​​database - ​an in-memory ​solution to hold the shopping list​ works too. Keep it simple.

- Ideally the shopping list should not contain duplicate drink names.

- Please feel free to implement/explain any advanced features to demonstrate your skills and experience such as paginated lists, API authorisation, validation, etc.


2 - Service Client
------------------
We have a Web API client library hosted via GitHub.com at https://github.com/CKOTech/checkout-net-library.

We would like you to modify this library so that it can interact with your shopping list endpoints and act as an API client for those endpoints.


----------------------------------------------------------------------------------------------------------------------------------------------------------------------

Steps to run:
-------------

Open the ApiClient and ShoppingService solutions in two Visual Studio instances.

Run the ShoppingService and keep in running in the background. It is running on http://localhost:5000.

To test the ApiClient, I have created some tests in the ShoppingService folder in Checkout.ApiClient.Tests. Run them.

You can also use Postman to execute http requests while the service is running in the background
eg.

POST http://localhost:5000/shopping/fanta/41

POST http://localhost:5000/shopping/Evian/167

POST http://localhost:5000/shopping/beer/160

POST http://localhost:5000/shopping/Sprite/16

GET http://localhost:5000/shopping/

PUT http://localhost:5000/shopping/fanta/49

DEL http://localhost:5000/shopping/fanta

and even some invalid calls:

PUT http://localhost:5000/shopping/coca cola/49

PUT http://localhost:5000/shopping/Evian/49sdf

etc...

The choice was to use Nancy as a framework to implement a simple Rest API service.

FluentValidation has been used to validate inputs coming from the HTTP request.

I have tried to keep it simple and make the ApiClient side of the code follow as

much as possible the underlying architectural flow and coding style whilst on the Shopping Service

side, unit tests are also been added to test the code during development.

Pagination & the Nancy shopping module tests have not been completed yet.