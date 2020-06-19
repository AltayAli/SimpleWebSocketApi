using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace ChatApi.Utils
{
    /// <summary>
    /// Author : Altay Ali
    /// 
    /// All Sockets store in  collection in this class
    /// 
    /// Methods : Add, Remove, GetSockets
    /// </summary>
    public class WebSocketsStore
    {
        private  ConcurrentDictionary<string, WebSocket> sockets ;

        public WebSocketsStore()
        {
            sockets = new ConcurrentDictionary<string, WebSocket>();
        }

        /// <summary>
        /// Add socket to store if not in store. Update socket  if socket that known by given key is in store and received 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="socket"></param>
        public  void Add(string key,WebSocket socket)
        {
            if (!sockets.ContainsKey(key))
            {
                sockets.TryAdd(key, socket);
            }
            else
            {
                var oldSocket = sockets.GetOrAdd(key, socket);
                if (oldSocket.State == WebSocketState.CloseReceived)
                    sockets.TryUpdate(key, socket, oldSocket);
            }
        }

        /// <summary>
        /// Get off socket from store by given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="socket"></param>
        public void Remove(string key, WebSocket socket)
        {
            if (sockets.ContainsKey(key))
                sockets.TryRemove(key,out socket);

        }

        /// <summary>
        /// Get all socekts
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<string, WebSocket> GetSockets()
            => sockets;
         
        

    }
}
