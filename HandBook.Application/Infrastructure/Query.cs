﻿using System.Threading.Tasks;

namespace HandBook.Application.Infrastructure
{
    public abstract class Query<TQueryResult> : ApplicationBase
        where TQueryResult : class
    {
        public abstract Task<QueryExecutionResult<TQueryResult>> ExecuteAsync();

        protected async Task<QueryExecutionResult<TQueryResult>> FailAsync(ErrorCode errorCode)
        {
            var result = new QueryExecutionResult<TQueryResult>
            {
                ErrorCode = errorCode,
                Success = false,
            };

            return await Task.FromResult(result);
        }

        protected async Task<QueryExecutionResult<TQueryResult>> OkAsync(TQueryResult data)
        {
            var result = new QueryExecutionResult<TQueryResult>
            {
                Data = data,
                Success = true
            };

            return await Task.FromResult(result);
        }
    }
}
