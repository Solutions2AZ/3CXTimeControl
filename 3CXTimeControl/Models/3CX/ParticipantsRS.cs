using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CXTimeControl.Models._3CX
{


    public class ParticipantsRS
    {
        public int id { get; set; }
        public string? status { get; set; }
        public string? dn { get; set; }
        public string? party_caller_name { get; set; }
        public string? party_dn { get; set; }
        public string? party_caller_id { get; set; }
        public string? party_did { get; set; }
        public string? device_id { get; set; }
        public string? party_dn_type { get; set; }
        public bool direct_control { get; set; }
        public string? originated_by_dn { get; set; }
        public string? originated_by_type { get; set; }
        public string? referred_by_dn { get; set; }
        public string? referred_by_type { get; set; }
        public string? on_behalf_of_dn { get; set; }
        public string? on_behalf_of_type { get; set; }
        public int callid { get; set; }
        public int legid { get; set; }

        public NotificationRQ toNotification()
        {
            return new NotificationRQ()
            {
                id = id,
                callid = callid,
                //isInbound = originated_by_dn != null && originated_by_dn == "ROUTER",
                isInbound = party_caller_name == party_caller_id,
                //isInbound = party_dn_type != null && party_dn_type == "Wexternalline",
                extension = dn,
                status = status!.ToLower(),
                date = new DateTimeOffset(DateTime.UtcNow),
                destination = party_caller_id
            };
        }
    }



}
