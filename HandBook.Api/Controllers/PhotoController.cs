using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HandBook.Api.Infrastructure;
using HandBook.Application.Commands.Photo;
using HandBook.Application.Infrastructure;

namespace HandBook.Api.Controllers
{
    [Route("v1/photo")]
    [ApiController]
    public class PhotoController : BaseApiController
    {
        private readonly CommandExecutor _commandExecutor;

        public PhotoController(CommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] PlacePhotoCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            return CommandResult(result, ResultStatusCode.Created);
        }
    }
}
