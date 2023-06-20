public class NotFoundRedirectMiddleware
{
    private readonly RequestDelegate _next;

    public NotFoundRedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
        {
            context.Response.Redirect("/Home/NotFound");
        }
    }
}
