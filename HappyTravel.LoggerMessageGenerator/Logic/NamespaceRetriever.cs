using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace HappyTravel.LoggerMessageGenerator.Logic
{
    internal static class NamespaceRetriever
    {
        public static string GetFileNamespace(string projectFolderPath, string filePath)
        {
            var rootNamespace = GetProjectNamespace(projectFolderPath);
            var relativePath = Path.GetRelativePath(projectFolderPath, filePath);
            var relativeNamespace = relativePath
                .Replace(Path.DirectorySeparatorChar, '.')
                .Substring(0, relativePath.Length - Constants.LogEventsFileName.Length - 1);

            return $"{rootNamespace}.{relativeNamespace}";
        }
        
        private static string GetProjectNamespace(string projectFolderPath)
        {
            var projectFiles = Directory.GetFiles(projectFolderPath, "*.csproj");
            if (!projectFiles.Any())
                throw new Exception($"Could find project file in {projectFolderPath}");

            if (projectFiles.Length > 1)
                throw new Exception($"Found more than one project file in {projectFolderPath}");

            var projectFileName = projectFiles.Single();
            var projectXml = XDocument.Load(projectFileName);
            var rootNamespace = projectXml.Descendants("RootNamespace").SingleOrDefault();
            if (rootNamespace != null && !rootNamespace.IsEmpty)
                return rootNamespace.Value;

            return Path.GetFileNameWithoutExtension(projectFileName);
        }
    }
}