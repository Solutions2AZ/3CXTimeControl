using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CXTimeControl.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Config3cxSettings
    {
        public string? Domain { get; set; }
        public string? ClientId { get; set; }
        public string? Secret { get; set; }
        public string? CFDEndCall { get; set; }
        public string? CFDInitialNoTime { get; set; }
        public int MinExtension { get; set; }
        public int MaxExtension { get; set; }
    }

}
