using CustomerCrudApi.Dtos;

namespace CustomerCrudApi.Services
{
    public interface IAuthService
    {
        Task<CustomerResponseDto> RegisterCustomerAsync(CustomerRequestDto requestDto);
        
        Task<bool> ValidateLogin(CustomerLoginRequestDto loginRequestDto);
    }
}
