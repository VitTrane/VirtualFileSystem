using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;
using System;

namespace Infrastructure
{
    public class DbTraceListener : TraceListener
    {
        private readonly int BUFFER_SIZE = 2;

        private string _connectionString;

        private List<string> buffer;

        public DbTraceListener(string configSectionName)
        {
            buffer = new List<string>();
            var config = (ConfigurationManager.GetSection(configSectionName) as LoggingConfigSection);
            if (config != null)
            {
                BUFFER_SIZE = config.MessageBufferSize;
                _connectionString = config.ConnectionStrings["localDb"].ConnectionString;
            }
        }

        public override void Write(string message)
        {
            if (buffer.Count >= BUFFER_SIZE)
            {
                WriteInDb();
            }
            else 
            {
                string commandText = string.Format("INSERT INTO Exception VALUES ('{0}', '{1}')", message, DateTime.Now.ToLongTimeString());
                buffer.Add(commandText);
            }
        }

        public override void WriteLine(string message)
        {
            if (buffer.Count >= BUFFER_SIZE)
            {
                WriteInDb();
            }
            else
            {
                string commandText = string.Format("INSERT INTO Exception VALUES ('{0}', '{1}')", message, DateTime.Now.ToLongTimeString());
                buffer.Add(commandText);
            }
        }

        private void WriteInDb() 
        {
            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                foreach (var item in buffer)
                {
                    IDbCommand command = connection.CreateCommand();
                    command.CommandText = item;
                    command.ExecuteNonQuery();
                }                
            }
        }
    }
}
