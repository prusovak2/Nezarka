using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Linq;

namespace NezarkaBookstore
{
    //
    // Model
    //
//TODO:check dupliated id!!
//TODO: validni kladne pocty knih?
    public class ModelStore
    {
        internal List<Book> books = new List<Book>();
        private List<Customer> customers = new List<Customer>();

        public List<Book> GetBooks()
        {
            return books;
        }

        public Book GetBook(int id)
        {
            if (IsBook(id))
            {
                return books.Find(b => b.Id == id);
            }
            else
            {
                return null;
            }
        }

        public Customer GetCustomer(int id)
        {
            if (IsCustomer(id))
            {
                return customers.Find(c => c.Id == id);
            }
            return null;
        }

        public bool IsCustomer(int id)
        {
            return (customers.Where(x => x.Id == id).Count() == 1);
        }

        public bool IsBook(int id)
        {
            return (books.Where(x => x.Id == id).Count() == 1);
        }

        public static ModelStore LoadFrom(TextReader reader)
        {
            var store = new ModelStore();

            try
            {
                string line = reader.ReadLine();
                if (line != "DATA-BEGIN")
                {
                    //Console.WriteLine("Data error.");                    
                    return null;
                }
                while (true)
                {
                     line = reader.ReadLine();
                    if (line == null)
                    {
                        //Console.WriteLine("Data error.");
                        return null;
                    }
                    else if (line == "DATA-END")
                    {
                        break;
                    }

                    string[] tokens = line.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    switch (tokens[0])
                    {
                        case "BOOK":
                            if (tokens.Length != 5)
                            {
                                //Console.WriteLine("Data error.");
                                return null;
                            }
                            if (Book.TryParse(tokens[1], tokens[2], tokens[3], tokens[4], out Book b,  store))
                            {                               
                                store.books.Add(b);
                            }
                            else //parse failed, number was expected
                            { 
                                //Console.WriteLine("Data error.");
                                return null;
                            }
                            break;
                        case "CUSTOMER":
                            if (tokens.Length != 4)
                            {
                               // Console.WriteLine("Data error.");
                                return null;
                            }
                            if (Customer.TryParse(tokens[1], tokens[2], tokens[3], out Customer c, store))
                            {
                                store.customers.Add(c);
                            }
                            else //parse failed, number was expected
                            {
                                //Console.WriteLine("Data error.");
                                return null;
                            }
                            break;
                        case "CART-ITEM":
                            if (tokens.Length != 4)
                            {
                                //Console.WriteLine("Data error.");
                                return null;
                            }
                            if(!Int32.TryParse(tokens[1], out int custID))
                            {
                                //Console.WriteLine("Data error.");
                                return null;
                            }
                            var customer = store.GetCustomer(custID);
                            if (customer == null)
                            {
                                //Console.WriteLine("Data error.");
                                return null;
                            }
                            if(ShoppingCartItem.TryParse(tokens[2], tokens[3], store, out ShoppingCartItem s))
                            {
                                customer.ShoppingCart.Items.Add(s);
                            }
                            else //parse failed, number was expected
                            {
                                //Console.WriteLine("Data error.");
                                return null;
                            }
                            break;
                        default:
                            //Console.WriteLine("Data error.");
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is IndexOutOfRangeException)
                {
                   // Console.WriteLine("Data error.");
                    return null;
                }
                throw;
            }

            return store;
        }
    }

     public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }

        public static bool TryParse(string id1, string title2, string author3, string price4, out Book b, ModelStore store)
        {
            if(!Int32.TryParse(id1, out int id))
            {
                b = null;
                return false;
            }
            //check bookId duplicity
            if (store.IsBook(id) || id<0)
            {
                b = null;
                return false;
            }
            if(!decimal.TryParse(price4, out decimal price))
            {
                b = null;
                return false;
            }
            if (price < 0)
            {
                b = null;
                return false;
            }
            b = new Book
            {
                Id = id,
                Title = title2,
                Author = author3,
                Price = price,
            };
            return true;
        }

    }

    public class Customer
    {
        private ShoppingCart shoppingCart;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<ShoppingCartItem> GetCart()
        {
            return ShoppingCart.Items;
        }
        public int CountItemsInCart()
        {
            if (shoppingCart == null)
            {
                return 0;
            }
            return shoppingCart.Items.Count;
        }
        public ShoppingCart ShoppingCart
        {
            get
            {
                if (shoppingCart == null)
                {
                    shoppingCart = new ShoppingCart();
                }
                return shoppingCart;
            }
            set
            {
                shoppingCart = value;             
            }
        }
        public static bool TryParse(string id1, string name2, string surname3, out Customer c,ModelStore store)
        {
            if (!Int32.TryParse(id1, out int id))
            {
                c = null;
                return false;
            }
            if (store.IsCustomer(id) || id<0)
            {
                c = null;
                return false;
            }
            c = new Customer
            {
                Id = id,
                FirstName = name2,
                LastName = surname3
            };
            return true;
            
        }
    }

    public class ShoppingCartItem
    {
        public int BookId { get; set; }
        public int Count { get; set; }

        public static bool TryParse(string bookId1, string count2, ModelStore store, out ShoppingCartItem s)
        {
            if (!Int32.TryParse(bookId1, out int bookId))
            {
                s = null;
                return false;
            }
            Book b = store.GetBook(bookId);
            if (b == null)
            {
                s = null;
                return false;
            }
            if (!Int32.TryParse(count2, out int count))
            {
                s = null;
                return false;
            }
            if (count < 0)
            {
                s = null;
                return false;
            }
            s = new ShoppingCartItem
            {
                BookId = bookId,
                Count = count,
            };
            return true;
        }
    }

    public class ShoppingCart
    {
        public int CustomerId { get; set; }
        public List<ShoppingCartItem> Items = new List<ShoppingCartItem>();

        public bool ContainsBook(int BookId)
        {
           return (Items.Where(x => x.BookId == BookId).Count() == 1);
        }

        public void AddItem(int bookId)
        {
            if (ContainsBook(bookId))
            {
                ShoppingCartItem item = Items.Find(x => x.BookId == bookId);
                item.Count++;
            }
            else
            {
                ShoppingCartItem newItem = new ShoppingCartItem()
                {
                    BookId = bookId,
                    Count = 1,
                };
                Items.Add(newItem);
            }
        }
        public bool RemoveItem(int bookId)
        {
            if (ContainsBook(bookId))
            {
                ShoppingCartItem item = Items.Find(x => x.BookId == bookId);
                if(item.Count > 1)
                {
                    item.Count--;
                }
                else
                {
                    Items.Remove(item);
                }
                return true;
            }
            return false;
        }
    }
}
