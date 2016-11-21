using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLogic
{
    public class Book : IEquatable<Book>,IComparable<Book>,IComparable
    {
        public double Price { get; }
        public string Title { get; }
        public string Author { get; }
        public DateTime ReleaseDate { get; }

        public Book(double price, string title, string author, DateTime releasDate)
        {
            if(price <0)
                throw new ArgumentOutOfRangeException();

            if(string.IsNullOrEmpty(title) || !char.IsLetterOrDigit(title,0) || string.IsNullOrEmpty(author) || !char.IsLetterOrDigit(author, 0))
                throw new ArgumentException();

            if(ReferenceEquals(releasDate,null))
                throw new ArgumentNullException();

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
