using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;

namespace MySpot.Api
{
    public static class UsersApi
    {
        public static WebApplication UseUsersApi(this WebApplication app)
        {
            const string MeRoute = "me";

            app.MapGet("api/users/me", async (HttpContext context, IQueryHandler<GetUser, UserDto> handler) =>
            {
                var userDto = await handler.HandleAsync(new GetUser { UserId = Guid.Parse(context.User.Identity?.Name) });

                return Results.Ok(userDto);
            }).RequireAuthorization().WithName(MeRoute);

            app.MapGet("api/users/{userId:guid}", async (Guid userId, IQueryHandler<GetUser, UserDto> handler) =>
            {
                var userDto = await handler.HandleAsync(new GetUser { UserId = userId });

                if (userDto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(userDto);
            }).RequireAuthorization("is-admin");

            app.MapPost("api/users", async (SignUp command, ICommandHandler<SignUp> handler) =>
            {
                command = command with { UserId = Guid.NewGuid() };
                await handler.HandleAsync(command);

                return Results.CreatedAtRoute(MeRoute);
            });

            return app;
        }
    }
}
