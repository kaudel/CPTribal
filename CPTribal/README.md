# CPTribal 
(Credit Processing Tribal )
Is an application for calculate a credit line requested by a client.This application, exposes a API Post method called EvaluateCreditRequest for verify if a credit line is approved.

## Parameters
The application needs some parameters:
- FoundigType: it can be 
    - StartUp
    - SME
- Cash balance: The costumer's bank account balance
- Monthly revenue: The total sales revenue for the month
- Requested credit line
- Requested date
 
Example:
```sh
{
  "id": "1",
  "foundingType": "SME",
  "cashBalance": 1252,
  "monthlyRevenue": 4125,
  "requestedCreditLine": 20,
  "requestedDate": "2022-02-27T13:27:40.357Z"
}
```
**************************
The app uses an InMemoryDatabase called "CreditEvaluation" this database is generated every time when the app is executed, and will store the data just during the life of the app.

Also a nuget is included: ASPNetCoreRateLimit.
This nuget helps to rate the limit of the calls to the API in a period of time. The configuration is available in the appsettings.json  in the node: IpRateLimiting . It is configured limit the calls to 3  request in 2 minutes.
Also swagger is enabled.
A project with unit test called CPTribalTest is included.

It contains some folders inside of the project:
- **BussinesRules:** Contains the logic for evaluate everyone of the FoundigTypes and logic for verify and return the correct messages according with the result.
- **Controllers:** Contains the controllers that exposes the functionality
- **Data:** Contains the classes and logic for access to the database
- **DataModels:** Contains the classes used by the database for process the requests processed
- **Identity:** Contains the entity classes that are used for process the request and the response

When the controller is called, the parameters provided are verified and the type of founding provided is evaluated, depending of the type will be the object class that will be executed

The app implements a base clase called BaseCreditLine, it is an abstract class, contains concrete implementations for calculate:
- Montly Revenue
- Cash Balance

Also an abstract method called CalculateCreditLine. The class that implements the BaseCreditLine class needs to make an specific implementation of this menthod. It makes more easy the way to integrate new credit types. If a new credit is requested, then just a class with the specific behavior needs to be added and the functionality for process and validate the result of evalute the credit, not will be modified. It makes more easy to extend the class.

Everytime when a request is procesed and rejected, a counter is incremented, if the same request is rejected 3 times then the request not will be processed anymore.

Most of the logic of the app is contained inside of the file CreditValidation
In every request:
- Verify if the request was previously processed using the key field Id provided in the request
- If is a new request then evalute the credit, 
  -  if is accepted then returns a message "Credit application accepted",  httpCode 201 and the value of the creditLine accepted.
   - if not is accepted the returns a message "Credit application not accepted", httpCode 200 and 0
- if not is a new request, and the previous request was accepted, then return the that request.
   - if not was accepted, verify the failIntents, if failintents is bigger or equal to 3 returns a message "A sales agent will contact you".
    - if failIntents is lower than 3, the request evaluated again, and processed as rejected or accepted.


## Run App
Just run the app from VS, a swagger window will be displayed

## Test App
in order to test the App you can send transactins from PostMan or a related tool
