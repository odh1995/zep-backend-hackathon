# zep-backend-hackathon

This repo contains all the code for the backend side for the Zep Track hackathon for team 58. 

Highly recommended to use Visual Studio 2022 due to the ease of publishing to the Azure cloud service. 

The content team tracks issues for .NET documentation in the [dotnet/docs](https://github.com/dotnet/docs) and [dotnet/dotnet-api-docs](https://github.com/dotnet/dotnet-api-docs) repositories. Issues are turned off on this repository. File issues against existing samples and suggestions for new samples in those repositories. If you're not sure where, choose [dotnet/docs](https://github.com/dotnet/docs/issues). This process keeps the issues associated with the articles that explain the concepts for each sample. 

The project is run and tested directly to Azure cloud services 


## Publishing the backend with API management
Please follow this guide in order to set up the API management resources and link it to the VS2022 IDE [how-to-publish-API](https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-api-management-using-vs?view=aspnetcore-6.0). 

## Creating CosmosDB
Please follow this guide in order to set up database for the API mangement to call and perform CRUD [how-to-set-up-cosmosDB](https://docs.microsoft.com/en-us/azure/cosmos-db/sql/create-cosmosdb-resources-portal)

## How to deploy
For deployment please right click on the application and click publish using the preset Azure resource setting. 