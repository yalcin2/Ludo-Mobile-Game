using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSystem
{
    class BuisnessManager
    {
        static BuisnessManager singleton;
        static StoreEntities db = new StoreEntities();

        public static BuisnessManager Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new BuisnessManager();
                return singleton;
            }
        }

        public void SaveToDatabase()
        {
            DataManager.Instance.SaveToDatabase();
        }

        // Customer & Employee methods

        public bool AddCustomer(Customer customerToAdd)
        {
            bool result = false;
            Customer c = DataManager.Instance.GetCustomerByID(customerToAdd.CustomerID);
            if (c == null)
            {
                DataManager.Instance.AddCustomer(customerToAdd);
                result = true;
            }
            return result;
        }

        public bool ValidateCustomer(string Username, string Password)
        {
            return DataManager.Instance.ValidateCustomer(Username, Password);
        }

        public bool ValidateEmployee(string Username, string Password)
        {
            return DataManager.Instance.ValidateEmployee(Username, Password);
        }

        public Customer GetCustomerByID(int id)
        {
            return DataManager.Instance.GetCustomerByID(id);
        }

        public Customer GetCustomerByUsername(string Username)
        {
            return DataManager.Instance.GetCustomerByUsername(Username);
        }

        public Customer GetCustomerByEmail(string Email)
        {
            return DataManager.Instance.GetCustomerByEmail(Email);
        }

        public int GetIdByUsername(string Username)
        {
            return DataManager.Instance.GetIdByUsername(Username);
        }


        // Product methods

        public bool AddProduct(Product productToAdd)
        {
            bool result = false;
            Product p = DataManager.Instance.GetProductByID(productToAdd.ProductID);
            if (p == null)
            {
                DataManager.Instance.AddProduct(productToAdd);
                result = true;
            }
            return result;
        }

        public Product GetProductByID(int id)
        {
            return DataManager.Instance.GetProductByID(id);
        }

        public void DeleteProduct(Product productToDelete)
        {
            DataManager.Instance.DeleteProduct(productToDelete);
        }

        public List<Product> GetProduct()
        {
            return DataManager.Instance.GetProduct();
        }

        public int ValidateStock(int id)
        {
            return DataManager.Instance.ValidateStock(id);
        }


        // Order methods

        public void AddOrder(Order orderToAdd)
        {
            DataManager.Instance.AddOrder(orderToAdd);
        }

        public List<Order> GetOrderByCustomerID(int id)
        {
            return DataManager.Instance.GetOrderByCustomerID(id);
        }

        public List<Order> GetOrderByProductID(int id)
        {
            return DataManager.Instance.GetOrderByProductID(id);
        }

        public List<Order> GetOrder(Guid id)
        {
            return DataManager.Instance.GetOrder(id);
        }

        public List<Order> GetStatus(string status)
        {
            return DataManager.Instance.GetStatus(status);
        }

        public Order GetOrderByID(Guid id)
        {
            return DataManager.Instance.GetOrderByID(id);
        }

    }
}
