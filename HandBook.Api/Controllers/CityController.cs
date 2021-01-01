using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HandBook.Api.Infrastructure;
using HandBook.Application.Commands.City;
using HandBook.Application.Infrastructure;

namespace HandBook.Api.Controllers
{
    [Route("v1/city")]
    [ApiController]
    public class CityController : BaseApiController
    {
        private readonly CommandExecutor _commandExecutor;

        public CityController(CommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] PlaceCityCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            return CommandResult(result, ResultStatusCode.Created);
        }
    }
}
