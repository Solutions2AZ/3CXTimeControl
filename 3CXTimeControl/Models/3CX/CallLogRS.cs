using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CXTimeControl.Models._3CX
{
    

    public class CallLogRS
    {
        public string? odatacontext { get; set; }
        public ValueCallLog[]? value { get; set; }
    }

    public class ValueCallLog
    {
        public int CallId { get; set; }
        public int Indent { get; set; }
        public DateTime StartTime { get; set; }
        public int SourceType { get; set; }
        public string? SourceDn { get; set; }
        public string? SourceCallerId { get; set; }
        public string? SourceDisplayName { get; set; }
        public int DestinationType { get; set; }
        public string? DestinationDn { get; set; }
        public string? DestinationCallerId { get; set; }
        public string? DestinationDisplayName { get; set; }
        public int ActionType { get; set; }
        public int ActionDnType { get; set; }
        public string? actionDnDn { get; set; }
        public string? ActionDnCallerId { get; set; }
        public string? ActionDnDisplayName { get; set; }
        public string? RingingDuration { get; set; }
        public string? TalkingDuration { get; set; }
        public int CallCost { get; set; }
        public bool Answered { get; set; }
        public string? RecordingUrl { get; set; }
        public int SubrowDescNumber { get; set; }
        public string? Reason { get; set; }
        public int SrcRecId { get; set; }
        public int SegmentId { get; set; }
        public bool QualityReport { get; set; }
    }

}
