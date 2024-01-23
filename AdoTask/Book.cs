using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoTask
{
    public class Book
    {

        public string BookName { get; set; }

        public override string ToString()
        {
            return BookName;
        }
    }
}
