using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.Mail
{
    /// <summary>
    /// Represents the method that will handle the SendCompleted event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SendCompletedEventHandler(object sender, AsyncCompletedEventArgs e);
}
