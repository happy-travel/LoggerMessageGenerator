using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HappyTravel.LoggerMessageGenerator.Model;
using Stubble.Core.Builders;
using Stubble.Core.Classes;
using Stubble.Core.Settings;

namespace HappyTravel.LoggerMessageGenerator.Logic
{
    public static class LoggerExtensionsGenerator
    {
        public static string Generate(IEnumerable<LogEventDescriptor> events, string @namespace)
        {
            var template = LoadTemplate();

            return new StubbleBuilder()
              //  .Configure(c=> c.SetDefaultTags(new Tags("<%", "%>")))
                .Build()
                .Render(template, new 
                {
                    Events = events.ToArray(),
                    Namespace = @namespace
                }, new RenderSettings {SkipHtmlEncoding = true});


            static string LoadTemplate()
            {
                var assembly = Assembly.GetExecutingAssembly();
                using var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.LoggerExtensions.Mustache");
                if(stream is null)
                    throw new FileNotFoundException("Template not found");
                
                using var reader = new StreamReader(stream);

                return reader.ReadToEnd();
            }
        }
    }
}