#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsMobile
{
    internal static class NativeMethods
    {
        internal const int QUERYESCSUPPORT = 8;

        internal const int GETVFRAMEPHYSICAL = 6144;
        internal const int GETVFRAMELEN = 6145;
        internal const int DBGDRIVERSTAT = 6146;
        internal const int SETPOWERMANAGEMENT = 6147;
        internal const int GETPOWERMANAGEMENT = 6148;

        internal enum VideoPowerState
        {
            On = 1,
            StandBy,
            Suspend,
            Off
        }

         /*
          this is an example of how to turn the PPC screen on or off
       internal struct VideoPowerManagementInfo
        {
            public uint length;
            public uint DPMSVersion;
            public VideoPowerState powerState;
        }


         
        HDC gdc;
        int iESC=SETPOWERMANAGEMENT;
 
        gdc = ::GetDC(NULL);
        if (ExtEscape(gdc, QUERYESCSUPPORT, sizeof(int), (LPCSTR)&iESC, 
                       0, NULL)==0)           
                 MessageBox(NULL,
                  L"Sorry, your Pocket PC does not support DisplayOff",
                  L"Pocket PC Display Off Feature",
                  MB_OK);
        else
        {
               VIDEO_POWER_MANAGEMENT vpm;
               vpm.Length = sizeof(VIDEO_POWER_MANAGEMENT);
               vpm.DPMSVersion = 0x0001;
               vpm.PowerState = VideoPowerOff;
// Power off the display
               ExtEscape(gdc, SETPOWERMANAGEMENT, vpm.Length, (LPCSTR) &vpm, 
                             0, NULL);
               Sleep(5000);
               vpm.PowerState = VideoPowerOn;
// Power on the display
               ExtEscape(gdc, SETPOWERMANAGEMENT, vpm.Length, (LPCSTR) &vpm, 
                             0, NULL);
               ::ReleaseDC(NULL, gdc);
        }
        return 0;
         */


        //Vibrate API

        [DllImport("aygshell.dll", EntryPoint = "Vibrate", SetLastError = true)]
        internal static extern int VibratePlay(
            int cvn,
            IntPtr rgvn,
            uint fRepeat,
            uint dwTimeout);

        [DllImport("aygshell.dll", SetLastError = true)]
        internal static extern int VibrateStop();

        [DllImport("aygshell.dll", SetLastError = true)]
        internal static extern int VibrateGetDeviceCaps(
            Vibrate.VibrationCapabilities caps);

    }
}
