using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLogic;
using BookStorage;
using LogLogic;

namespace BookService
{
    public class BookService
    {
        private readonly ILogAdapter logger;
        private SortedSet<Book> booksStorage;

        public int Count => booksStorage.Count;
        public bool IsReadOnly => false;

        public BookService(ILogAdapter logger)
        {
            this.logger = logger;
            booksStorage = new SortedSet<Book>(Comparer<Book>.Default);
        }

        public BookService(IComparer<Book> comparer, ILogAdapter logger)
        {
            if (ReferenceEquals(comparer, null))
                comparer = Comparer<Book>.Default;
            if(ReferenceEquals(logger,null))
                throw new ArgumentNullException();

            this.logger = logger;
            booksStorage = new SortedSet<Book>(comparer);
        }

        public BookService(Comparison<Book> comparer, ILogAdapter logger) : this(new ComparisonToInterfacerAdapter<Book>(comparer),logger)
        {
        }

        public BookService(IEnumerable<Book> collection, IComparer<Book> comparer , ILogAdapter logger )
        {
            if (ReferenceEquals(collection, null))
            {
                logger.Error($"Faild to create new books storage -> collection is null\n {new StackTrace()}");
                throw new ArgumentNullException();
            }
            if (ReferenceEquals(comparer, null))
                comparer = Comparer<Book>.Default;

            this.logger = logger;
            booksStorage = new SortedSet<Book>(collection, comparer);
        }

        public BookService(IEnumerable<Book> collection, ILogAdapter logger) : this(collection, null, logger)
        {
        }

        public void Add(Book item)
        {
            if (ReferenceEquals(item, null))
            {
                logger.Error($"Faild to add new item into books storage , item is null\n {new StackTrace()}");
                throw new ArgumentNullException();
            }
            var res = booksStorage.Add(item);
            logger.Info($"Added {item} with {res} result");
        }

        public bool Remove(Book item)
        {
            if (ReferenceEquals(item, null))
            {
                logger.Error($"Faild to remove new item into books storage , item is null\n {new StackTrace()}");
                throw new ArgumentNullException();
            }
            var res = booksStorage.Remove(item);
            logger.Info($"Remove {item} with {res} result");
            return res;
        }

        public void SortByTag(IComparer<Book> comparer)
        {
            if (ReferenceEquals(comparer, null))
                throw new ArgumentNullException();
            booksStorage = new SortedSet<Book>(booksStorage, comparer);
        }

        public void SortByTag(Comparison<Book> comparer) => SortByTag(new ComparisonToInterfacerAdapter<Book>(comparer));

        public Book FindBookByTag(IEquatable<Book> comparer)
        {
            return (from b in booksStorage
                    where comparer.Equals(b)
                    select new Book(b.Price, b.Title, b.Author, b.ReleaseDate)).FirstOrDefault();
        }

        public Book FindBookByTag(Predicate<Book> comparer) => FindBookByTag(new PredicateToInterfacerAdapter<Book>(comparer));

        public void Load(IBookStorage storage)
        {
            if(ReferenceEquals(storage,null))
                throw new ArgumentNullException();
            try
            {
                booksStorage = new SortedSet<Book>(storage.LoadBooks());
            }
            catch (IOException ex)
            {
                throw new IOException("Can't load books", ex);
            }
        }

        public void Save(IBookStorage storage)
        {
            if (ReferenceEquals(storage, null))
                throw new ArgumentNullException();
            try
            {
                storage.SaveBooks(booksStorage);
            }
            catch (IOException ex)
            {
                throw new IOException("Can't save books", ex);
            }
        }

        internal class ComparisonToInterfacerAdapter<T> : IComparer<T>
        {
            private readonly Comparison<T> comparer;

            public ComparisonToInterfacerAdapter(Comparison<T> comparer)
            {
                if (ReferenceEquals(comparer, null))
                    throw new ArgumentNullException();
                this.comparer = comparer;
            }

            public int Compare(T x, T y)
            {
                return comparer(x, y);
            }

        }

        internal class PredicateToInterfacerAdapter<T> : IEquatable<T>
        {
            private readonly Predicate<T> comparer;

            public PredicateToInterfacerAdapter(Predicate<T> comparer)
            {
                if (ReferenceEquals(comparer, null))
                    throw new ArgumentNullException();
                this.comparer = comparer;
            }

            public bool Equals(T other)
            {
                return comparer(other);
            }

        }
    }
}
