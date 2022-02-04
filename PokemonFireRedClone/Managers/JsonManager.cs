using System;
using System.IO;
using Newtonsoft.Json;


namespace PokemonFireRedClone
{
    public class JsonManager<T>
    {

        public System.Type Type;

        public JsonManager()
        {
            Type = typeof(T);
        }

        public T Load(string path)
        {
            string jsonFromFile;

            using (var reader = new StreamReader(path))
                jsonFromFile = reader.ReadToEnd();

            return JsonConvert.DeserializeObject<T>(jsonFromFile);

        }

        public void Save(T obj, string path)
        {
            if (!File.Exists(path))
                using (var file = File.Create(path)) { }

            var jsonToWrite = JsonConvert.SerializeObject(obj, Formatting.Indented);

            using (var writer = new StreamWriter(path))
                writer.Write(jsonToWrite);

        }


    }
}
