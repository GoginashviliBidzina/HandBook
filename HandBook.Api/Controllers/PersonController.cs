using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using HandBook.Api.Infrastructure;
using HandBook.Application.Queries.Person;
using HandBook.Application.Infrastructure;
using HandBook.Application.Commands.Person;

namespace HandBook.Api.Controllers
{
    [Route("v1/person")]
    [ApiController]
    public class PersonController : BaseApiController
    {
        private readonly QueryExecutor _queryExecutor;
        private readonly CommandExecutor _commandExecutor;

        public PersonController(QueryExecutor queryExecutor,
                                CommandExecutor commandExecutor)
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] PlacePersonCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            return CommandResult(result, ResultStatusCode.Created);
        }

        [HttpPatch]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdatePersonCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            return CommandResult(result, ResultStatusCode.Patched);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromQuery] DeletePersonCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            return CommandResult(result, ResultStatusCode.Created);
        }

        [HttpGet]
        [Route("details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Details([FromQuery] PersonDetailsQuery query)
        {
            var result = await _queryExecutor.ExecuteAsync<PersonDetailsQuery, PersonDetailsQueryResult>(query);

            return QueryResult(result);
        }

        [HttpGet]
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] PersonSearchQuery query)
        {
            var result = await _queryExecutor.ExecuteAsync<PersonSearchQuery, PersonSearchQueryResult>(query);

            return QueryResult(result);
        }

        [HttpGet]
        [Route("detailedSearch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DetailedSearch([FromQuery] PersonDetailedSearchQuery query)
        {
            var result = await _queryExecutor.ExecuteAsync<PersonDetailedSearchQuery, PersonDetailedSearchQueryResult>(query);

            return QueryResult(result);
        }

        [HttpGet]
        [Route("report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Report([FromQuery] PersonReportQuery query)
        {
            var result = await _queryExecutor.ExecuteAsync<PersonReportQuery, IEnumerable<PersonReportQueryResult>>(query);

            return QueryResult(result);
        }
    }
}
