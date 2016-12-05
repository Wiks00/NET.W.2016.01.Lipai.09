using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookService;
using BookLogic;
using BookStorage;
using LogLogic;
using Book = BookLogic.Book;

namespace Book.ConsoleTests
{
    class Program 
    {
        static void Main(string[] args)
        {

            var BookService =  new BookService.BookService(NLogAdapter.Logger);

            BookService.Load(new BinaryBookStorage("text.txt"));
            BookService.Add(new BookLogic.Book(1, "qq", "ww", new DateTime(2016, 1, 1)));
            BookService.Save(new XmlBookStorage());

            Console.ReadKey();
        }
    }
}

