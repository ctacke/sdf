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
//using OpenNETCF.Win32;

namespace OpenNETCF.WindowsCE
{
    internal static class NativeMethods
    {
        public const int ERROR_INSUFFICIENT_BUFFER = 0x7A;
        public const int MAX_PATH = 260;
        public const int ERROR_ALREADY_EXISTS = 183;
        public const int MSGQUEUE_MSGALERT = 0x00000001;
        public const int ERROR_PIPE_NOT_CONNECTED = 233;
        public const int MSGQUEUE_NOPRECOMMIT = 0x00000001;
        public const int MSGQUEUE_ALLOW_BROKEN = 0x00000002;
        public const int ERROR_OUTOFMEMORY = 14;
        public const int ERROR_TIMEOUT = 1460;
        public const int WAIT_TIMEOUT = 258;
        public const int WAIT_OBJECT_0 = 0;
        public const int INVALID_HANDLE_ERROR = 6;

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int WaitForMultipleObjects(int count, IntPtr[] arrHandle, int fWaitll, int dwMilliseconds);

        public enum EventFlags
        {
            EVENT_PULSE = 1,
            EVENT_RESET = 2,
            EVENT_SET = 3
        }

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int EventModify(IntPtr hEvent, int function);
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern IntPtr CreateEvent(int reserved, bool bManualReset, bool bInitialState, string lpName);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool CloseMsgQueue(IntPtr hMsgQ);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern IntPtr CreateMsgQueue(string lpszName, Messaging.MsgQueueOptions lpOptions);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool GetMsgQueueInfo(IntPtr hMsgQ, ref MsgQueueInfo lpInfo);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern IntPtr OpenMsgQueue(IntPtr hSrcProc, IntPtr hMsgQ, Messaging.MsgQueueOptions lpOptions);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool ReadMsgQueue(IntPtr hMsgQ, byte[] lpBuffer, Int32 cbBufferSize, out Int32 lpNumberOfBytesRead, Int32 dwTimeout, out Int32 pdwFlags);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool WriteMsgQueue(IntPtr hMsgQ, byte[] lpBuffer, Int32 cbDataSize, Int32 dwTimeout, Int32 dwFlags);

        public struct MsgQueueInfo
        {
            public Int32 dwSize;
            public Int32 dwFlags;
            public Int32 dwMaxMessages;
            public Int32 cbMaxMessage;
            public Int32 dwCurrentMessages;
            public Int32 dwMaxQueueMessages;
            public Int16 wNumReaders;
            public Int16 wNumWriters;
        }
    }
 }
