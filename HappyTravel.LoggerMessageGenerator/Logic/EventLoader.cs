using System;
using System.IO;
using System.Linq;
using HappyTravel.LoggerMessageGenerator.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HappyTravel.LoggerMessageGenerator.Logic
{
    internal static class EventLoader
    {
        public static LogEventDescriptor[] LoadEvents(string filePath)
        {
            var eventsData = JsonConvert
                .DeserializeObject<LogEventDescriptor[]>(
                    File.ReadAllText(filePath),
                    new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});

            if (eventsData is null || !eventsData.Any())
                throw new Exception("Could not find any event definitions");

            return eventsData;
        }
    }
}