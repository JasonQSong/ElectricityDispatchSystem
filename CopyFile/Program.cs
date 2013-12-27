using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyFile
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() == 3)
            {
                System.IO.File.Copy(args[1], args[2]);
            }
        }
    }
}
