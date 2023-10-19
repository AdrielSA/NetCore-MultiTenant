using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.Application.DTOs;
using MultiTenant.Domain.Contracts.IServices;
using MultiTenant.Domain.Entities;

namespace MultiTenant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost($"/Login")]
        public IActionResult Login([FromBody]LoginDto dto)
        {
            var token = _userService.AuthenticateUser(dto.Email, dto.Password);
            return !string.IsNullOrEmpty(token) ? Ok(new { Token = token }) : Unauthorized(dto);
        }

        [HttpPost($"/Register")]
        public IActionResult RegisterUser([FromBody]UserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            var slugTenant = _userService.AddUser(user);
            return Ok(slugTenant);
        }
    }
}
