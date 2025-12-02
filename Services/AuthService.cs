using CustomerCrudApi.Data;
using CustomerCrudApi.Dtos;
using CustomerCrudApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerCrudApi.Services
{
    public class AuthService : IAuthService
    {

        private readonly CustomerDbContext _context;

        public AuthService(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateLogin(CustomerLoginRequestDto loginRequestDto)
        {
            var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Email == loginRequestDto.Email);
            if(customer == null)
            {
                return false;
            }

            var passwordVerificationResult = new PasswordHasher<Customer>().VerifyHashedPassword(customer, customer.HashPassword!, loginRequestDto.Password!);
            if(passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return false;
            }

            return true;
        }

        public async Task<CustomerResponseDto> RegisterCustomerAsync(CustomerRequestDto requestDto)
        {

            var Customer = new Customer();
            var HashedPassword = new PasswordHasher<Customer>().HashPassword(Customer, requestDto.Password!);
            Customer.HashPassword = HashedPassword;
            Customer.FirstName = requestDto.FirstName;
            Customer.Email = requestDto.Email;
            Customer.Address = requestDto.Address;
            Customer.Role = requestDto.Role;

            _context.Customers.Add(Customer);
            await _context.SaveChangesAsync();

            return new CustomerResponseDto
            {
                Id = Customer.Id,
                Name = Customer.FirstName,
                Email = Customer.Email,
                Address = Customer.Address,
                Role = Customer.Role.ToString()
            };
        }
    }
}
