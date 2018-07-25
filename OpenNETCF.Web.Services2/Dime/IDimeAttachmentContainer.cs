
using System;


namespace OpenNETCF.Web.Services2.Dime {
    /// <summary>
    /// IDimeAttachmentContainer is an interface used by the DimeExtension to
    /// get and set unreferenced DIME attachments. This interface can be implemented
    /// on the service or proxy to add support for unreferenced attachments.
    /// </summary>
    public interface IDimeAttachmentContainer {
        DimeAttachmentCollection RequestAttachments { get; }
        DimeAttachmentCollection ResponseAttachments { get; }
    }
}
