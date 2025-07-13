using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Services
{
    public class PersistenceService
    { 
        public void Save<T>(T data, string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
            Directory.CreateDirectory(directory);
            }

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);      
        }

        public T Load<T>(string filePath)
        {
            if(!File.Exists(filePath))
            {
                return default(T);
            }

            var json = File.ReadAllText(filePath);

            var data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }
    }
}
