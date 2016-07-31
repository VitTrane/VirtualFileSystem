using System.Configuration;

namespace Infrastructure
{
    public class LoggingConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("BufferSize")]
        public int MessageBufferSize
        {
            get
            {
                var str = (base["BufferSize"]).ToString();
                return int.Parse(str);
            }
        }

        [ConfigurationProperty("connectionStrings")]
        [ConfigurationCollection(typeof(ConnectionStringSettings))]
        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                return (ConnectionStringSettingsCollection)base["connectionStrings"];
            }
        }
    }
}
