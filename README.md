# Getting Started - .NET CORE 7 WEB API using CosmosDB as database
This project is Task Manager, as name states, this application exposes API to create task and it is assigned to a user. Task has labels, priority, sub tasks, attachments and more.
This application is built using .NET Core 7 WEB API and cosmos db as database.

Complete video can be watched here[https://youtu.be/zdih5L0I880] https://youtu.be/zdih5L0I880

## Steps to make this application work

* Configure the cosmos db either imstall cosmos db emulator or provision using Azure Portal (watch starting of the video)
 ** Watch this video to provision Cosmos Db in Azure portal. https://youtu.be/kxBafyDy5LQ
 ** Watch this video to see how to setup Cosmos DB Emulator in local https://youtu.be/8NlCk2_Tl10
	*** To install Emulator use this link to download https://aka.ms/cosmosdb-emulator
* Database name should be "PersonalTaskManagerDB", two containers namely "Tasks" and "Users". If you name differently, just adjust the details in the configuration.
* Once you have setup the database, containers as per the video you need to replace the URL, Primary Key in the configuration.
* You are all set! Now, if you run the application will boot. 



## Upcoming Concepts
 * Add Docker Support and run this app as container app.
 * SeriLog and extensive logging using serilog.sink.appinsight
 * Using Docker Registry(Docker Hub) we will deploy this app to Azure App Service Web App
 * Using Azure Container Registry, we will deploy this app to Azure App Service Web App.
 * CI/CD pipeline using Devops to build, push image to Docker Hub / Azure Container Registry and automatically deploy the application to Azure
 * Docker Volume concepts
 * Docker Network concepts
 * Log all the logs to Azure Storage Account (Table Storage). Also, Azure file share is used to log the details so it can be shared with other containers
 * Integrate with Azure AD B2C for Authentication and Authorization and save user profile/manage tasks for users
 * Build Angular 16 App for the Task Manager
 * Kubernates and many more to come using this application

