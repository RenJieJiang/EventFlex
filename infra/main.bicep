// Main Bicep template for EventFlex infrastructure
@description('Environment name (dev, test, prod)')
param environmentName string = 'dev'

@description('Location for all resources')
param location string = resourceGroup().location

@description('Function App name')
param functionAppName string = 'eventflex-event-types-api-${environmentName}'

@description('Storage account name')
param storageAccountName string = 'evfnstor${uniqueString(resourceGroup().id)}'

@description('App Service Plan name')
param appServicePlanName string = 'eventflex-plan-${environmentName}'

@description('MongoDB connection string')
@secure()
param mongoDbConnectionString string

@description('Messaging service domain')
param messagingServiceDomain string = 'messaging-service'

@description('Messaging service port')
param messagingServicePort string = '3002'

// Create a unique resource token based on resource group and environment
var resourceToken = uniqueString(subscription().subscriptionId, environmentName)

// Reference the modules
module functionApp 'modules/function-app.bicep' = {
  name: 'functionAppDeploy'
  params: {
    functionAppName: functionAppName
    appServicePlanName: appServicePlanName
    storageAccountName: storageAccountName
    location: location
    environmentName: environmentName
    resourceToken: resourceToken
    mongoDbConnectionString: mongoDbConnectionString
    messagingServiceDomain: messagingServiceDomain
    messagingServicePort: messagingServicePort
  }
}

// Define outputs to use in our Azure DevOps pipeline
output functionAppName string = functionApp.outputs.functionAppName
output functionAppHostName string = functionApp.outputs.functionAppHostName
