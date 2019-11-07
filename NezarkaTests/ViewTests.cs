using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NezarkaBookstore;
using System.IO;

namespace NezarkaTests
{
    [TestClass]
    public class ViewTests
    {
        [TestMethod]
        public void GenFirstHeadTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\FirtsHeader.html");
            View.GenFirstHead(sw);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\FirtsHeader.html", @"TestFiles\FirstHeader.txt");
            Assert.IsTrue(b);           
        }
        [TestMethod]
        public void GenInvalidRequestTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\InvalidRequest.html");
            View.GenInvalidRequest(sw);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\InvalidRequest.html", @"TestFiles\InvalidRequest.txt");
            Assert.IsTrue(b);
        }
        [TestMethod]
        public void GenStyleTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\Style.html");
            View.GenStyle(sw);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\Style.html", @"TestFiles\Style.txt");
            Assert.IsTrue(b);
        }
        [TestMethod]
        public void GenCommonHeaderTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\CommonHeader.html");
            View.GenCommonHeader(sw, "Kachna", 42);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\CommonHeader.html", @"TestFiles\CommonHeader.txt");
            Assert.IsTrue(b);
        }
        [TestMethod]
        public void GenBookInfoBodyTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\BookInfoBody.html");
            View.GenBookInfoBody(sw, "HP", "JKR", (decimal)4.2, 3);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\BookInfoBody.html", @"TestFiles\BookInfoBody.txt");
            Assert.IsTrue(b);
        }
        [TestMethod]
        public void DecimalConvert()
        {
            decimal d = (decimal)2.1;
            string A = d.ToString(View.StdCulture);
            string B = "2.1";
            Assert.IsTrue(Utils.Diff(A, B));
        }
        [TestMethod]
        public void GenBookInfoTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\BookInfo.html");
            View.GenBookDetail(sw, "Kachna", 42, "HP", "JKR", (decimal)4.2,3);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\BookInfo.html", @"TestFiles\BookInfo.txt");
            Assert.IsTrue(b);
        }
        [TestMethod]
        public void GenBookListTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\BookList.html");
            List < Book > books= new List<Book>();
            for (int i = 0; i < 7; i++)
            {
                Book book = new Book(){
                Id = i,
                Title = $"Title{i}",
                Author = $"Author{i}",
                Price = i,
            };
                books.Add(book);
            }
            

            View.GenBookList(sw,"Kachna", 42, books);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\BookList.html", @"TestFiles\BookList.txt");
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void GenCartTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\Cart.html");
            List<Book> books = new List<Book>();
            for (int i = 0; i < 7; i++)
            {
                Book book = new Book()
                {
                    Id = i,
                    Title = $"Title{i}",
                    Author = $"Author{i}",
                    Price = i,
                };
                books.Add(book);
            }
            ModelStore store = new ModelStore();
            store.books = books;

            List<ShoppingCartItem> Items = new List<ShoppingCartItem>();
            for (int i = 0; i < 5; i++)
            {
                ShoppingCartItem item = new ShoppingCartItem()
                {
                    BookId = i,
                    Count = i,
                };
                Items.Add(item);
                
            }


            View.GenCart(sw, "Kachna", Items, store);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\Cart.html", @"TestFiles\Cart.txt");
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void GenCartTest2()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\Cart2.html");
            List<Book> books = new List<Book>();
            for (int i = 0; i < 8; i++)
            {
                Book book = new Book()
                {
                    Id = (i * i * i + 150) / (i + 1),
                    Title = $"Title Book{i * 2 + 1}",
                    Author = $"Author{i + 42 * 57 - 8}",
                    Price = i * i + 15 * i,
                };
                books.Add(book);
            }
            ModelStore store = new ModelStore();
            store.books = books;

            Customer cust = new Customer();


            IList<Book> booklist = store.GetBooks();

            List<ShoppingCartItem> Items = new List<ShoppingCartItem>();
            for (int i = 0; i < booklist.Count; i++)
            {
                ShoppingCartItem item = new ShoppingCartItem()
                {
                    BookId = booklist[i].Id,
                    Count = i * 150 + 10,
                };
                Items.Add(item);
            }


            View.GenCart(sw, "Kachna", Items, store);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\Cart2.html", @"TestFiles\Cart2.txt");
            Assert.IsTrue(b);
        }
        [TestMethod]
        public void GenCartEmptyTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\CartEmpty.html");
            View.GenCartEmpty(sw, "Kachna");
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\CartEmpty.html", @"TestFiles\CartEmpty.txt");
            Assert.IsTrue(b);
        }
        [TestMethod]
        public void GeneralTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\NezarkaTest.html");
            StreamReader Reader = new StreamReader(@"TestFiles\NezarkaTest.in");
            ModelStore store = ModelStore.LoadFrom(Reader);
            Controler.ReadAdnDoRequests(Reader, sw, store);
            sw.Close();
            Reader.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\NezarkaTest.html", @"TestFiles\NezarkaTest.out");
            Assert.IsTrue(b);
        }
    }
}
