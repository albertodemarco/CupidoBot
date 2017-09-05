# CupidoBot

CupidoBot is a demo bot built to showcase Microsoft Cognitive Service integrated with Bots in a funny way.

It uses **Vision API**,**Face API**, **Custom Vision API** and leverages **Facebook Graph**.

In order to test the Bot you need the following :

**BotId** , **MicrosoftAppId** and **MicrosoftAppPassword** that are provided when you register the bot on the botframework web site.

On the same site you have to setup the facebook channel and this will force you to create a facebook page and conseguently a **Facebook Page Access Token** that will be used by the Facebook Profile Api.

You have to provide, using the Azure portal, a **Vision APi Key** and a **Text Analytics Key** (this is used to do sentiment analysis on the logged conversations.

On **customvision.ai** you have to create your custom vision project and collect **projectId**, **IterationId** and the **prediction api full url** once your model has been trained.

Finally on the azure portal you have to provision an **Application Insight** instance and **SQL Azure** Basic Instance. From the first one collect **Instrumentation Key** and from the second one the **connection string** of the database.

