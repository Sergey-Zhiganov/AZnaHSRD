using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Мотосалон
{
    public static class Files
    {
        public static void Serialize<T>(T type, string path, string file)
        {
            Program.Json(path, file);
            string json = JsonConvert.SerializeObject(type);
            File.WriteAllText($"{path}//{file}.json", json);
        }
        public static T Deserialize<T>(string path, string file)
        {
            Program.Json(path, file);
            string json = File.ReadAllText($"{path}//{file}.json");
            T type = JsonConvert.DeserializeObject<T>(json);
            return type;
        }
    }
}