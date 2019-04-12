using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RCM.Mobile.Helpers
{
    public static class Constant
    {
        public static string Login = "Login";

        public const string DATE_FORMAT = "yyyyMMdd";

        public const string TIME_FORMAT = "HHmm";
        #region Notification

        public const string NOTIFICATION_TYPE_NEW_RECEIVABLE = "New Receivable";
        public const int NOTIFICATION_TYPE_NEW_RECEIVABLE_CODE = 11;

        public const string NOTIFICATION_TYPE_ASSIGN_RECEIVABLE = "Assign Receivable";
        public const int NOTIFICATION_TYPE_ASSIGN_RECEIVABLE_CODE = 13;
        #endregion

        #region Debtor

        public const string CONTACT_DEBTOR = "Debtor";
        public const int CONTACT_DEBTOR_CODE = 0;

        public const string CONTACT_RELATION = "Relation";
        public const int CONTACT_RELATION_CODE = 1;
        #endregion

        #region Action

        public const string ACTION_SMS = "SMS";
        public const int ACTION_SMS_CODE = 0;

        public const string ACTION_PHONECALL = "PhoneCall";
        public const int ACTION_PHONECALL_CODE = 1;

        public const string ACTION_NOTIFICATION = "Notification";
        public const int ACTION_NOTIFICATION_CODE = 3;

        public const string ACTION_VISIT = "Visit";
        public const int ACTION_VISIT_CODE = 2;

        public const string ACTION_REPORT = "Report";
        public const int ACTION_REPORT_CODE = 4;

        #endregion

        #region CollectionStatus

        //Action is executed but response result is bad.
        //Collection progress is closed and not collected.
        public const string COLLECTION_STATUS_CANCEL = "Cancel";
        public const int COLLECTION_STATUS_CANCEL_CODE = 0;

        //Action is in order to be executed.
        //Stage is in order to be executed.
        //Collection Progress is in order to be collected.
        public const string COLLECTION_STATUS_COLLECTION = "Collection";
        public const int COLLECTION_STATUS_COLLECTION_CODE = 1;

        //Action is executed.
        //Stage's actions are all executed.
        //All stages of Collection Progress are executed.
        public const string COLLECTION_STATUS_DONE = "Done";
        public const int COLLECTION_STATUS_DONE_CODE = 2;

        //Action is not executed on time.
        public const string COLLECTION_STATUS_LATE = "Late";
        public const int COLLECTION_STATUS_LATE_CODE = 3;

        //Collection progress is still pending for assgined.
        public const string COLLECTION_STATUS_WAIT = "Wait";
        public const int COLLECTION_STATUS_WAIT_CODE = 4;

        //Collection progress is closed, debt has been recovered.
        public const string COLLECTION_STATUS_CLOSED = "Closed";
        public const int COLLECTION_STATUS_CLOSED_CODE = 5;

        public static List<Status> STATUSES = new List<Status>()
        {
            new Status(Color.FromHex("#21ba45"), COLLECTION_STATUS_COLLECTION),
            new Status(Color.FromHex("#db2828"), COLLECTION_STATUS_CANCEL),
            new Status(Color.FromHex("#767676"), COLLECTION_STATUS_CLOSED),
            new Status(Color.FromHex("#2185d0"), COLLECTION_STATUS_DONE),
        };
        #endregion

    }
    public class Status
    {
        public Status(Color color, string name)
        {
            this.Color = color;
            this.Name = name;
        }

        public Color Color { get; set; }
        public string Name { get; set; }

    }
}
