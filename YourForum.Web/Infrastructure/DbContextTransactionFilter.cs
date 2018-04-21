using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using YourForum.Web.Data;

namespace YourForum.Web.Infrastructure
{
    public class DbContextTransactionFilter : IAsyncActionFilter
    {
        private readonly YourForumContext _dbContext;

        public DbContextTransactionFilter(YourForumContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                await _dbContext.BeginTransactionAsync();

                var actionExecuted = await next();
                if (actionExecuted.Exception != null && !actionExecuted.ExceptionHandled)
                {
                    _dbContext.RollbackTransaction();

                }
                else
                {
                    await _dbContext.CommitTransactionAsync();

                }
            }
            catch (Exception)
            {
                _dbContext.RollbackTransaction();
                throw;
            }
        }
    }
}
