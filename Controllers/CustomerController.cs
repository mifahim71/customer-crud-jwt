using CustomerCrudApi.Dtos;
using CustomerCrudApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<CustomerResponseDto>> GetOneCustomer()
        {
            var email = User.Identity?.Name;
            if (email is null)
            {
                return Unauthorized();
            }

            var response =  await _customerService.GetCustomerByEmailAsync(email);
            if(response is null)
            {
                return NotFound();
            }
            
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<List<CustomerResponseDto>>> GetAllCustomers()
        {
            var response = await _customerService.GetAllCustomersAsync();
            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<CustomerResponseDto>> UpdateCustomer([FromBody] CustomerUpdateRequestDto requestDto)
        {
            
            if(ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            var email = User.Identity?.Name;
            if (email is null)
            {
                return Unauthorized();
            }

            var response = await _customerService.UpdateCustomer(email, requestDto);

            if(response is null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteCustomer()
        {
            var email = User.Identity?.Name;
            if (email is null)
            {
                return Unauthorized();
            }
            var isDeleted = await _customerService.DeleteCustomerAsync(email);
            if(!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
