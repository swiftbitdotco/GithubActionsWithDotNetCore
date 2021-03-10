using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Some.Contracts;

namespace GithubActions.Web.Server.Controllers
{
    /// <summary>
    /// A base class for any controller; has url-versioned routing built in.
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}