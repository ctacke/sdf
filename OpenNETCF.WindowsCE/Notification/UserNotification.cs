using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsCE.Notification
{
    /// <summary>
    /// Contains information used for a user notification.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class UserNotification
    {
        int ActionFlags;
        [MarshalAs(UnmanagedType.LPWStr)]
        string pwszDialogTitle;
        [MarshalAs(UnmanagedType.LPWStr)]
        string pwszDialogText;
        [MarshalAs(UnmanagedType.LPWStr)]
        string pwszSound;
        int nMaxSound;
        int dwReserved;

        /// <summary>
        /// Create a new instance of the UserNotification class
        /// </summary>
        public UserNotification()
        {
        }

        #region Action
        /// <summary>
        /// Any combination of the <see cref="NotificationAction"/> members.  
        /// </summary>
        /// <value>Flags which specifies the action(s) to be taken when the notification is triggered.</value>
        /// <remarks>Flags not valid on a given hardware platform will be ignored.</remarks>
        public NotificationAction Action
        {
            get
            {
                return (NotificationAction)ActionFlags;
            }
            set
            {
                ActionFlags = (int)value;
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Required if NotificationAction.Dialog is set, ignored otherwise
        /// </summary>
        public string Title
        {
            get
            {
                return pwszDialogTitle;
            }
            set
            {
                pwszDialogTitle = value;
            }
        }
        #endregion

        #region Text
        /// <summary>
        /// Required if NotificationAction.Dialog is set, ignored otherwise.
        /// </summary>
        public string Text
        {
            get
            {
                return pwszDialogText;
            }
            set
            {
                pwszDialogText = value;
            }
        }
        #endregion

        #region Sound
        /// <summary>
        /// Sound string as supplied to PlaySound.
        /// </summary>
        public string Sound
        {
            get
            {
                return pwszSound;
            }
            set
            {
                pwszSound = value;
            }
        }

        internal int MaxSound
        {
            get
            {
                return nMaxSound;
            }
            set
            {
                nMaxSound = value;
            }
        }
        #endregion
    }
}
