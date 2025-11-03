#pragma warning disable CS1591

namespace WebAPI.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase {
    private ISender _sender;
        
    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>();
}
#pragma warning restore CS1951