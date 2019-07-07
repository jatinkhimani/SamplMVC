using AppCustomer.DataAccess;
using RepositoryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustomer.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CustomerDBEntities context) : base(context)
        {
        }

        public void AddCustomer(Customer customer)
        {
            Add(customer);
        }

        public void DeleteCustomer(int customerId)
        {
            Delete(customerId);
        }

        public void EditCustomer(Customer customer)
        {
            Update(customer);
        }

        public List<Customer> FindAllCustomer()
        {
            return SelectQuery(null,null,f=>f.City).ToList();
        }

        public Customer FindCustomer(int customerId)
        {
            return SelectQuery(f => f.CustomerId == customerId,null,f=>f.City).FirstOrDefault();
        }
    }
}
