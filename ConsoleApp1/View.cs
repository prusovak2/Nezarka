using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace NezarkaBookstore
{
    public static class View
    {
        internal static readonly CultureInfo StdCulture = new CultureInfo("en-US");
        internal static void GenFirstHead(TextWriter writer)
        {
            writer.WriteLine(@"<!DOCTYPE html>");
            writer.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            writer.WriteLine("<head>");
            writer.WriteLine("	<meta charset=\"utf-8\" />");
            writer.WriteLine("	<title>Nezarka.net: Online Shopping for Books</title>");
            writer.WriteLine("</head>");
            writer.WriteLine("<body>");
            writer.Flush();
        }
        internal static void GenBodyOfInvalidRequest(TextWriter writer)
        {
            writer.WriteLine("<p>Invalid request.</p>");
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Flush();
        }
        public static void GenInvalidRequest(TextWriter writer)
        {
            GenFirstHead(writer);
            GenBodyOfInvalidRequest(writer);
        }
        public static void GenBookDetail(TextWriter writer, string CustFirtsName, int NumItems ,string BookName, string Author, decimal Price)
        {
            GenFirstHead(writer);
            GenStyle(writer);
            GenCommonHeader(writer, CustFirtsName, NumItems);
            GenBookInfoBody(writer, BookName, Author, Price);
        }
        public static void GenBookList(TextWriter writer, string CustFirtsName, int NumItems, List<Book> books)
        {
            GenFirstHead(writer);
            GenStyle(writer);
            GenCommonHeader(writer, CustFirtsName, NumItems);
            GenBooksTable(writer, books);
        }

        internal static void GenBooksTable(TextWriter writer, List<Book> Books)
        {
            writer.WriteLine("	Our books for you:");
            writer.WriteLine("	<table>");

            int BookCounter = 0;
            while (BookCounter < Books.Count)
            {
                writer.WriteLine("		<tr>");
                for (int i = 0; i < 3; i++)
                {
                    if (BookCounter < Books.Count)
                    {
                        GenBookRecord( writer, BookCounter, Books[BookCounter]);
                        BookCounter++;
                    }
                    else
                    {
                        break;
                    }
                }
                writer.WriteLine("		</tr>");
            }
            writer.WriteLine("	</table>");
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Flush();

        }
        internal static void GenBookRecord(TextWriter writer, int counter, Book book) 
        {
            writer.WriteLine("			<td style=\"padding: 10px;\">");
            writer.WriteLine($"				<a href=\"/Books/Detail/{counter}\">{book.Title}</a><br />");
            writer.WriteLine($"				Author: {book.Author}<br />");
            writer.WriteLine($"				Price: {book.Price.ToString(StdCulture)} EUR &lt;<a href=\"/ShoppingCart/Add/{counter}\">Buy</a>&gt;");
            writer.WriteLine("			</td>");
            writer.Flush();
        }
        internal static void GenStyle(TextWriter writer)
        {
            writer.WriteLine("	<style type=\"text/css\">");
            writer.WriteLine("		table, th, td {");
            writer.WriteLine("			border: 1px solid black;");
            writer.WriteLine("			border-collapse: collapse;");
            writer.WriteLine("		}");
            writer.WriteLine("		table {");
            writer.WriteLine("			margin-bottom: 10px;");
            writer.WriteLine("		}");
            writer.WriteLine("		pre {");
            writer.WriteLine("			line-height: 70%;");
            writer.WriteLine("		}");
            writer.WriteLine("	</style>");
            writer.Flush();

        }
        static internal void GenCommonHeader(TextWriter writer, string FirstName, int NumOfItems)
        {
            writer.WriteLine("	<h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");
            writer.WriteLine($"	{FirstName}, here is your menu:");
            writer.WriteLine("	<table>");
            writer.WriteLine("		<tr>");
            writer.WriteLine("			<td><a href=\"/Books\">Books</a></td>");
            writer.WriteLine($"			<td><a href=\"/ShoppingCart\">Cart ({NumOfItems})</a></td>");
            writer.WriteLine("		</tr>");
            writer.WriteLine("	</table>");
            writer.Flush();
        }
        static internal void GenBookInfoBody(TextWriter writer, string BookName, string Author, decimal Price)
        {           
            writer.WriteLine("	Book details:");
            writer.WriteLine($"	<h2>{BookName}</h2>");
            writer.WriteLine("	<p style=\"margin-left: 20px\">");
            writer.WriteLine($"	Author: {Author}<br />");
            writer.WriteLine($"	Price: {Price.ToString(StdCulture)} EUR<br />");
            writer.WriteLine("	</p>");
            writer.WriteLine("	<h3>&lt;<a href=\"/ShoppingCart/Add/3\">Buy this book</a>&gt;</h3>");
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Flush();
        }
        public static void GenCart()
        {

        }
    }
}
