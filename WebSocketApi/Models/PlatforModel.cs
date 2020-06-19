namespace ChatApiApi.Models
{

    /// <summary>
    /// Author : Altay Ali
    /// 
    /// Platform model. Client send this data for register to WebSocketsStore
    /// </summary>
    public class PlatforModel
    {
        public string Email { get; set; }
        public string Platform { get; set; }
    }
}
