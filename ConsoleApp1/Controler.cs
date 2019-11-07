using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace NezarkaBookstore
{
    public static class Controler
    {
        public  static void ReadAdnDoRequests(TextReader reader, TextWriter writer, ModelStore store)
        {
            Regex BookList = new Regex(@"^GET \d+ http://www.nezarka.net/Books$");
            Regex BookDetail = new Regex(@"^GET \d+ http://www.nezarka.net/Books/Detail/\d+$");
            Regex ShopingCart = new Regex(@"^GET \d+ http://www.nezarka.net/ShoppingCart$");
            Regex ShopingCartAddItem = new Regex(@"^GET \d+ http://www.nezarka.net/ShoppingCart/Add/\d+$");
            Regex ShopingCartRemoveItem = new Regex(@"^GET \d+ http://www.nezarka.net/ShoppingCart/Remove/\d+$");

            char[] delims = new char[] { ' ', '/' };

           // bool first = true;

            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                string[] Records = line.Split(delims, StringSplitOptions.RemoveEmptyEntries);
                /*if (!first)
                {
                    writer.WriteLine("====");
                }
                first = false; */
                if (BookList.Match(line).Length > 0)
                {
                    int cusID = Int32.Parse(Records[1]);
                    Customer cust = store.GetCustomer(cusID);
                    
                    if (cust == null)
                    {
                        View.GenInvalidRequest(writer);
                        
                        break;
                    }
                    string firstName = cust.FirstName;
                    List<Book> Books = store.GetBooks();
                    int numItems = cust.CountItemsInCart();
                    View.GenBookList(writer, firstName, numItems, Books);
                }
                else if (BookDetail.Match(line).Length > 0)
                {
                    int cusID = Int32.Parse(Records[1]);
                    int bookID = Int32.Parse(Records[Records.Length - 1]);
                    Customer cust = store.GetCustomer(cusID);
                    Book b = store.GetBook(bookID);
                    if (cust == null || b==null)
                    {
                        View.GenInvalidRequest(writer);
                        break;
                    }
                    string firstNsame = cust.FirstName;
                    int numItems = cust.CountItemsInCart();
                                      
                    View.GenBookDetail(writer, firstNsame, numItems, b.Title, b.Author, b.Price);
                }
                else if (ShopingCart.Match(line).Length > 0)
                {
                    int cusID = Int32.Parse(Records[1]);
                    Customer cust = store.GetCustomer(cusID);
                    if (cust == null)
                    {
                        View.GenInvalidRequest(writer);
                        break;
                    }

                    string firstName = cust.FirstName;
                    if (cust.CountItemsInCart() == 0)
                    {
                        View.GenCartEmpty(writer, firstName);
                        break;
                    }
                    List<ShoppingCartItem> cart = cust.GetCart();

                    View.GenCart(writer, firstName, cart, store);
                }
                else if (ShopingCartAddItem.Match(line).Length > 0)
                {
                    int cusID = Int32.Parse(Records[1]);
                    int bookID = Int32.Parse(Records[Records.Length - 1]);
                    Customer cust = store.GetCustomer(cusID);
                    Book b = store.GetBook(bookID);
                    if (cust == null || b == null)
                    {
                        View.GenInvalidRequest(writer);
                        break;
                    }
                    //TODO: create shopping cart?
                    cust.ShoppingCart.AddItem(bookID);

                    string firstName = cust.FirstName;
                    List<ShoppingCartItem> cart = cust.GetCart();

                    View.GenCart(writer, firstName, cart, store);

                }
                else if (ShopingCartRemoveItem.Match(line).Length > 0)
                {
                    int cusID = Int32.Parse(Records[1]);
                    int bookID = Int32.Parse(Records[Records.Length - 1]);
                    Customer cust = store.GetCustomer(cusID);
                    Book b = store.GetBook(bookID);
                    if (cust == null || b == null)
                    {
                        View.GenInvalidRequest(writer);
                        break;
                    }
                    bool RemoveSuccesful = cust.ShoppingCart.RemoveItem(bookID);
                    if (RemoveSuccesful)
                    {
                        string firstName = cust.FirstName;
                        List<ShoppingCartItem> cart = cust.GetCart();
                        if (cust.CountItemsInCart() == 0)
                        {
                            View.GenCartEmpty(writer, firstName);
                            break;
                        }
                        View.GenCart(writer, firstName, cart, store);
                    }
                    else
                    {
                        View.GenInvalidRequest(writer);
                    }
                }
                else
                {
                    View.GenInvalidRequest(writer);
                }
                writer.WriteLine("====");
                // line = reader.ReadLine();
            }
        }
    }
}
