using BookShop.Helper;
using BookShop.Models;
using System.Xml;

namespace BookShop.BookManager
{
    public class BookFileHandler
    {
        private readonly IConfiguration configuration;
        private readonly XmlSerialization xml;
        private readonly string? storePath;

        public BookFileHandler(IConfiguration configuration, XmlSerialization xml)
        {
            this.configuration = configuration;
            this.xml = xml;
            storePath = configuration["FilePaths:Development"];
        }

        public BookStore GetBooks()
        {
            BookStore books = ConvertXmlToObject();

            return books;
        }

        private BookStore ConvertXmlToObject()
        {
            return xml.DeserializeFile<BookStore>(storePath);
        }

        public Book GetBookDataByBookId(ulong bookId)
        {
            BookStore books = ConvertXmlToObject();
            Book specificBook = new();

            if (books != null)
            {
                specificBook = books.book.FirstOrDefault(book => book.isbn == bookId);
            }

            return specificBook;

        }


        public OperationResult AddNewBook(Book book)
        {
            OperationResult result = new();

            try
            {
                XmlDocument xmlDoc = new();
                xmlDoc.Load(storePath);

                XmlElement newBook = xmlDoc.CreateElement("book");

                newBook.SetAttribute("category", book.category);
                newBook.SetAttribute("cover", book.cover);

                XmlElement bookId = xmlDoc.CreateElement("isbn");
                bookId.InnerText = Convert.ToString(book?.isbn);
                newBook.AppendChild(bookId);

                XmlElement title = xmlDoc.CreateElement("title");
                if (book?.title != null)
                {
                    title.SetAttribute("lang", book.title.lang);
                    title.InnerText = book?.title?.Value;
                }
                newBook.AppendChild(title);


                for (int i = 0; i < book?.author?.Count; i++)
                {
                    XmlElement author = xmlDoc.CreateElement("author");
                    author.InnerText += book.author[i];
                    newBook.AppendChild(author);
                }

                XmlElement year = xmlDoc.CreateElement("year");
                year.InnerText = Convert.ToString(book?.year);
                newBook.AppendChild(year);

                XmlElement price = xmlDoc.CreateElement("price");
                price.InnerText = Convert.ToString(book?.price);
                newBook.AppendChild(price);

                XmlNode? root = xmlDoc.DocumentElement;
                root?.AppendChild(newBook);

                xmlDoc.Save(storePath);
                result.IsSuccessful = true;
                result.Message = "New book added successfully";
            }
            catch (Exception ex)
            {
                result.Message = $"Failed to add new book: {ex.Message}";
            }

            return result;
        }

        public OperationResult EditBook(ulong bookId, Book book)
        {
            OperationResult result = new();

            try
            {
                XmlDocument xmlDoc = new();
                xmlDoc.Load(storePath);

                XmlNode? bookNode = xmlDoc.SelectSingleNode($"//book[isbn='{bookId}']");

                if (bookNode != null)
                {
                    XmlElement bookElement = (XmlElement)bookNode;
                    bookElement.SetAttribute("category", book.category);
                    bookElement.SetAttribute("cover", book.cover);

                    XmlNode? titleNode = bookElement.SelectSingleNode("title");
                    if (titleNode != null)
                    {
                        XmlElement titleElement = (XmlElement)titleNode;
                        titleElement.SetAttribute("lang", book.title.lang);
                        titleElement.InnerText = book.title.Value;
                    }
                    else
                    {
                        XmlElement titleElement = xmlDoc.CreateElement("title");
                        titleElement.SetAttribute("lang", book.title.lang);
                        titleElement.InnerText = book.title.Value;
                        bookElement.AppendChild(titleElement);
                    }

                    XmlNodeList authorNodes = bookElement.SelectNodes("author");
                    foreach (XmlNode authorNode in authorNodes)
                    {
                        bookElement.RemoveChild(authorNode);
                    }

                    foreach (string author in book.author)
                    {
                        XmlElement authorElement = xmlDoc.CreateElement("author");
                        authorElement.InnerText = author;
                        bookElement.AppendChild(authorElement);
                    }

                    XmlNode? yearNode = bookElement.SelectSingleNode("year");
                    if (yearNode != null)
                    {
                        yearNode.InnerText = book.year.ToString();
                    }
                    else
                    {
                        XmlElement yearElement = xmlDoc.CreateElement("year");
                        yearElement.InnerText = book.year.ToString();
                        bookElement.AppendChild(yearElement);
                    }

                    XmlNode? priceNode = bookElement.SelectSingleNode("price");
                    if (priceNode != null)
                    {
                        priceNode.InnerText = book.price.ToString();
                    }
                    else
                    {
                        XmlElement priceElement = xmlDoc.CreateElement("price");
                        priceElement.InnerText = book.price.ToString();
                        bookElement.AppendChild(priceElement);
                    }

                    xmlDoc.Save(storePath);
                    result.IsSuccessful = true;
                    result.Message = "Book updated successfully";
                }
                else
                {
                    result.Message = "Book with specified ISBN not found.";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Failed to edit book: {ex.Message}";
            }

            return result;
        }

        public OperationResult DeleteBookById(ulong bookId)
        {
            OperationResult result = new();

            try
            {
                XmlDocument xmlDoc = new();
                xmlDoc.Load(storePath);

                XmlNode? bookNode = xmlDoc.SelectSingleNode($"//book[isbn='{bookId}']");

                if (bookNode != null)
                {
                    XmlNode? root = bookNode.ParentNode;

                    if (root != null)
                    {
                        root.RemoveChild(bookNode);
                        xmlDoc.Save(storePath);
                        result.IsSuccessful = true;
                        result.Message = $"Book with ISBN {bookId} deleted successfully.";
                    }
                }
                else
                {
                    result.Message = $"Book with ISBN {bookId} not found.";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Failed to delete book with ISBN {bookId}: {ex.Message}";
            }

            return result;
        }
    }
}