using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ImageMagick;

namespace TextureMerge
{
    public class ProjectConfig
    {
        public string SavePath { get; set; }
        public string SaveName { get; set; }
        public string DefaultColor { get; set; }
        public List<ChannelSettings> Channels { get; set; }

        public class ChannelSettings
        {
            public int Slot { get; set; }
            public string FilePath { get; set; }
            public int SourceChannel { get; set; }
        }

        public static void Save(string path, ProjectConfig config)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(path, json);
        }

        public static ProjectConfig Load(string path)
        {
            if (!File.Exists(path)) return null;
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<ProjectConfig>(json);
        }
    }
}
