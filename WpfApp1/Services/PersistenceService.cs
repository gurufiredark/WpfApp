using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.Services
{
    public class PersistenceService
    { 
        public void Save<T>(T data, string filePath)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);

                // Se o diretório não existir, cria.
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);

            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao salvar o arquivo:\n{ex.Message}\n\nCaminho: {filePath}", "Erro de Persistência");
            }
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
