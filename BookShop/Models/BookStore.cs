using System.Xml.Serialization;

namespace BookShop.Models
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot("bookstore", Namespace = "")]
    public class BookStore
    {
        [XmlElement("book")]
        public List<Book>? book { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public class Book
    {
        public ulong isbn { get; set; }

        public BookTitle? title { get; set; }

        [XmlElement("author")]
        public List<string>? author { get; set; }

        public ushort year { get; set; }

        public decimal price { get; set; }

        [XmlAttribute]
        public string? category { get; set; }

        [XmlAttribute]
        public string? cover { get; set; }
    }
    
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class BookTitle
    {
        [XmlAttribute]
        public string? lang { get; set; }

        [XmlText]
        public string? Value { get; set; }
    }
}