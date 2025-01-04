using Data.Enums;
using Repositories.AccountRepos;
using Services.Helper.CustomExceptions;
using System.Net;
using System.Security.Claims;

namespace ConstructionEquipmentRental.API.Middlewares
{
    public class AuthorizeMiddleware
    {

        private readonly RequestDelegate _next;

        public AuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAccountRepository accountRepository)
        {
            try
            {
                var requestPath = context.Request.Path;

                if (requestPath.StartsWithSegments("/api/auth"))
                {
                    await _next.Invoke(context);
                    return;
                }

                var accIdentity = context.User.Identity as ClaimsIdentity;
                if (!accIdentity.IsAuthenticated)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                var account = await accountRepository.GetAccountById(int.Parse(accIdentity.FindFirst("id").Value));



                if (account != null)
                {
                    if (account.Status.Equals(AccountStatusEnum.UNVERIFIED.ToString()))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return;
                    }
                }
               
                else
                {
                    throw new ApiException(HttpStatusCode.NotFound, "Account not found");
                }

                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(ex.ToString());
            }

        }

    }
}
