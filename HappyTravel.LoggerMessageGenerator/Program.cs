using System;
using System.IO;
using System.Linq;
using HappyTravel.LoggerMessageGenerator.Logic;

namespace HappyTravel.LoggerMessageGenerator
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                
                Console.WriteLine($"Searching for '{Constants.LogEventsFileName}' in '{Directory.GetCurrentDirectory()}'");

                var sourceFilePath = GetSourceFilePath();
                Console.WriteLine($"Found at '{sourceFilePath}'");

                var classNamespace = NamespaceRetriever.GetFileNamespace(Directory.GetCurrentDirectory(), sourceFilePath);
                Console.WriteLine($"Generated namespace '{classNamespace}'");

                var eventsData = EventLoader.LoadEvents(sourceFilePath);
                Console.WriteLine($"Found {eventsData.Length} event definitions");

                var result = LoggerExtensionsGenerator.Generate(eventsData, classNamespace);

                var targetFilePath = GetTargetFilePath(sourceFilePath);
                Console.WriteLine($"Saving file as '{targetFilePath}'");

                File.WriteAllText(targetFilePath, result);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ResetColor();
            }


            static string GetSourceFilePath()
            {
                var files = Directory
                    .GetFiles(Directory.GetCurrentDirectory(), Constants.LogEventsFileName, SearchOption.AllDirectories)
                    .Where(f=>
                    {
                        var directory = Path.GetDirectoryName(f) ?? string.Empty;
                        return !directory.Contains("bin") && !directory.Contains("obj");
                    })
                    .ToArray();
                
                if (!files.Any())
                    throw new Exception($"Could find '{Constants.LogEventsFileName}'");

                if (files.Length > 1)
                    throw new Exception($"More than 1 '{Constants.LogEventsFileName}' found");

                return files.Single();
            }


            static string GetTargetFilePath(string eventsDataFilePath)
                => Path.Combine(
                    Path.GetDirectoryName(eventsDataFilePath) ?? throw new DirectoryNotFoundException(eventsDataFilePath),
                    "LoggerExtensions.g.cs");
        }
    }
}