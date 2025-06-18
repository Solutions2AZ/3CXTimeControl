using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _3CXTimeControl.Models._3CX
{
    /// <summary>
    /// 
    /// </summary>
    public class WebSocketRS
    {
        public int sequence { get; set; }
        [JsonPropertyName("event")]
        public Event? _event { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Event
    {
        public int event_type { get; set; }
        public string? entity { get; set; }
        public object? attached_data { get; set; }
    }

}
