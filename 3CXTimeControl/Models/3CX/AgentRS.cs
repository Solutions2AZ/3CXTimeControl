using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CXTimeControl.Models._3CX
{
    

    public class AgentRS
    {
        public string? odatacontext { get; set; }
        public ValueAgentRS[]? value { get; set; }
    }

    public class ValueAgentRS
    {
        //public bool Enable2FA { get; set; }
        //public bool Require2FA { get; set; }
        //public bool MyPhoneShowRecordings { get; set; }
        //public bool MyPhoneAllowDeleteRecordings { get; set; }
        //public bool MyPhoneHideForwardings { get; set; }
        //public bool MyPhonePush { get; set; }
        //public bool RecordCalls { get; set; }
        //public bool AllowOwnRecordings { get; set; }
        //public bool RecordExternalCallsOnly { get; set; }
        //public int PrimaryGroupId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DisplayName { get; set; }
        public string? EmailAddress { get; set; }
        //public string Mobile { get; set; }
        //public string WebMeetingFriendlyName { get; set; }
        //public bool WebMeetingApproveParticipants { get; set; }
        //public bool SendEmailMissedCalls { get; set; }
        //public bool VMEnabled { get; set; }
        //public string VMEmailOptions { get; set; }
        //public string VMPIN { get; set; }
        //public bool VMDisablePinAuth { get; set; }
        //public bool VMPlayCallerID { get; set; }
        //public string VMPlayMsgDateTime { get; set; }
        //public string ContactImage { get; set; }
        //public bool IsRegistered { get; set; }
        //public string CurrentProfileName { get; set; }
        //public string QueueStatus { get; set; }
        //public string AuthID { get; set; }
        //public string DeskphonePassword { get; set; }
        //public string AuthPassword { get; set; }
        //public string OutboundCallerID { get; set; }
        //public string Blfs { get; set; }
        //public object[] OfficeHoursProps { get; set; }
        //public string Language { get; set; }
        //public bool Internal { get; set; }
        //public bool Enabled { get; set; }
        //public string PromptSet { get; set; }
        //public bool HideInPhonebook { get; set; }
        //public object[] Tags { get; set; }
        //public bool PinProtected { get; set; }
        //public int PinProtectTimeout { get; set; }
        //public bool CallScreening { get; set; }
        //public bool AllowLanOnly { get; set; }
        //public string SIPID { get; set; }
        //public bool MS365SignInEnabled { get; set; }
        //public bool MS365ContactsEnabled { get; set; }
        //public bool MS365CalendarEnabled { get; set; }
        //public bool MS365TeamsEnabled { get; set; }
        //public bool GoogleSignInEnabled { get; set; }
        //public bool EnableHotdesking { get; set; }
        //public string SRTPMode { get; set; }
        //public bool PbxDeliversAudio { get; set; }
        //public string EmergencyLocationId { get; set; }
        //public string EmergencyAdditionalInfo { get; set; }
        //public string CallUsRequirement { get; set; }
        //public bool CallUsEnablePhone { get; set; }
        //public bool CallUsEnableChat { get; set; }
        //public bool CallUsEnableVideo { get; set; }
        //public string ClickToCallId { get; set; }
        //public string Number { get; set; }
        //public string TranscriptionMode { get; set; }
        //public int Id { get; set; }
        //public Hours Hours { get; set; }
        //public Breaktime BreakTime { get; set; }
    }

    //public class Hours
    //{
    //    public string Type { get; set; }
    //    public bool IgnoreHolidays { get; set; }
    //    public object[] Periods { get; set; }
    //}

    //public class Breaktime
    //{
    //    public string Type { get; set; }
    //    public bool IgnoreHolidays { get; set; }
    //    public object[] Periods { get; set; }
    //}

}
