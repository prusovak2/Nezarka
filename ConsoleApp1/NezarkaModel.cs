using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace NezarkaBookstore
{
    //
    // Model
    //
//TODO:check dupliated id!!
    class ModelStore
    {
        private List<Book> books = new List<Book>();
        private List<Customer> customers = new List<Customer>();

        public IList<Book> GetBooks()
        {
            return books;
        }

        public Book GetBook(int id)
        {
            return books.Find(b => b.Id == id);
        }

        public Customer GetCustomer(int id)
        {
            return customers.Find(c => c.Id == id);
        }

        public static ModelStore LoadFrom(TextReader reader)
        {
            var store = new ModelStore();

            try
            {
                if (reader.ReadLine() != "DATA-BEGIN")
                {
                    Console.WriteLine("Data error.");
                    return null;
                }
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        Console.WriteLine("Data error.");
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
                                Console.WriteLine("Data error.");
                                return null;
                            }
                            if (Book.TryParse(tokens[1],tokens[2], tokens[3], tokens[4], out Book b))
                            {
                                store.books.Add(b);
                            }
                            else //parse failed, number was expected
                            { 
                                Console.WriteLine("Data error.");
                                return null;
                            }
                            break;
                        case "CUSTOMER":
                            if (tokens.Length != 4)
                            {
                                Console.WriteLine("Data error.");
                                return null;
                            }
                            if (Customer.TryParse(tokens[1], tokens[2], tokens[3], out Customer c))
                            {
                                store.customers.Add(c);
                            }
                            else //parse failed, number was expected
                            {
                                Console.WriteLine("Data error.");
                                return null;
                            }
                            break;
                        case "CART-ITEM":
                            if (tokens.Length != 4)
                            {
                                Console.WriteLine("Data error.");
                                return null;
                            }
                            if(!Int32.TryParse(tokens[1], out int custID))
                            {
                                Console.WriteLine("Data error.");
                                return null;
                            }
                            var customer = store.GetCustomer(custID);
                            if (customer == null)
                            {
                                Console.WriteLine("Data error.");
                                return null;
                            }
                            if(ShoppingCartItem.TryParse(tokens[2], tokens[3], store, out ShoppingCartItem s))
                            {
                                customer.ShoppingCart.Items.Add(s);
                            }
                            else //parse failed, number was expected
                            {
                                Console.WriteLine("Data error.");
                                return null;
                            }
                            break;
                        default:
                            Console.WriteLine("Data error.");
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is IndexOutOfRangeException)
                {
                    return null;
                }
                throw;
            }

            return store;
        }
    }

     class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }

        public static bool TryParse(string id1, string title2, string author3, string price4, out Book b)
        {
            if(!Int32.TryParse(id1, out int id))
            {
                b = null;
                return false;
            }
            if(!decimal.TryParse(price4, out decimal price))
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

    class Customer
    {
        private ShoppingCart shoppingCart;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

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
        public static bool TryParse(string id1, string name2, string surname3, out Customer c)
        {
            if (!Int32.TryParse(id1, out int id))
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

    class ShoppingCartItem
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
            s = new ShoppingCartItem
            {
                BookId = bookId,
                Count = count,
            };
            return true;
        }
    }

    class ShoppingCart
    {
        public int CustomerId { get; set; }
        public List<ShoppingCartItem> Items = new List<ShoppingCartItem>();
    }
}
