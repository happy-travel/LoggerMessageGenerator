using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HappyTravel.LoggerMessageGenerator.Model
{
    public readonly struct LogEventDescriptor
    {
        [JsonConstructor]
        public LogEventDescriptor(int id, string name, LogLevel level, string source, bool isException = false)
        {
            Id = id;
            Name = name;
            Level = level;
            Source = source;
            IsException = isException;
        }


        public int Id { get; }
        public string Name { get; }
        public LogLevel Level { get; }
        public string Source { get; }
        public bool IsException { get; }
        public string LevelUpperCase => Level.ToString().ToUpperInvariant();
    }
}