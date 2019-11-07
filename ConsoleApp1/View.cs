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
        public static void GenBookDetail(TextWriter writer, string CustFirtsName, int NumItems ,string BookName, string Author, decimal Price, int bookID)
        {
            GenFirstHead(writer);
            GenStyle(writer);
            GenCommonHeader(writer, CustFirtsName, NumItems);
            GenBookInfoBody(writer, BookName, Author, Price, bookID);
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
                        GenBookRecord( writer, Books[BookCounter]);
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
        internal static void GenBookRecord(TextWriter writer, Book book) 
        {
            writer.WriteLine("			<td style=\"padding: 10px;\">");
            writer.WriteLine($"				<a href=\"/Books/Detail/{book.Id}\">{book.Title}</a><br />");
            writer.WriteLine($"				Author: {book.Author}<br />");
            writer.WriteLine($"				Price: {book.Price.ToString(StdCulture)} EUR &lt;<a href=\"/ShoppingCart/Add/{book.Id}\">Buy</a>&gt;");
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
        static internal void GenBookInfoBody(TextWriter writer, string BookName, string Author, decimal Price, int bookID)
        {           
            writer.WriteLine("	Book details:");
            writer.WriteLine($"	<h2>{BookName}</h2>");
            writer.WriteLine("	<p style=\"margin-left: 20px\">");
            writer.WriteLine($"	Author: {Author}<br />");
            writer.WriteLine($"	Price: {Price.ToString(StdCulture)} EUR<br />");
            writer.WriteLine("	</p>");
            writer.WriteLine($"	<h3>&lt;<a href=\"/ShoppingCart/Add/{bookID}\">Buy this book</a>&gt;</h3>");
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Flush();
        }
        public static void GenCart(TextWriter writer, string CustFirtsName, List<ShoppingCartItem> Items, ModelStore store)
        {
            GenFirstHead(writer);
            GenStyle(writer);
            GenCommonHeader(writer, CustFirtsName, Items.Count);
            writer.WriteLine("	Your shopping cart:");
            writer.WriteLine("	<table>");
            writer.WriteLine("		<tr>");
            writer.WriteLine("			<th>Title</th>");
            writer.WriteLine("			<th>Count</th>");
            writer.WriteLine("			<th>Price</th>");
            writer.WriteLine("			<th>Actions</th>");
            writer.WriteLine("		</tr>");

            decimal TotalPrice = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                int NumOfBooks = Items[i].Count;
                Book book = store.GetBook(Items[i].BookId);
                GenCartItem(writer, book, NumOfBooks, ref TotalPrice);
            }

            writer.WriteLine("	</table>");
            writer.WriteLine($"	Total price of all items: {TotalPrice} EUR");
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Flush();

        }
        internal static void GenCartItem(TextWriter writer, Book book, int NumOfBooks, ref decimal TotalPrice)
        {            
            string title = book.Title;
            decimal price = book.Price;
            int bookID = book.Id;
            TotalPrice += (NumOfBooks*price);


            writer.WriteLine("		<tr>");
            writer.WriteLine($"			<td><a href=\"/Books/Detail/{bookID}\">{title}</a></td>");
            writer.WriteLine($"			<td>{NumOfBooks}</td>");

            if (NumOfBooks > 1)
            {
                writer.WriteLine($"			<td>{NumOfBooks} * {price} = {NumOfBooks*price} EUR</td>");
            }
            else
            {
                writer.WriteLine($"			<td>{price} EUR</td>");
            }            
            writer.WriteLine($"			<td>&lt;<a href=\"/ShoppingCart/Remove/{bookID}\">Remove</a>&gt;</td>");
            writer.WriteLine("		</tr>");
        }

        public static void GenCartEmpty(TextWriter writer,string FirstName)
        {
            GenFirstHead(writer);
            GenStyle(writer);
            GenCommonHeader(writer,FirstName, 0);
            writer.WriteLine("	Your shopping cart is EMPTY.");
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Flush();

        }
    }
}
