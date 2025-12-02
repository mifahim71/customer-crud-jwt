using CustomerCrudApi.Data;
using CustomerCrudApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CustomerCrudApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerDbContext _dbContext;

        public CustomerService(CustomerDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<CustomerResponseDto>> GetAllCustomersAsync()
        {
            return await _dbContext.Customers
                .Select(c => new CustomerResponseDto
                {
                    Id = c.Id,
                    Name = c.FirstName,
                    Email = c.Email,
                    Address = c.Address,
                    Role = c.Role.ToString()
                })
                .ToListAsync();
        }


        public async Task<CustomerResponseDto?> GetCustomerByEmailAsync(string email)
        {
            var Customer = await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Email == email);
            if (Customer == null)
            {
                return null;
            }

            return new CustomerResponseDto
            {
                Id = Customer.Id,
                Name = Customer.FirstName,
                Email = Customer.Email,
                Address = Customer.Address,
                Role = Customer.Role.ToString()
            };
        }

        public async Task<CustomerResponseDto?> UpdateCustomer(string email, CustomerUpdateRequestDto requestDto)
        {
            var customer = await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null)
            {
                return null;
            }

            customer.FirstName = requestDto.Name ?? customer.FirstName;
            customer.Address = requestDto.Address ?? customer.Address;
            customer.Email = requestDto.Email ?? customer.Email;

            await _dbContext.SaveChangesAsync();

            return new CustomerResponseDto()
            {
                Name = customer.FirstName,
                Email = customer.Email,
                Address = customer.Address,
                Id = customer.Id
            };

        }


        public async Task<bool> DeleteCustomerAsync(string email)
        {
            var Customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (Customer == null)
            {
                return false;
            }

            _dbContext.Customers.Remove(Customer);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
