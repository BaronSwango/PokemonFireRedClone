using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows;

namespace PokemonFireRedClone
{
    public class XmlManager<T>
    {
        public Type Type;

        // using a text reader in order to read from xml file and convert into object
        public T Load(string path)
        {
            T instance;
            using (TextReader reader = new StreamReader(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\" + path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                instance = (T)xml.Deserialize(reader);

            }
            return instance;
        }

        // using a text writer in order to write and save and object to an xml file 
        public void Save(string path, object obj)
        {
            using (TextWriter writer = new StreamWriter(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\" + path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                xml.Serialize(writer, obj);
            }
        }
    }
}
