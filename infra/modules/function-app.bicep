// Azure Function App module
@description('Function App name')
param functionAppName string

@description('App Service Plan name')
param appServicePlanName string

@description('Storage account name')
param storageAccountName string

@description('Environment name')
param environmentName string

@description('Location for all resources')
param location string

@description('Resource token for unique naming')
param resourceToken string

@description('MongoDB connection string')
@secure()
param mongoDbConnectionString string

@description('Messaging service domain')
param messagingServiceDomain string

@description('Messaging service port')
param messagingServicePort string

// Storage Account
resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  tags: {
    'azd-env-name': environmentName
  }
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        file: {
          keyType: 'Account'
          enabled: true
        }
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
    minimumTlsVersion: 'TLS1_2'
  }
}

// App Service Plan (Consumption plan for Functions)
resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  tags: {
    'azd-env-name': environmentName
  }
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {
    reserved: true // Required for Linux
  }
}

// Application Insights for monitoring
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: 'appinsights-${functionAppName}'
  location: location
  tags: {
    'azd-env-name': environmentName
  }
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
  }
}

// User Assigned Managed Identity
resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'id-${functionAppName}'
  location: location
  tags: {
    'azd-env-name': environmentName
  }
}

// Azure Function App
resource functionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: functionAppName
  location: location
  tags: {
    'azd-env-name': environmentName
    'azd-service-name': 'event-type-management-api'
  }
  kind: 'functionapp,linux'
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNET-ISOLATED|8.0'
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(functionAppName)
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'ConnectionStrings__MongoDb'
          value: mongoDbConnectionString
        }
        {
          name: 'MessagingService__Domain'
          value: messagingServiceDomain
        }
        {
          name: 'MessagingService__Port'
          value: messagingServicePort
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
      ]
      cors: {
        allowedOrigins: [
          '*'
        ]
      }
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
    }
    httpsOnly: true
    identity: {
      type: 'UserAssigned'
      userAssignedIdentities: {
        '${managedIdentity.id}': {}
      }
    }
  }
  dependsOn: [
    appServicePlan
    storageAccount
    appInsights
  ]
}

// Outputs
output functionAppName string = functionApp.name
output functionAppHostName string = functionApp.properties.defaultHostName
