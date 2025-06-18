using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CXTimeControl.Models._3CX
{
    public class NotificationRQ
    {
        public int id { get; set; }
        public int callid { get; set; }
        public string? status { get; set; }
        public bool isInbound { get; set; }
        public string? extension { get; set; }
        public string? extensionType { get; set; }
        public string? extensionName { get; set; }
        //public string? party_dn { get; set; }
        public string? destination { get; set; }
        public DateTimeOffset? date { get; set; }

        public List<ValueCallLog>? CDRs { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj is not NotificationRQ other)
                return false;

            return /*id == other.id &&*/
                   callid == other.callid &&
                   status == other.status &&
                   isInbound == other.isInbound &&
                   extension == other.extension &&
                   //party_dn == other.party_dn &&
                   destination == other.destination;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(callid, status, isInbound, extension, destination);
        }
    }
}
