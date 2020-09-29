using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSystem
{
    class DataManager
    {
        static DataManager singleton;
        static StoreEntities db = new StoreEntities();

        public static DataManager Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new DataManager();
                return singleton;
            }
        }

        public DataManager()
        {
            db = new StoreEntities();
        }

        public void SaveToDatabase()
        {
            db.SaveChanges();
        }

        // Customer & Employee methods

        public void AddCustomer(Customer customerToAdd)
        {
            db.Customers.Add(customerToAdd);
            db.SaveChanges();
        }

        public bool ValidateCustomer(string Username, string Password)
        {
            Customer customer = db.Customers.SingleOrDefault(c => c.Username == Username && c.Password == Password);
            if (customer == null)
                return false;
            else
                return true;
        }

        public bool ValidateEmployee(string Username, string Password)
        {
            Employee employee = db.Employees.SingleOrDefault(e => e.Username == Username && e.Password == Password);
            if (employee == null)
                return false;
            else
                return true;
        }

        public Customer GetCustomerByID(int id)
        {
            return db.Customers.SingleOrDefault(c => c.CustomerID == id);
        }

        public Customer GetCustomerByUsername(string Username)
        {
            return db.Customers.SingleOrDefault(c => c.Username == Username);
        }

        public Customer GetCustomerByEmail(string Email)
        {
            return db.Customers.SingleOrDefault(c => c.Email == Email);
        }

        public int GetIdByUsername(string Username)
        {
            return db.Customers.Single(c => c.Username == Username).CustomerID;
        }

        // Product methods
        
        public void AddProduct(Product productToAdd)
        {
            db.Products.Add(productToAdd);
            db.SaveChanges();
        }

        public Product GetProductByID(int id)
        {
            return db.Products.SingleOrDefault(p => p.ProductID == id);
        }

        public void DeleteProduct(Product productToDelete)
        {
            db.Products.Remove(productToDelete);
            db.SaveChanges();
        }

        public List<Product> GetProduct()
        {
            return db.Products.ToList();
        }

        public int ValidateStock(int productid)
        {
            return db.Products.Single(p => p.ProductID == productid).Stock;
        }

        // Order methods

        public void AddOrder(Order orderToAdd)
        {
            db.Orders.Add(orderToAdd);
            db.SaveChanges();
        }

        public List<Order> GetOrderByCustomerID(int id)
        {
            return db.Orders.Where(o => o.CustomerID == id).ToList();
        }

        public List<Order> GetOrderByProductID(int id)
        {
            return db.Orders.Where(o => o.ProductID == id).ToList();
        }

        public List<Order> GetOrder(Guid id)
        {
            return db.Orders.Where(o => o.OrderID == id).ToList();
        }

        public List<Order> GetStatus(string status)
        {
            return db.Orders.Where(o => o.Status == status).ToList();
        }

        public Order GetOrderByID(Guid id)
        {
            return db.Orders.Single(o => o.OrderID == id);
        }


    }
}

