using System.Xml.Serialization;

namespace BookShop.Helper
{
    public class XmlSerialization
    {
        public T DeserializeFile<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
                return (T)serializer.Deserialize(file);
            }
        }

        public void SerializeFile<T>(T ObjectToSerialize, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream file = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(file, ObjectToSerialize);
            }
        }
    }
}