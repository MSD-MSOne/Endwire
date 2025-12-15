using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.ConfigurationOptions
{
    public class StoredProcedure
    {
        public string Database { get; set; }
        public string Name { get; set; }
        public int Timeout { get; set; }
    }

    public class StoredProcedures
    {
        public StoredProcedure SendReminder { get; set; }
        public string FcmSendReminder { get; set; }
        public string GetAuthToken { get; set; }
        public string EndSession { get; set; }
        public string GetLocationList { get; set; }
        public string GetNudgeList { get; set; }
        public string GetResourceByNudgeId { get; set; }
        public string GetRoleAuthorities { get; set; }
        public string GetInboxList { get; set; }
        public string GetUserOnlineStatus { get; set; }
        public string GetUserProfile { get; set; }
        public string GetUserRoles { get; set; }
        public string GetUserLocations { get; set; }
        public string GetOutboxList { get; set; }
        public string GetConfirmBy { get; set; }
        public string GetReminderRecepients { get; set; }
        public string GetUserListByNudgeId { get; set; }
        public string GetGroupUsersList { get; set; }
        public string GetGroups { get; set; }
        public string SetTimerTask { get; set;}
        public string GetTimerTaskList { get; set;}
        public string UpdateNudgeStatus { get;set; }
        public string GetStartCall { get; set; }
        public string EndCall { get; set; }
        public string ExitCall { get; set; }
        public string AcceptCall { get; set; }
        public string GetStartBroadcast { get; set;}
        public string GetEndBroadcast { get; set; }
        public string NudgeStatusChange_FCM { get; set;}
        public string AddNewResource { get; set; }

        public string GetUserNudgeStatus { get; set; }

        public string GetExitCall_FCM { get; set; }
    }
}
