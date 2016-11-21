using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLogic;

namespace BookStorage
{
    public interface IBookStorage
    {
        void SaveBooks(IEnumerable<Book> books);

        IEnumerable<Book> LoadBooks();
    }
}
