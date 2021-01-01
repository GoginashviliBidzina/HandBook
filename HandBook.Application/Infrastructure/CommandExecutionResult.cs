using System.Collections.Generic;

namespace HandBook.Application.Infrastructure
{
    public class CommandExecutionResult : ExecutionResult
    {
        public IEnumerable<string> Errors { get; set; }
    }
}