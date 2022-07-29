﻿using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using HyPlayer.Web.Infrastructure.Interfaces;
using HyPlayer.Web.Infrastructure.Models;
using HyPlayer.Web.Infrastructure.Models.Packages.ResponsePackages;
using Microsoft.AspNetCore.Authentication;

namespace HyPlayer.Web.Filters;

public class AuthenticationFilter : IRouteHandlerFilter
{
    private readonly IAdminRepository _adminRepository;

    public AuthenticationFilter(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public async ValueTask<object?> InvokeAsync(RouteHandlerInvocationContext context, RouteHandlerFilterDelegate next)
    {
        if (!context.HttpContext.Request.Query.ContainsKey("admin") ||
            !context.HttpContext.Request.Query.ContainsKey("password"))
            return Results.BadRequest("Parameters Not Filled");
        if (await _adminRepository.GetAdministratorAsync(context.HttpContext.Request.Query["admin"]!) is not
            { } admin)
            return Results.Problem("Admin Not Found", statusCode: 403);
        var computedPasswordHash = string.Join("",
            SHA256.HashData(Encoding.UTF8.GetBytes(context.HttpContext.Request.Query["password"]!))
                .Select(t => t.ToString("x2")));
        if (admin.Password != computedPasswordHash)
            return Results.Problem("Password Error", statusCode: 403);
        return await next(context);
    }
}