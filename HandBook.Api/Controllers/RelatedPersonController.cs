using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HandBook.Api.Infrastructure;
using HandBook.Application.Infrastructure;
using HandBook.Application.Commands.Person;

namespace HandBook.Api.Controllers
{
    [Route("v1/relatedPerson")]
    [ApiController]
    public class RelatedPersonController : BaseApiController
    {
        private readonly CommandExecutor _commandExecutor;

        public RelatedPersonController(CommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] PlaceRelatedPersonCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            return CommandResult(result, ResultStatusCode.Created);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromBody] DeleteRelatedPersonCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            return CommandResult(result, ResultStatusCode.Deleted);
        }
    }
}
