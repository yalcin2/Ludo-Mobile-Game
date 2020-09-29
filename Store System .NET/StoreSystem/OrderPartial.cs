using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSystem
{
    public partial class Order
    {
        // Returns values from the orders table
        public override string ToString()
        {
            return string.Format("\nOrder ID: {0} \nCustomer ID: {1} \nProduct ID: {2} \nStatus: {3} \nQuantity: {4}",
                OrderID, CustomerID, ProductID, Status, Quantity);
        }
    }
}
