using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLogic
{
    public interface ILogAdapter
    {
        void Error(string message);
        void Trace(string message);
        void Info(string message);
    }
}
