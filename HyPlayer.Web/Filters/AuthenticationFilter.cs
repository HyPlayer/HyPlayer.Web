using System.Security.Cryptography;
using System.Text;
using HyPlayer.Web.Interfaces;

namespace HyPlayer.Web.Filters;

public class AuthenticationFilter(IAdminRepository adminRepository) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Query.ContainsKey("admin") ||
            !context.HttpContext.Request.Query.ContainsKey("password"))
            return Results.BadRequest("参数不全");
        if (await adminRepository.GetAdministratorAsync(context.HttpContext.Request.Query["admin"]!) is not
            { } admin)
            return Results.Problem("管理账户登入失败", statusCode: 403);
        var computedPasswordHash = string.Join("",
            SHA256.HashData(Encoding.UTF8.GetBytes(context.HttpContext.Request.Query["password"]!))
                .Select(t => t.ToString("x2")));
        if (admin.Password != computedPasswordHash)
            return Results.Problem("管理账户登入失败", statusCode: 403);
        return await next(context);
    }
}