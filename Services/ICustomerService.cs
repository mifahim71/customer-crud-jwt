using CustomerCrudApi.Dtos;

namespace CustomerCrudApi.Services
{
    public interface ICustomerService
    {

        Task<List<CustomerResponseDto>> GetAllCustomersAsync();

        Task<CustomerResponseDto?> GetCustomerByEmailAsync(string email);

        Task<CustomerResponseDto?> UpdateCustomer(string email, CustomerUpdateRequestDto requestDto);

        Task<bool> DeleteCustomerAsync(string email);
    }
    
        

}
