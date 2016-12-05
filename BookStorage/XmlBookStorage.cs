using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BookLogic;
using LogLogic;

namespace BookStorage
{
    public class XmlBookStorage : IBookStorage
    {
        private readonly ILogAdapter logger;
        public string FilePath { get; }

        public XmlBookStorage() : this(NLogAdapter.Logger, "Books.xml")
        {
        }

        public XmlBookStorage(string filePath) : this(NLogAdapter.Logger, filePath)
        {
        }

        public XmlBookStorage(ILogAdapter logger) : this(logger, "Books.xml")
        {
        }

        public XmlBookStorage(ILogAdapter logger, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new AggregateException();

            this.logger = logger;
            FilePath = filePath;
        }

        public void SaveBooks(IEnumerable<Book> books)
        {
            if (ReferenceEquals(books, null))
                throw new ArgumentNullException();

            try
            {
                using (var fileStream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    new XmlSerializer(typeof(List<Book>)).Serialize(fileStream, new List<Book>(books));
                }

                logger.Trace($"Saved {books.Count()} books to {Environment.CurrentDirectory}'\'{FilePath}");
            }

            catch (IOException ex)
            {
                logger.Error($"error {ex.GetType()} in\n {ex.StackTrace}\n whith message {ex.Message}");
                throw new Exception("Can't save data", ex);
            }
        }

        public IEnumerable<Book> LoadBooks()
        {
            List<Book> bookStorage = new List<Book>();

            try
            {
                using (var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer xmlData = new XmlSerializer(typeof(List<Book>));

                    //while (fileStream.Position < fileStream.Length)
                    //{
                        bookStorage = (List<Book>)xmlData.Deserialize(fileStream);
                    //}
                }

                logger.Trace($"Added {bookStorage.Count} books from {Environment.CurrentDirectory}'\'{FilePath}");
            }
            catch (IOException ex)
            {
                logger.Error($"error {ex.GetType()} in\n {ex.StackTrace}\n whith message {ex.Message}");
                throw new Exception("Can't load data", ex);
            }

            return bookStorage;
        }
    }
}
