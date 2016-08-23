LUIS
==============
LUIS is a service for language understanding that provides intent classification and entity extraction.
In order to use the SDK you first need to create and publish an app on www.luis.ai where you will get your appID and appKey and put their values into App.config in the application provided

The solution contains the SDK itself and a sample application that contains 2 sample use cases (one with intent routers and one using the client directly)


The SDK
--------------
The SDK can be used in 2 different ways (both are shown in the sample).
- one way to use it is to use the client directly and call the functions "Predict" and "reply" that are present in the "LuisClient".
- another way is to create handlers for each intent (as shown in the sample) and setup a router using these handlers in order to have the router handle the responses instead of doing so within the client application.

Sample Application
--------------
The sample application allows you to perform the Predict and Reply operations and to view the following parts of the parsed response:
- Query
- Top Intent
- Dialog prompt/status
- Entities

License
=======

All Microsoft Cognitive Services SDKs and samples are licensed with the MIT License. For more details, see
[LICENSE](</LICENSE.md>).


