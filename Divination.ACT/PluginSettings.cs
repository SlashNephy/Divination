using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Advanced_Combat_Tracker;
using Divination.Common;
using Timer = System.Timers.Timer;

namespace Divination.ACT
{
    public class PluginSettings : IDisposable
    {
        public PluginSettings()
        {
            SettingsSerializer = new Serializer(this);
        }

        public Serializer SettingsSerializer { get; }

        public void Dispose()
        {
            SettingsSerializer.Dispose();
        }

        public class Serializer : SettingsSerializer, IDisposable
        {
            private readonly Timer timer;

            public Serializer(PluginSettings settings) : base(settings)
            {
                // Saves every 5 minutes.
                timer = new Timer(5 * 60 * 1000);
                timer.Elapsed += (sender, args) => { Save(); };
                timer.Start();
            }

            private static string SettingFileName => $"{DivinationEnvironment.AssemblyName}.xml";
            private static string SettingPath => Path.Combine(DivinationEnvironment.ConfigDirectory, SettingFileName);

            public void Dispose()
            {
                timer.Dispose();

                Save();
            }

            public void Load()
            {
                if (!File.Exists(SettingPath))
                {
                    return;
                }

                CreateBackup();

                try
                {
                    using var stream = new FileStream(SettingPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using var reader = new XmlTextReader(stream);

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "SettingsSerializer")
                        {
                            ImportFromXml(reader);
                        }
                    }
                }
                catch
                {
                }
            }

            private static void CreateBackup()
            {
                if (!Directory.Exists(DivinationEnvironment.ConfigBackupDirectory))
                {
                    Directory.CreateDirectory(DivinationEnvironment.ConfigBackupDirectory);
                }

                var backupFile = Path.Combine(DivinationEnvironment.ConfigBackupDirectory,
                    $"{SettingFileName}.{DateTime.Now:yyyy-MM-dd}.bak");
                File.Copy(SettingPath, backupFile, true);
            }

            public void Save()
            {
                if (!Directory.Exists(DivinationEnvironment.ConfigDirectory))
                {
                    Directory.CreateDirectory(DivinationEnvironment.ConfigDirectory);
                }

                using var stream = new FileStream(SettingPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                using var writer = new XmlTextWriter(stream, Encoding.UTF8)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };

                writer.WriteStartDocument(true);
                writer.WriteStartElement("Config");
                writer.WriteStartElement("SettingsSerializer");
                ExportToXml(writer);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();

                writer.Flush();
            }
        }
    }

    public static class PluginSettingsEx
    {
        public static void Load(this PluginSettings settings)
        {
            settings.SettingsSerializer.Load();
        }

        public static void Save(this PluginSettings settings)
        {
            settings.SettingsSerializer.Save();
        }

        public static void Bind(this PluginSettings settings, string key, Control control)
        {
            settings.SettingsSerializer.AddControlSetting(key, control);
        }

        public static void Unbind(this PluginSettings settings, string key)
        {
            settings.SettingsSerializer.RemoveControlSetting(key);
        }
    }
}
