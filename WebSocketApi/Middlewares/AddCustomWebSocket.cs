using Microsoft.AspNetCore.Builder;

namespace ChatApi.Middlewares
{
    public static class AddCustomWebSocket
    {
        public static IApplicationBuilder AddWebSocketMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomWebSocketMiddleware>();
        }
    }
}
