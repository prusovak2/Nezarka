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
            Stream InputStrem = Console.OpenStandardInput();
            Stream Outputstream = Console.OpenStandardOutput();
            StreamReader Reader = new StreamReader(InputStrem);            
            StreamWriter Writer = new StreamWriter(Outputstream);
            //Writer.AutoFlush = true;
            Console.SetOut(Writer);
            ModelStore Store = ModelStore.LoadFrom(Reader);
            if (Store == null)
            {
                Writer.WriteLine("Data error."); //TODO:remove
                Writer.Flush();
                return;
            }
            Controler.ReadAdnDoRequests(Reader, Writer, Store);
            
            
        }
    }
}
