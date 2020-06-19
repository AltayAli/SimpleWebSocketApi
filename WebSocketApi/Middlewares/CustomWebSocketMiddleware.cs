using ChatApi.Utils;
using ChatApiApi.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApi.Middlewares
{

    /// <summary>
    /// Author : Altay Ali
    /// 
    /// 
    /// Chat middleWare for get connection othet devices.
    /// 
    /// </summary>
    public class CustomWebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketsStore _store;

        /// <summary>
        /// Middle ware constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="store"></param>
        public CustomWebSocketMiddleware(RequestDelegate next,
                                            WebSocketsStore store)
        {
           _next = next;
           _store = store;
        }

        /// <summary>
        /// Middleware invoke method. Call from IApplicationBuilder help with Reflection
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();
                    byte[] buffer = new byte[100];
                    WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    var data = Encoding.UTF8.GetString(buffer);
                    var model = JsonConvert.DeserializeObject<PlatforModel>(data);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        while(!result.CloseStatus.HasValue)
                        {
                            _store.Add(model.Email + "-" + model.Platform, socket);
                            await SendMessageToClients(model);
                            result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        }
                    }else if(result.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                        _store.Remove(model.Email + "-" + model.Platform,socket);
                    }                 
                }
            }
            else
            {
               await _next(context);
            }
        }

        /// <summary>
        /// Send message to devices         
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task SendMessageToClients(PlatforModel model)
        {
            var key = model.Email + "-" + model.Platform;
            foreach (var socket in _store.GetSockets().Where(x=>x.Key.Contains(model.Email)))
            {
               if(socket.Value.State == WebSocketState.Open|| socket.Value.State==WebSocketState.CloseReceived)
                {
                    try
                    {
                        var otherKey = socket.Key;
                        var bytes = Encoding.UTF8.GetBytes(key == otherKey ? "1" : "0");
                        await socket.Value.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch
                    {
                        _store.Remove(model.Email + "-" + model.Platform, socket.Value);
                    }
                }
            }
        }
    }
}
