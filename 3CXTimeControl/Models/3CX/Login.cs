using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CXTimeControl.Models._3CX
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginRS
    {
        public string? token_type { get; set; }
        public int expires_in { get; set; }
        public string? access_token { get; set; }
        public object? refresh_token { get; set; }
    }

}
