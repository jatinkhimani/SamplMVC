using AppCustomer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustomer.Repository
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customer);
        void EditCustomer(Customer customer);
        void DeleteCustomer(int customerId);
        Customer FindCustomer(int customerId);
        List<Customer> FindAllCustomer();
    }
}
