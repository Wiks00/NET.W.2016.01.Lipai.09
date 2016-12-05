using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLogic
{
    [Serializable]
    public class Book : IEquatable<Book>,IComparable<Book>,IComparable
    {

        private double price;
        private string title;
        private string author;
        private DateTime releaseDate;

        public double Price
        {
            get { return price; }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();

                price = value;
            }
        }

        public string Title
        {
            get { return title; }

            set
            {
                if (string.IsNullOrEmpty(value) || !char.IsLetterOrDigit(value, 0))
                    throw new ArgumentException();

                title = value;
            }
        }

        public string Author
        {
            get { return author; }

            set
            {
                if (string.IsNullOrEmpty(value) || !char.IsLetterOrDigit(value, 0))
                    throw new ArgumentException();

                author = value;
            }
        }

        public DateTime ReleaseDate { get; set; }

        public Book() { }

        public Book(double price, string title, string author, DateTime releasDate)
        {
            Price = price;
            Title = title;
            Author = author;
            ReleaseDate = releasDate;
        }

        public override int GetHashCode()
        {
            return 8 * (int)(Price - 4) + ((ReleaseDate.Day*ReleaseDate.Year)^Title.Length);
        }

        public override string ToString()
        {
            return $"Price: {Price}\n Title: {Title}\n Author: {Author}\n ReleaseDate: {ReleaseDate.Date:d}";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj) || obj.GetType() != GetType())
                return false;
            return ReferenceEquals(this, obj) || Equals(obj as Book);
        }

        public bool Equals(Book other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (!Price.Equals(other.Price)) return false;
            if (!ReleaseDate.Equals(other.ReleaseDate)) return false;
            if (!Title.Equals(other.Title)) return false;

            return Author.Equals(other.Author);
        }

        public int CompareTo(Book other)
        {
            if(ReferenceEquals(other,null))
                throw new ArgumentNullException();

            return Price.CompareTo(other.Price);
        }

        int IComparable.CompareTo(object obj)
        {
            if(ReferenceEquals(obj,null))
                throw new ArgumentOutOfRangeException();
            var book = obj as Book;
            if(ReferenceEquals(book,null))
                throw new ArgumentException();
            return CompareTo(book);
        }
    }
}
