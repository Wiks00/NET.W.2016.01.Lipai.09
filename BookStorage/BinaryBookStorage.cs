using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BookLogic;
using LogLogic;

namespace BookStorage
{
    public class BinaryBookStorage : IBookStorage
    {
        private readonly ILogAdapter logger;
        public string FilePath { get; }

        public BinaryBookStorage(string filePath) : this(NLogAdapter.Logger, filePath)
        {
        }

        public BinaryBookStorage(ILogAdapter logger) : this(logger, "Books.txt")
        {
        }

        public BinaryBookStorage(ILogAdapter logger, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new AggregateException();

            this.logger = logger;
            FilePath = filePath;
        }

        public void SaveBooks(IEnumerable<Book> books)
        {
            if(ReferenceEquals(books,null))
                throw new ArgumentNullException();
            try
            {
                using (var writer = new BinaryWriter(File.Open(FilePath, FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    foreach (var book in books)
                    {
                        writer.Write(book.Title);
                        writer.Write(book.Author);
                        writer.Write(book.ReleaseDate.ToString("d"));
                        writer.Write(book.Price);
                    }
                }
                logger.Trace($"Saved {books.Count()} books to {Environment.CurrentDirectory}'\'{FilePath}");
            }
            catch (IOException ex)
            {
                logger.Error($"error {ex.GetType()} in\n {ex.StackTrace}\n whith message {ex.Message}");
                throw new Exception("Can't save data",ex);
            }
        }

        public IEnumerable<Book> LoadBooks()
        {
            List<Book> bookStorage = new List<Book>();
            try
            {
                using (var reader = new BinaryReader(File.Open(FilePath, FileMode.OpenOrCreate, FileAccess.Read)))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        var title = reader.ReadString();
                        var author = reader.ReadString();
                        var releaseDate = Convert.ToDateTime(reader.ReadString());
                        var price = reader.ReadDouble();

                        bookStorage.Add(new Book(price,title,author,releaseDate));
                    }
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
