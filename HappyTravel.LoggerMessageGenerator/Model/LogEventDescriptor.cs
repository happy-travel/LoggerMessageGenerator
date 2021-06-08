using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HappyTravel.LoggerMessageGenerator.Model
{
    public readonly struct LogEventDescriptor
    {
        [JsonConstructor]
        public LogEventDescriptor(int id, string name, string template, LogLevel level, string source, string[]? argumentTypes, bool isException = false)
        {
            Id = id;
            Name = name;
            Template = template;
            Level = level;
            Source = source;
            ArgumentTypes = argumentTypes;
            IsException = isException;
        }


        public int Id { get; }
        public string Name { get; }
        public string Template { get; }
        public LogLevel Level { get; }
        public string Source { get; }
        public bool IsException { get; }
        
        public string[]? ArgumentTypes { get; }

        public string ArgumentTypesString
        {
            get
            {
                if (ArgumentTypes is null || !ArgumentTypes.Any())
                    return string.Empty;
                
                return string.Join(", ", ArgumentTypes);
            }
        }

        public string ArgumentsString
        {
            get
            {
                if (ArgumentTypes is null)
                    return string.Empty;

                var argumentNames = GetTokens(Template);
                if (argumentNames.Length != ArgumentTypes.Length)
                    throw new Exception("Argument types mismatch");

                var argumentsString = string.Empty;
                for (var i = 0; i < argumentNames.Length; i++)
                {
                    var type = ArgumentTypes[i];
                    var name = argumentNames[i];
                    argumentsString += $", {type} {name}";
                }

                return argumentsString;
            }
        }

 

        public string ArgumentNamesString
        {
            get
            {
                var argumentNames = GetTokens(Template);
                if (!argumentNames.Any())
                    return string.Empty;

                var argumentNamesString = string.Empty;
                foreach (var name in argumentNames)
                    argumentNamesString += $", {name}";

                return argumentNamesString;
            }
        }
        
        private static string[] GetTokens(string str)
        {
            var regex = new Regex(@"(?<=\{)[^}]*(?=\})", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(str);

            // Results include braces (undesirable)
            return matches.Cast<Match>().Select(m => m.Value).Distinct().ToArray();
        }
        

        public string LevelUpperCase => Level.ToString().ToUpperInvariant();
    }

    public readonly struct ArgumentInfo
    {
        public string Name { get; }
        public string Type { get; }

        [JsonConstructor]
        public ArgumentInfo(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}