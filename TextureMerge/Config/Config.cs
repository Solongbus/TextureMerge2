using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace TextureMerge
{
    [Serializable]
    public class Config
    {
        public static Config Current { get; private set; } = new Config();

        // Config related
        public string Redirect { get; set; } = "config.xml";
        public bool UseLastWindowSize { get; set; } = true;
        public bool UseLastPathToSave { get; set; } = true;
        public bool UseLastSaveImageName { get; set; } = true;
        public bool CheckForUpdates { get; set; } = true;
        public string SkipVersion { get; set; } = "";
        public bool EnableSendTo { get; set; } = false;

        // State related
        public int WindowWidth { get; set; } = -1;
        public int WindowHeight { get; set; } = -1;
        public string PathToSave { get; set; } = @"%UserProfile%\Documents";
        public string SaveImageName { get; set; } = "Pack.png";
        public uint DefaultColorInt { get; set; } = 0;

        public static void ApplyConfig(Config config) => Current = config;

        public Config Copy()
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ms, this);
                    ms.Position = 0;
                    return (Config)formatter.Deserialize(ms);
                }
            }
            catch (Exception) { return null; }
        }

        private static string GetConfigPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            string expanded = path.Expand();
            if (Path.IsPathRooted(expanded)) return expanded;
            
            // 如果是相对路径，则相对于可执行文件所在目录
            string exeDir = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            return Path.Combine(exeDir, expanded);
        }

        public static void Load()
        {
            string lastRedirect = Current.Redirect;
            int redirected = 0;
            try
            {
                do
                {
                    string fullPath = GetConfigPath(lastRedirect);
                    if (!File.Exists(fullPath))
                        return;

                    var serializer = new XmlSerializer(typeof(Config));

                    using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                    {
                        var config = (Config)serializer.Deserialize(stream);

                        if (config.Redirect == lastRedirect)
                        {
                            Current = config;
                            return;
                        }

                        else
                        {
                            lastRedirect = config.Redirect;
                            redirected++;
                        }
                    }
                } while (redirected < 2);
            }
            catch (Exception e)
            {
                MessageDialog.Show("Unable to load configuration" + Environment.NewLine + e.Message,
                    "Error", MessageDialog.Type.Error);
            }
        }

        public static void Save()
        {
            if (App.IsCliMode) return; // CLI 模式下不自动保存配置，避免生成多余的 config.xml

            string fullPath = GetConfigPath(Current.Redirect);
            if (string.IsNullOrEmpty(fullPath)) return;

            string dir = Path.GetDirectoryName(fullPath);
            if (string.IsNullOrEmpty(dir) || Directory.Exists(dir))
            {
                try
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        var serializer = new XmlSerializer(typeof(Config));
                        serializer.Serialize(stream, Current);
                    }
                }
                catch (Exception e)
                {
                    MessageDialog.Show("Unable to save configuration" + Environment.NewLine + e.Message,
                        "Error", MessageDialog.Type.Error);
                }
            }
        }
    }
}
