using Microsoft.AspNetCore.Diagnostics;

namespace SocialNetworkAnalyser.ExceptionHandler
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            return await ValueTask.FromResult(false);
        }
    }
}
