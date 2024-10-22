using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRewardManagement
{
    public class FalconRewardStore
    {
        public void RedeemPoints(Employee employee, string item)
        {
            switch (item)
            {
                case "Walmart Gift Card":
                    if (employee.Points >= 500)
                    {
                        int amount = (employee.Points / 500) * 50;
                        employee.Points %= 500;
                        Console.WriteLine($"{employee.Name} redeemed A${amount} Walmart Gift Card");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient points");
                    }
                    break;

                case "Amazon Gift Card":
                    if (employee.Points >= 500)
                    {
                        int amount = (employee.Points / 500) * 50;
                        employee.Points %= 500;
                        Console.WriteLine($"{employee.Name} redeemed A${amount} Amazon Gift Card");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient points");
                    }
                    break;

                case "iPhone 14 Pro Max":
                    if (employee.Points >= 10000)
                    {
                        employee.Points -= 10000;
                        Console.WriteLine($"{employee.Name} redeemed an iPhone 14 Pro Max");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient points");
                    }
                    break;

                case "Professional Camera":
                    if (employee.Points >= 5000)
                    {
                        employee.Points -= 5000;
                        Console.WriteLine($"{employee.Name} redeemed a Professional Camera");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient points");
                    }
                    break;

                case "40 inch Television":
                    if (employee.Points >= 3000)
                    {
                        employee.Points -= 3000;
                        Console.WriteLine($"{employee.Name} redeemed a 40 inch Television");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient points");
                    }
                    break;

                case "Louis Vuitton Designer Handbag":
                    if (employee.Points >= 2000)
                    {
                        employee.Points -= 2000;
                        Console.WriteLine($"{employee.Name} redeemed a Louis Vuitton Designer Handbag");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient points");
                    }
                    break;

                default:
                    Console.WriteLine("Item not available in store");
                    break;
            }
        }
    }

}
