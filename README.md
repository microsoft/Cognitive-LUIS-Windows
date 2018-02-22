# This SDK is being deprecated and is no longer maintained. 
# Please go [here](https://github.com/Azure/azure-sdk-for-net/tree/psSdkJson6/src/SDKs/CognitiveServices/dataPlane/Language/LUIS) for the latest .NET SDK.
 
 LUIS
==============
LUIS is a service for language understanding that provides intent classification and entity extraction.
In order to use the SDK you first need to create and publish an app on www.luis.ai where you will get your Application Id and Subscription Key.

The solution contains the SDK and a sample application that allows you to enter your appId and appKey, and to perform the two actions "predict" and "reply".

Cloning The Repo
--------------
Please note that the repo depends on a submodule, so in order to clone it you'll have to attach --recursive option to the clone command:
`git clone --recursive  https://github.com/Microsoft/Cognitive-LUIS-Windows.git`

The SDK
--------------
The SDK can be used in 2 different ways (both are shown in the sample).
- one way to use it is to use the client directly and call the functions "Predict" and "reply" that are present in the "LuisClient".
- another way is to create handlers for each intent (as shown in the sample) and setup a router using these handlers in order to have the router handle the responses instead of doing so within the client application.

Sample Application
--------------
The sample application allows you to try three different modes.
- Mode 1: Perform the Predict and Reply actions using LuisClient directly operations and to view the following parts of the parsed response: Query, Top Intent, Dialog prompt/status, Entities
- Mode 2: Perform the Predict action function using the IntentRouter class and an IntentHandlers class that contains normal functions
- Mode 3: Perform the Predict action function using the IntentRouter class and an IntentHandlers class that contain static functions

Intent Router and Handlers mode
--------------
In order to use modes 2 and 3 of the sample application you have to import this [LUIS application JSON file](</Sample/LUIS Sample Application JSON/SDK Test.json>) using your LUIS account, train and publish it, and use its Application Id in those two modes.
You can try the following utterances:
- "Book me a flight" get routed to "Book a flight" intent handler
- "Book me a cab" gets routed to "Book a cab" intent handler
- "Order food" gets routed to "None" intent handler
- "Book me a train" doesn't get routed to any intent handler due to low confidence score

License
=======

All Microsoft Cognitive Services SDKs and samples are licensed with the MIT License. For more details, see
[LICENSE](</LICENSE.md>).

Developer Code of Conduct
=======

Developers using Cognitive Services, including this client library & sample, are required to follow the “[Developer Code of Conduct for Microsoft Cognitive Services](http://go.microsoft.com/fwlink/?LinkId=698895)”.

