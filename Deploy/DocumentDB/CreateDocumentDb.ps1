#Creates DocumentDB account 

# Set the Azure resource group name and location
$resourceGroupName = "A2J-develop-rg"
$resourceGroupLocation = "Central US"

# Get reference to the resource group
Get-AzureRmResourceGroup -Name $resourceGroupName 

# Database name
$DBName = "a2jdb"

# Write and read locations and priorities for the database
$locations = @(@{"locationName"="Central US"; 
                 "failoverPriority"=0}, 
               @{"locationName"="North Central US"; 
                  "failoverPriority"=1})

# IP addresses that can access the database through the firewall
$iprangefilter = "10.0.0.1"

# Consistency policy
$consistencyPolicy = @{"defaultConsistencyLevel"="BoundedStaleness";
                       "maxIntervalInSeconds"="10"; 
                       "maxStalenessPrefix"="200"}

# DB properties
$DBProperties = @{"databaseAccountOfferType"="Standard"; 
                          "locations"=$locations; 
                          "consistencyPolicy"=$consistencyPolicy; 
                          "ipRangeFilter"=$iprangefilter}

# Create the database
New-AzureRmResource -ResourceType "Microsoft.DocumentDb/databaseAccounts" `
                    -ApiVersion "2015-04-08" `
                    -ResourceGroupName $resourceGroupName `
                    -Location $resourceGroupLocation `
                    -Name $DBName `
                    -PropertyObject $DBProperties