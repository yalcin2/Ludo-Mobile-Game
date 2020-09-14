using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StoreSystem
{
    class Program
    {
        static void AddProduct()
        {
            Console.Clear();
            Console.WriteLine();
            Product p = new Product();
            Console.Write("Enter product`s name: ");
            p.Name = Console.ReadLine();
            Console.Write("Enter product`s category: ");
            p.Category = Console.ReadLine();
            Console.Write("Enter product`s ID: ");
            p.ProductID = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter product`s price: ");
            p.Price = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Enter product`s stock: ");
            p.Stock = Convert.ToInt32(Console.ReadLine());

            bool validProduct = BuisnessManager.Instance.AddProduct(p);
            if (validProduct)
            {
                Console.Clear();
                Console.WriteLine("You have successfully added a new product!");
                EmployeeMenu();
            }
            else {
                Console.Clear();
                Console.WriteLine("ERROR: A Product with the indicated ID already exists");
                EmployeeMenu();
            }

        }

        static void EditProduct()
        {
            Console.Clear();
            Console.WriteLine();
            Console.Write("Enter product ID: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Product product = BuisnessManager.Instance.GetProductByID(id);
            if (product == null)
            {
                Console.Clear();
                Console.WriteLine("The product was not found");
                EmployeeMenu();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("What do you want to modify?");
                Console.WriteLine("1. Name");
                Console.WriteLine("2. Category");
                Console.WriteLine("3. Price");
                Console.WriteLine("4. Stock");
                Console.Write("Choose an option: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.Write("New name: ");
                        product.Name = Console.ReadLine();
                        Console.WriteLine("The product was updated.");
                        BuisnessManager.Instance.SaveToDatabase();
                        EmployeeMenu();
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("New category: ");
                        product.Category = Console.ReadLine();
                        Console.WriteLine("The product was updated.");
                        BuisnessManager.Instance.SaveToDatabase();
                        EmployeeMenu();
                        break;
                    case 3:
                        Console.Clear();
                        Console.Write("New price: ");
                        product.Price = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("The product was updated.");
                        BuisnessManager.Instance.SaveToDatabase();
                        EmployeeMenu();
                        break;
                    case 4:
                        Console.Clear();
                        Console.Write("Stock: ");
                        product.Stock = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("The product was updated.");
                        BuisnessManager.Instance.SaveToDatabase();
                        EmployeeMenu();
                        break;
                    default:
                        Console.Clear();
                        Console.Write("Invalid input ");
                        EmployeeMenu();
                        break;
                }
                
            }
        }

        static void DeleteProduct()
        {
            Console.Clear();
            Console.WriteLine();
            Console.Write("Enter product ID: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Product product = BuisnessManager.Instance.GetProductByID(id);
            if (product == null)
            {
                Console.Clear();
                Console.WriteLine("The product was not found.");
                EmployeeMenu();
            }
            else
            {

                Console.Clear();
                Console.WriteLine("The product was deleted.");
                BuisnessManager.Instance.DeleteProduct(product);
                EmployeeMenu();
            }
        }

        static void EmployeeOrderStatus()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter Order ID: ");
                string orderID = Console.ReadLine();
                Guid guidOrder = Guid.Parse(orderID);
                Order orderList = BuisnessManager.Instance.GetOrderByID(guidOrder);

                Console.Write("Order dispatched?[Yes/No]: ");
                string order = Console.ReadLine();

                if (order == "yes" || order == "Yes" || order == "YES")
                {
                    orderList.Status = "Dispatched";
                    BuisnessManager.Instance.SaveToDatabase();
                }
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                EmployeeMenu();
            }
            catch (SystemException)
            {
                Console.Clear();
                Console.WriteLine("Invalid Order ID: ID should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
                EmployeeMenu();
            }
            
        }

        // When called it will show the employee`s main menu
        static void EmployeeMenu()
        {
            bool loop = true;
            while (loop == true)
            {
                Console.WriteLine();
                Console.WriteLine("{ 1 } View “Awaiting Dispatch” Orders ");
                Console.WriteLine("{ 2 } View “Dispatched” Orders ");
                Console.WriteLine("{ 3 } View Order via Order ID ");
                Console.WriteLine("{ 4 } View all Orders of a particular Customer ");
                Console.WriteLine("{ 5 } View all Orders of a particular Product ");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("{ 6 } Add new product");
                Console.WriteLine("{ 7 } Edit existing product");
                Console.WriteLine("{ 8 } Delete product");
                Console.WriteLine("{ 9 } Update existing order");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("{ 10 } Log out");
                Console.WriteLine("{ 11 } Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        string arriveStatus = "Awaiting Dispatch";
                        List<Order> checkArriveStatus = BuisnessManager.Instance.GetStatus(arriveStatus);
                        foreach (Order cs in checkArriveStatus)
                        {
                            Console.WriteLine(cs.ToString());
                        }
                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "2":
                        Console.Clear();
                        string sentStatus = "Dispatched";
                        List<Order> checkSentStatus = BuisnessManager.Instance.GetStatus(sentStatus);
                        foreach (Order cs in checkSentStatus)
                        {
                            Console.WriteLine(cs.ToString());
                        }
                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "3":
                        try
                        {
                            Console.Clear();
                            Console.Write("Enter Order ID: ");
                            string orderID = Console.ReadLine();
                            Guid guidOrder = Guid.Parse(orderID);

                            List<Order> orderList = BuisnessManager.Instance.GetOrder(guidOrder);
                            foreach (Order o in orderList)
                            {
                                Console.WriteLine(o.ToString());
                            }
                            Console.WriteLine();
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        catch (SystemException)
                        {
                            Console.WriteLine("Invalid Order ID: ID should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
                        }
                        break;
                    case "4":
                        Console.Clear();
                        Console.Write("Enter Customer ID: ");
                        int customerID = Convert.ToInt32(Console.ReadLine());

                        List<Order> orderListCustomer = BuisnessManager.Instance.GetOrderByCustomerID(customerID);
                        foreach (Order oc in orderListCustomer)
                        {
                            Console.WriteLine(oc.ToString());
                        }
                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "5":
                        Console.Clear();
                        Console.Write("Enter Product ID: ");
                        int productID = Convert.ToInt32(Console.ReadLine());

                        List<Order> orderListProduct = BuisnessManager.Instance.GetOrderByProductID(productID);
                        foreach (Order op in orderListProduct)
                        {
                            Console.WriteLine(op.ToString());
                        }
                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "6":
                        AddProduct();
                        loop = false;
                        break;
                    case "7":
                        EditProduct();
                        loop = false;
                        break;
                    case "8":
                        DeleteProduct();
                        loop = false;
                        break;
                    case "9": 
                        EmployeeOrderStatus();
                        loop = false;
                        break;
                    case "10":
                        Console.Clear();
                        Console.WriteLine("You have successfully logged out of your account!");
                        loop = false;
                        MainMenu();
                        break;
                    case "11":
                        loop = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }

        static int currentCustomerID;

        static void OrderProduct()
        {
            Order o = new Order();
            o.OrderID = Guid.NewGuid();
            Console.Write("Enter the product`s ID: ");
            o.ProductID = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the quantity chosen: ");
            o.Quantity = Convert.ToInt32(Console.ReadLine());
            o.CustomerID = currentCustomerID;
            o.Status = "Awaiting Dispatch";
            
            
            Product product = BuisnessManager.Instance.GetProductByID(o.ProductID);
            if (product == null)
            {
                Console.Clear();
                Console.WriteLine("The product was not found.");
                CustomerMenu();
            }
            else
            {
                int stock = BuisnessManager.Instance.ValidateStock(o.ProductID);
                if (stock == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Stock for this product is empty.");
                }
                else if (o.Quantity > stock || o.Quantity <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("INVALID STOCK: You have entered an incorrect number of stock");
                }
                else
                {
                    Console.Clear();
                    product.Stock = product.Stock - o.Quantity;
                    BuisnessManager.Instance.SaveToDatabase();
                    BuisnessManager.Instance.AddOrder(o);
                    Console.WriteLine("Order saved!");
                }
            }
            
        }

        static void CustomerOrderStatus()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter Order ID: ");
                string orderID = Console.ReadLine();
                Guid guidOrder = Guid.Parse(orderID);
                Order orderList = BuisnessManager.Instance.GetOrderByID(guidOrder);

                Console.Write("Order delivered?[Yes/No]: ");
                string order = Console.ReadLine();

                if (order == "yes" || order == "Yes" || order == "YES")
                {
                    orderList.Status = "Delivered";
                    BuisnessManager.Instance.SaveToDatabase();
                }
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                CustomerMenu();
            }
            catch (SystemException)
            {
                Console.Clear();
                Console.WriteLine("Invalid Order ID: ID should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
                CustomerMenu();
            }
        }

        // When called, it will show the customer`s main menu
        static void CustomerMenu()
        {
            bool loop = true;
            while (loop == true)
            {
                Console.WriteLine();
                Console.WriteLine("{ 1 } View Available Products ");
                Console.WriteLine("{ 2 } View Order History");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("{ 3 } Log out");
                Console.WriteLine("{ 4 } Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        List<Product> productList = BuisnessManager.Instance.GetProduct();
                        foreach (Product p in productList)
                        {
                            Console.WriteLine(p.ToString());
                        }
                        Console.WriteLine();
                        Console.WriteLine("{ 1 } Order product/s");
                        Console.WriteLine("{ 2 } Back");
                        Console.Write("Choose an option: ");
                        string secondChoice = Console.ReadLine();

                        if (secondChoice == "1")
                            OrderProduct();
                        else if (secondChoice == "2")
                        {
                            Console.Clear();
                            break;
                        }
                        else
                            Console.Clear();
                        break;
                    case "2":
                        Console.Clear();
                        List<Order> orderList = BuisnessManager.Instance.GetOrderByCustomerID(currentCustomerID);
                        foreach (Order p in orderList)
                        {
                            Console.WriteLine(p.ToString());
                        }
                        Console.WriteLine();
                        Console.Write("Update Order?[Yes/No]: ");
                        string UpdateOrder = Console.ReadLine();
                        if (UpdateOrder == "yes" || UpdateOrder == "Yes" || UpdateOrder == "YES")
                        {
                            CustomerOrderStatus();
                        }
                        Console.Clear();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("You have successfully logged out of your account!");
                        loop = false;
                        MainMenu();
                        break;
                    case "4":
                        loop = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid Input");
                        break;
                } 
            }
        }

        // When called, it checks the parameter to see whether it contains symbols or numbers.
        static bool Validator(string dataValidation)
        {
            if (!Regex.IsMatch(dataValidation, @"^[\p{L}\p{M}' \.\-]+$"))
            {
                return false;
            }
            {
                return true;
            }
        }


        static void SignUpMenu()
        {
            bool loop = true; // Initialization variables within the method
            string validateUser, validateEmail;
            bool addCustomer;
            Customer validUsername, validEmail;   
            while (loop == true) // Will loop untill loop variable is false
            {  
                Customer c = new Customer();
                Console.WriteLine();
                Console.Write("Enter your name:  ");
                c.Name = Console.ReadLine();
                Console.Write("Enter your surname:  ");
                c.Surname = Console.ReadLine();
                Console.Write("Enter your email:  ");
                c.Email = Console.ReadLine();
                Console.Write("Enter your ID Card:  ");
                c.CustomerID = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter home address:  ");
                c.HomeAddress = Console.ReadLine();
                Console.Write("Enter mobile number:  ");
                c.MobileNumber = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter your new username:  ");
                c.Username = Console.ReadLine();
                Console.Write("Enter your new password:  ");
                c.Password = Console.ReadLine();
                Console.WriteLine();

                try // Try Catch is being used to catch InvalidOperationException (email already exists)
                {
                    // If the name or surname are left empty, an error is outputted.
                    if (Validator(c.Name) == false || Validator(c.Surname) == false)
                    {
                        Console.WriteLine();
                        Console.WriteLine("ERROR: Missing or irrelevant information, please re-input data");
                    }
                    else
                    {
                        validateUser = c.Username;
                        validateEmail = c.Email;
                        addCustomer = BuisnessManager.Instance.AddCustomer(c);
                        validUsername = BuisnessManager.Instance.GetCustomerByUsername(validateUser);
                        validEmail = BuisnessManager.Instance.GetCustomerByEmail(validateEmail);
                        // The customer ID, Username and email must be unique else an error will be outputted.
                        if (addCustomer && validateUser.Equals(validUsername) == false && validateEmail.Equals(validEmail) == false)
                        {
                            Console.Clear();
                            Console.WriteLine("You have successfully registered to the database.");
                            loop = false;
                            MainMenu(); // The while loop is stopped and the customer/employee is taken back to the main menu
                        }
                        else
                            Console.Clear();
                        Console.WriteLine("ERROR: A Customer with the indicated ID/USERNAME already exists");
                    }
                }
                catch (InvalidOperationException)
                {
                    Console.Clear();
                    Console.WriteLine("ERROR: A Customer with the indicated EMAIL already exists");
                }
            }
        }

        // When called, the method will show a log in menu
        static void LogInMenu()
        {
            bool loop = true;
            while (loop == true)
            {
                Console.WriteLine();
                Console.Write("Enter username: ");
                string typedUsername = Console.ReadLine();
                Console.Write("Enter password: ");
                string typedPassword = Console.ReadLine();
                Console.WriteLine();

                bool ValidCustomerLogin = BuisnessManager.Instance.ValidateCustomer(typedUsername, typedPassword);
                bool ValidEmployeeLogin = BuisnessManager.Instance.ValidateEmployee(typedUsername, typedPassword);
                if (ValidCustomerLogin)
                {
                    loop = false;
                    Console.Clear();
                    currentCustomerID = BuisnessManager.Instance.GetIdByUsername(typedUsername);
                    Console.WriteLine("You have successfully logged into your account!");
                    CustomerMenu();
                }
                else if (ValidEmployeeLogin)
                {
                    loop = false;
                    Console.Clear();
                    Console.WriteLine("You have successfully logged into your account!");
                    EmployeeMenu();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid username and/or password!");
                    Console.WriteLine();
                    Console.Write("Go back to main menu? [Yes/No]: ");
                    string back = Console.ReadLine();
                    if (back == "Yes" || back == "YES" || back == "yes")
                    {
                        MainMenu();
                    }
                    else if (back == "No" || back == "NO" || back == "no")
                    {
                        continue;
                    }
                    else { }
                }
            }
        }

        static void MainMenu()
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Store Login");
            Console.WriteLine();
            Console.WriteLine("{ 1 } Login");
            Console.WriteLine("{ 2 } Register ");
            Console.WriteLine("{ 3 } Exit");
            Console.WriteLine();
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    LogInMenu();
                    break;
                case "2":
                    SignUpMenu();
                    break;
                case "3":
                    Environment.Exit(1);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid Input");
                    MainMenu();
                    break;
            }
        }

        static void Main(string[] args)
        {
            MainMenu();
        }
    }
}
