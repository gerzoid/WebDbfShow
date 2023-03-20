namespace WebDBFShow.Extensions
{
    public class BlockSubdomainsMiddleware
    {
        private readonly RequestDelegate _next;

        public BlockSubdomainsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Host.Value.Split('.').Length > 2)
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Subdomains are not allowed");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
