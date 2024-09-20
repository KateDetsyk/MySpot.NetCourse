﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Application.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace MySpot.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ICommandHandler<SignUp> _signUpHandler;
        private readonly ICommandHandler<SignIn> _signInHandler;
        private readonly IQueryHandler<GetUser, UserDto> _getUserHandler;
        private readonly IQueryHandler<GetUsers, IEnumerable<UserDto>> _getUsersHandler;
        private readonly ITokenStorage _tokenStorage;

        public UsersController(ICommandHandler<SignUp> signUpHandler,
            ICommandHandler<SignIn> signInHandler,
            IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler,
            IQueryHandler<GetUser, UserDto> getUserHandler,
            ITokenStorage tokenStorage) 
        {
            _signUpHandler = signUpHandler;
            _signInHandler = signInHandler;
            _getUserHandler = getUserHandler;
            _getUsersHandler = getUsersHandler;
            _tokenStorage = tokenStorage;
        }

        [Authorize(Policy = "is-admin")]
        [HttpGet("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var user = await _getUserHandler.HandleAsync(new GetUser { UserId = id });
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> Get()
        {
            if (string.IsNullOrWhiteSpace(User.Identity?.Name))
            {
                return NotFound();
            }

            var userId = Guid.Parse(User.Identity?.Name);
            var user = await _getUserHandler.HandleAsync(new GetUser { UserId = userId });

            return user;
        }

        [HttpGet]
        [SwaggerOperation("Get list of all the users")]
        [Authorize(Policy = "is-admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] GetUsers query) 
            => Ok(await _getUsersHandler.HandleAsync(query));

        [HttpPost]
        [SwaggerOperation("Sing in the user and return the JSON Web Token")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(SignUp command)
        {
            command = command with { UserId = Guid.NewGuid() };
            await _signUpHandler.HandleAsync(command);
            return CreatedAtAction(nameof(Get), new { command.UserId }, null);
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<JwtDto>> Post(SignIn command)
        {
            await _signInHandler.HandleAsync(command);
            var jwt = _tokenStorage.Get();
            return jwt;
        }
    }
}
