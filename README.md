# CQRSDemo

A short demo showing distributed handling of http request. This would be a nice start when you plan to implement CQRS into your project.

Prerequisites :
VS.NET allowing web development and containing the Azure Functions sdk
NodeJS LTS (or >= 10)

## To get this project running

Go to the Azure Portal and create a Storage Account and a Service Bus. Note the Service Bus must at least be a Standard to support topics.
Then create a new Topic with the name 'bank' in the Service Bus. On the Topic blade, click 'shared access policies' and add an access policy for Manage, Send and Listen.
Then copy & paste the connection string to the topic into the ASP.NET project's appsettings.json into the EventBus.EventBusConnection setting.
Now back to the Azure portal, on the topic blade (click on the topic you just created), and go to Subscriptions. Create a new subscription and name
it whatever you want. Also enter this name in the appsettings.json EventBus.SubscriptionClientName setting. You can leave the RetryCount setting to 5.
Now this project also requires a connection sting to the root of the Service Bus in order to send stuff the a Service Bus queue. Go back the the service
bus and copy and paste a connection string of the RootManageSharedAccessKey access policy (for this demo purpose you'll be fine).

## The functions project

Create a local.settings.json file in the root of the Azure Functions project and add the following json:

```json
  "Values": {
    "AzureWebJobsStorage": "/-- connection string to your storage account --/",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "AzureServiceBus": "/-- connection sting to the root of your service bus --/"
  }
```

## Running the project

Now start both the website project and the azure functions project. Determine the port on which website project runs and copy & past this
URL to the client project in two files:

```csharp
CqrsClient/src/app/services/transaction.service.ts
CqrsClient/src/app/pages/home/landing.component.ts
```

Now open a console app and navigate to the root of the client project (/CqrsClient).
enter the following commands:

```
npm i @angular/cli -g
npm i
ng serve -o
```

This installs the Angular CLI globally on your computer. Then installs all packages required by this demo, and starts a development server opening a new browser.

You should now see a form allowing you to add a bank transaction. Have fun!
