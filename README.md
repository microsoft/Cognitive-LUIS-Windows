LUIS
==============
LUIS is a service for language understanding that provides intent classification and entity extraction.
In order to use the SDK you first need to create and publish an app on www.luis.ai where you will get your appID and appKey.

The solution contains the SDK itself and a sample application that allow you to enter you appId and appKey, and to perform the two actions "predict" and "reply".

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
The sample application allows you to perform the Predict and Reply operations and to view the following parts of the parsed response:
- Query
- Top Intent
- Dialog prompt/status
- Entities

License
=======

All Microsoft Cognitive Services SDKs and samples are licensed with the MIT License. For more details, see
[LICENSE](</LICENSE.md>).

Developer Code of Conduct
=======

Developers using Cognitive Services, including this client library & sample, are required to follow the “[Developer Code of Conduct for Microsoft Cognitive Services](http://go.microsoft.com/fwlink/?LinkId=698895)”.

