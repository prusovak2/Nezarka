using System;
using System.IO;
using System.Text;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NezarkaTests")]
namespace NezarkaBookstore
{
    class Program
    {
        static void Main(string[] args)
        {
            Stream s = Console.OpenStandardInput();
            Stream o = Console.OpenStandardOutput();
            StreamReader r = new StreamReader(s);
            string line =r.ReadLine();
            StreamWriter w = new StreamWriter(o);
            w.AutoFlush = true;
            Console.SetOut(w);
            w.WriteLine(line);
            
        }
    }
}
