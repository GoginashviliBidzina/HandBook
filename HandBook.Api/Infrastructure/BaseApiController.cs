using Microsoft.AspNetCore.Mvc;
using HandBook.Application.Infrastructure;

namespace HandBook.Api.Infrastructure
{
    public class BaseApiController : ControllerBase
    {
        protected IActionResult CommandResult(CommandExecutionResult result, ResultStatusCode resultStatusCode)
        {
            if (result.Success)
                return resultStatusCode switch
                {
                    ResultStatusCode.Created => Created("", result.Data),
                    ResultStatusCode.Updated => Ok(result.Data),
                    ResultStatusCode.Deleted => NoContent(),
                    ResultStatusCode.Patched => Ok(result.Data),
                    _ => BadRequest(),
                };

            if (result.ErrorCode == ErrorCode.NotFound)
                return NotFound();

            return BadRequest(result.Errors);
        }

        protected IActionResult QueryResult<T>(QueryExecutionResult<T> result)
        {
            if (result.Success)
                return Ok(result.Data);

            if (result.ErrorCode == ErrorCode.NotFound)
                return NotFound();

            return BadRequest(result.ErrorCode.ToString());
        }
    }

    public enum ResultStatusCode
    {
        Created = 1,
        Updated = 2,
        Deleted = 3,
        Patched = 4
    }
}
