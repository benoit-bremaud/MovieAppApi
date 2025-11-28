using Microsoft.AspNetCore.Mvc;

namespace MovieAppApi.Src.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<T> : Controller
{
    protected readonly ILogger<T> Logger;

    protected BaseController(ILogger<T> logger)
    {
        Logger = logger;
    }
}
