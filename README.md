#### Summary
Log delegates generator tool. Uses LogEvents.json from project to generate ILogger extensions for high performance logging https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage?view=aspnetcore-3.1

The solution is implemented as a dotnet tool, more information at https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools

#### Install steps (for installing from sources, for external use):
1. Clone the repository, open command prompt and navigate to target project folder (where tool should be executed)
2. Run commands:

`dotnet pack`

`dotnet tool install HappyTravel.LoggerMessageGenerator -g --add-source ./nupkg`


#### Install steps (for installing github packages, internal use):
1. Run command prompt and navigate to target project (where log events should be generated)
2. Run command
`dotnet tool install HappyTravel.LoggerMessageGenerator -g`

#### Uninstall steps:
1. Open command prompt
2. Run command

`dotnet tool uninstall HappyTravel.LoggerMessageGenerator -g`

#### How to use:
1. Add `LogEvents.json` file to the project folder, in which `LoggerExtensions.g.cs` should be generated. 
2. Fill file contents using **'LogEvents.json format'** section
3. Open command prompt and navigate to target project folder
4. Run `generate-logmessages` command


#### LogEvents.json format:
The file can contain two event types: exceptions and not exceptionspo. To add an event use the following syntax:

 `{"id": <%EventId%>, "name": "<%EventTitle%>", "level": "<%LogLevel%>", "template": "<%MessageTemplate%>", "argumentTypes": <%ArgumentTypes%>}`

Where:
- **EventId** - is a numerical value, unique Id of event
- **EventTitle** - is a textual name of the event
- **LogLevel** - event logging level: Information, Debug, Error etc. from https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel?view=dotnet-plat-ext-3.1
- **MessageTemplate** - template of the message or the whole message when no arguments passed,
- **ArgumentTypes** - an ordered logging method argument CLR types list (optional)

Depending on the values of event fields, logger generator will create logger messages with logger extensions method. For each parameter that was used in message template, method parameter will be generated. Argument types are got from "argumentTypes", this must contain the same count of values as in template.
For example, for LogEvents.json with:

```json
[
  {"id": 1001, "name": "CustomerRegistrationException", "level": "Critical", "template": "Customer registration exception"},
  {"id": 1002, "name": "CustomerRegistrationSuccess", "level": "Information", "template": "Customer '{CustomerName}' registered with id: '{Id}'", "argumentTypes": ["string", "int"]},
]
```

the following file will be generated:

```csharp
    public static class LoggerExtensions
    {
        static LoggerExtensions()
        {
            CustomerRegistrationException = LoggerMessage.Define(LogLevel.Critical,
                new EventId(1001, "CustomerRegistrationException"),
                "Customer registration exception");
            
            CustomerRegistrationSuccess = LoggerMessage.Define<string, int>(LogLevel.Information,
                new EventId(1002, "CustomerRegistrationSuccess"),
                "Customer '{CustomerName}' registered with id: '{Id}'");
            
        }
    
                
         public static void LogCustomerRegistrationException(this ILogger logger, Exception exception = null)
            => CustomerRegistrationException(logger, exception);
                
         public static void LogCustomerRegistrationSuccess(this ILogger logger, string CustomerName, int Id, Exception exception = null)
            => CustomerRegistrationSuccess(logger, CustomerName, Id, exception);
    
    
        
        private static readonly Action<ILogger, Exception> CustomerRegistrationException;
        
        private static readonly Action<ILogger, string, int, Exception> CustomerRegistrationSuccess;
    }
 ```

 
To use this from code just inject ILogger to class and call `_logger.CustomerRegistrationSuccessOccured("John Doe")
