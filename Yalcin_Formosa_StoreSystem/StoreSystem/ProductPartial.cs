using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSystem
{
    public partial class Product
    {
        // Returns values from the product table
        public override string ToString()
        {
            return string.Format("\nID: {0} \nName: {1} \nCategory: {2} \nPrice: ${3} \nStock: {4}",
                ProductID, Name, Category, decimal.Round(Price, 2, MidpointRounding.AwayFromZero), Stock); 
                // The price is rounded from 2 places at the decimal point
        }
    }
}
