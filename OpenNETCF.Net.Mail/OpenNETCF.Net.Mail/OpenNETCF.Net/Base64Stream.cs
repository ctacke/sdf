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
using System.IO;
using OpenNETCF.Net.Mail;

namespace OpenNETCF.Net
{
    internal class Base64Stream : DelegatedStream
    {
        // Fields
        private static byte[] base64DecodeMap = new byte[] { 
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x3e, 0xff, 0xff, 0xff, 0x3f, 
        0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 60, 0x3d, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
        0xff, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 
        15, 0x10, 0x11, 0x12, 0x13, 20, 0x15, 0x16, 0x17, 0x18, 0x19, 0xff, 0xff, 0xff, 0xff, 0xff, 
        0xff, 0x1a, 0x1b, 0x1c, 0x1d, 30, 0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 40, 
        0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f, 0x30, 0x31, 50, 0x33, 0xff, 0xff, 0xff, 0xff, 0xff, 
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
     };
        private static byte[] base64EncodeMap = new byte[] { 
        0x41, 0x42, 0x43, 0x44, 0x45, 70, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, 80, 
        0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 90, 0x61, 0x62, 0x63, 100, 0x65, 0x66, 
        0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 110, 0x6f, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 
        0x77, 120, 0x79, 0x7a, 0x30, 0x31, 50, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x2b, 0x2f, 
        0x3d
     };

        private static int DefaultLineLength = 0x4c;
        private int lineLength;
        private ReadStateInfo readState;
        private WriteStateInfo writeState;

        // Methods
        internal Base64Stream()
        {
            this.lineLength = DefaultLineLength;
        }

        internal Base64Stream(int lineLength)
        {
            this.lineLength = lineLength;
        }

        internal Base64Stream(Stream stream)
            : this(stream, DefaultLineLength)
        {
        }

        internal Base64Stream(Stream stream, int lineLength)
            : base(stream)
        {
            this.lineLength = lineLength;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
            //if (buffer == null)
            //{
            //    throw new ArgumentNullException("buffer");
            //}
            //if ((offset < 0) || (offset > buffer.Length))
            //{
            //    throw new ArgumentOutOfRangeException("offset");
            //}
            //if ((offset + count) > buffer.Length)
            //{
            //    throw new ArgumentOutOfRangeException("count");
            //}
            //ReadAsyncResult result = new ReadAsyncResult(this, buffer, offset, count, callback, state);
            //result.Read();
            //return result;
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
            //if (buffer == null)
            //{
            //    throw new ArgumentNullException("buffer");
            //}
            //if ((offset < 0) || (offset > buffer.Length))
            //{
            //    throw new ArgumentOutOfRangeException("offset");
            //}
            //if ((offset + count) > buffer.Length)
            //{
            //    throw new ArgumentOutOfRangeException("count");
            //}
            //WriteAsyncResult result = new WriteAsyncResult(this, buffer, offset, count, callback, state);
            //result.Write();
            //return result;
        }

        public override void Close()
        {
            if ((this.writeState != null) && (this.WriteState.Length > 0))
            {
                switch (this.WriteState.Padding)
                {
                    case 1:
                        {
                            int num5;
                            int num6;
                            WriteStateInfo writeState = this.WriteState;
                            writeState.Length = (num5 = writeState.Length) + 1;
                            this.WriteState.Buffer[num5] = base64EncodeMap[this.WriteState.LastBits];
                            WriteStateInfo info5 = this.WriteState;
                            info5.Length = (num6 = info5.Length) + 1;
                            this.WriteState.Buffer[num6] = base64EncodeMap[0x40];
                            break;
                        }
                    case 2:
                        {
                            int num2;
                            int num3;
                            int num4;
                            WriteStateInfo info1 = this.WriteState;
                            info1.Length = (num2 = info1.Length) + 1;
                            this.WriteState.Buffer[num2] = base64EncodeMap[this.WriteState.LastBits];
                            WriteStateInfo info2 = this.WriteState;
                            info2.Length = (num3 = info2.Length) + 1;
                            this.WriteState.Buffer[num3] = base64EncodeMap[0x40];
                            WriteStateInfo info3 = this.WriteState;
                            info3.Length = (num4 = info3.Length) + 1;
                            this.WriteState.Buffer[num4] = base64EncodeMap[0x40];
                            break;
                        }
                }
                this.WriteState.Padding = 0;
                this.FlushInternal();
            }
            base.Close();
        }

        internal unsafe int DecodeBytes(byte[] buffer, int offset, int count)
        {
            fixed (byte* numRef = buffer)
            {
                byte* numPtr = numRef + offset;
                byte* numPtr2 = numPtr;
                byte* numPtr3 = numPtr;
                byte* numPtr4 = numPtr + count;
                while (numPtr2 < numPtr4)
                {
                    if (((numPtr2[0] == 13) || (numPtr2[0] == 10)) || (numPtr2[0] == 0x3d))
                    {
                        numPtr2++;
                        continue;
                    }
                    byte num = base64DecodeMap[numPtr2[0]];
                    if (num == 0xff)
                    {
                        throw new FormatException(SR.GetString("MailBase64InvalidCharacter"));
                    }
                    switch (this.ReadState.Pos)
                    {
                        case 0:
                            {
                                this.ReadState.Val = (byte)(num << 2);
                                ReadStateInfo readState = this.ReadState;
                                readState.Pos = (byte)(readState.Pos + 1);
                                break;
                            }
                        case 1:
                            {
                                numPtr3++;
                                numPtr3[0] = (byte)(this.ReadState.Val + (num >> 4));
                                this.ReadState.Val = (byte)(num << 4);
                                ReadStateInfo info2 = this.ReadState;
                                info2.Pos = (byte)(info2.Pos + 1);
                                break;
                            }
                        case 2:
                            {
                                numPtr3++;
                                numPtr3[0] = (byte)(this.ReadState.Val + (num >> 2));
                                this.ReadState.Val = (byte)(num << 6);
                                ReadStateInfo info3 = this.ReadState;
                                info3.Pos = (byte)(info3.Pos + 1);
                                break;
                            }
                        case 3:
                            numPtr3++;
                            numPtr3[0] = (byte)(this.ReadState.Val + num);
                            this.ReadState.Pos = 0;
                            break;
                    }
                    numPtr2++;
                }
                count = (int)((long)((numPtr3 - numPtr) / 1));
            }
            return count;
        }

        internal int EncodeBytes(byte[] buffer, int offset, int count, bool dontDeferFinalBytes)
        {
            int index = offset;
            switch (this.WriteState.Padding)
            {
                case 1:
                    {
                        int num7;
                        int num8;
                        WriteStateInfo writeState = this.WriteState;
                        writeState.Length = (num7 = writeState.Length) + 1;
                        this.WriteState.Buffer[num7] = base64EncodeMap[this.WriteState.LastBits | ((buffer[index] & 0xc0) >> 6)];
                        WriteStateInfo info6 = this.WriteState;
                        info6.Length = (num8 = info6.Length) + 1;
                        this.WriteState.Buffer[num8] = base64EncodeMap[buffer[index] & 0x3f];
                        index++;
                        count--;
                        this.WriteState.Padding = 0;
                        WriteStateInfo info7 = this.WriteState;
                        info7.CurrentLineLength++;
                        break;
                    }
                case 2:
                    {
                        int num4;
                        WriteStateInfo info1 = this.WriteState;
                        info1.Length = (num4 = info1.Length) + 1;
                        this.WriteState.Buffer[num4] = base64EncodeMap[this.WriteState.LastBits | ((buffer[index] & 240) >> 4)];
                        if (count != 1)
                        {
                            int num5;
                            int num6;
                            WriteStateInfo info2 = this.WriteState;
                            info2.Length = (num5 = info2.Length) + 1;
                            this.WriteState.Buffer[num5] = base64EncodeMap[((buffer[index] & 15) << 2) | ((buffer[index + 1] & 0xc0) >> 6)];
                            WriteStateInfo info3 = this.WriteState;
                            info3.Length = (num6 = info3.Length) + 1;
                            this.WriteState.Buffer[num6] = base64EncodeMap[buffer[index + 1] & 0x3f];
                            index += 2;
                            count -= 2;
                            this.WriteState.Padding = 0;
                            WriteStateInfo info4 = this.WriteState;
                            info4.CurrentLineLength += 2;
                            break;
                        }
                        this.WriteState.LastBits = (byte)((buffer[index] & 15) << 2);
                        this.WriteState.Padding = 1;
                        return (index - offset);
                    }
            }
            int num2 = index + (count - (count % 3));
            while (index < num2)
            {
                int num11;
                int num12;
                int num13;
                int num14;
                if ((this.lineLength != -1) && ((this.WriteState.CurrentLineLength + 4) > (this.lineLength - 2)))
                {
                    int num9;
                    int num10;
                    WriteStateInfo info8 = this.WriteState;
                    info8.Length = (num9 = info8.Length) + 1;
                    this.WriteState.Buffer[num9] = 13;
                    WriteStateInfo info9 = this.WriteState;
                    info9.Length = (num10 = info9.Length) + 1;
                    this.WriteState.Buffer[num10] = 10;
                    this.WriteState.CurrentLineLength = 0;
                }
                if ((this.WriteState.Length + 4) > this.WriteState.Buffer.Length)
                {
                    return (index - offset);
                }
                WriteStateInfo info10 = this.WriteState;
                info10.Length = (num11 = info10.Length) + 1;
                this.WriteState.Buffer[num11] = base64EncodeMap[(buffer[index] & 0xfc) >> 2];
                WriteStateInfo info11 = this.WriteState;
                info11.Length = (num12 = info11.Length) + 1;
                this.WriteState.Buffer[num12] = base64EncodeMap[((buffer[index] & 3) << 4) | ((buffer[index + 1] & 240) >> 4)];
                WriteStateInfo info12 = this.WriteState;
                info12.Length = (num13 = info12.Length) + 1;
                this.WriteState.Buffer[num13] = base64EncodeMap[((buffer[index + 1] & 15) << 2) | ((buffer[index + 2] & 0xc0) >> 6)];
                WriteStateInfo info13 = this.WriteState;
                info13.Length = (num14 = info13.Length) + 1;
                this.WriteState.Buffer[num14] = base64EncodeMap[buffer[index + 2] & 0x3f];
                WriteStateInfo info14 = this.WriteState;
                info14.CurrentLineLength += 4;
                index += 3;
            }
            index = num2;
            if ((this.WriteState.Length + 4) > this.WriteState.Buffer.Length)
            {
                return (index - offset);
            }
            if ((this.lineLength != -1) && ((this.WriteState.CurrentLineLength + 4) > this.lineLength))
            {
                int num15;
                int num16;
                WriteStateInfo info15 = this.WriteState;
                info15.Length = (num15 = info15.Length) + 1;
                this.WriteState.Buffer[num15] = 13;
                WriteStateInfo info16 = this.WriteState;
                info16.Length = (num16 = info16.Length) + 1;
                this.WriteState.Buffer[num16] = 10;
                this.WriteState.CurrentLineLength = 0;
            }
            switch ((count % 3))
            {
                case 1:
                    {
                        int num22;
                        WriteStateInfo info23 = this.WriteState;
                        info23.Length = (num22 = info23.Length) + 1;
                        this.WriteState.Buffer[num22] = base64EncodeMap[(buffer[index] & 0xfc) >> 2];
                        if (!dontDeferFinalBytes)
                        {
                            this.WriteState.LastBits = (byte)((buffer[index] & 3) << 4);
                            this.WriteState.Padding = 2;
                            WriteStateInfo info28 = this.WriteState;
                            info28.CurrentLineLength++;
                        }
                        else
                        {
                            int num23;
                            int num24;
                            int num25;
                            WriteStateInfo info24 = this.WriteState;
                            info24.Length = (num23 = info24.Length) + 1;
                            this.WriteState.Buffer[num23] = base64EncodeMap[(byte)((buffer[index] & 3) << 4)];
                            WriteStateInfo info25 = this.WriteState;
                            info25.Length = (num24 = info25.Length) + 1;
                            this.WriteState.Buffer[num24] = base64EncodeMap[0x40];
                            WriteStateInfo info26 = this.WriteState;
                            info26.Length = (num25 = info26.Length) + 1;
                            this.WriteState.Buffer[num25] = base64EncodeMap[0x40];
                            this.WriteState.Padding = 0;
                            WriteStateInfo info27 = this.WriteState;
                            info27.CurrentLineLength += 4;
                        }
                        index++;
                        goto Label_0677;
                    }
                case 2:
                    {
                        int num18;
                        int num19;
                        int num20;
                        int num21;
                        WriteStateInfo info17 = this.WriteState;
                        info17.Length = (num18 = info17.Length) + 1;
                        this.WriteState.Buffer[num18] = base64EncodeMap[(buffer[index] & 0xfc) >> 2];
                        WriteStateInfo info18 = this.WriteState;
                        info18.Length = (num19 = info18.Length) + 1;
                        this.WriteState.Buffer[num19] = base64EncodeMap[((buffer[index] & 3) << 4) | ((buffer[index + 1] & 240) >> 4)];
                        if (!dontDeferFinalBytes)
                        {
                            this.WriteState.LastBits = (byte)((buffer[index + 1] & 15) << 2);
                            this.WriteState.Padding = 1;
                            WriteStateInfo info22 = this.WriteState;
                            info22.CurrentLineLength += 2;
                            break;
                        }
                        WriteStateInfo info19 = this.WriteState;
                        info19.Length = (num20 = info19.Length) + 1;
                        this.WriteState.Buffer[num20] = base64EncodeMap[(buffer[index + 1] & 15) << 2];
                        WriteStateInfo info20 = this.WriteState;
                        info20.Length = (num21 = info20.Length) + 1;
                        this.WriteState.Buffer[num21] = base64EncodeMap[0x40];
                        this.WriteState.Padding = 0;
                        WriteStateInfo info21 = this.WriteState;
                        info21.CurrentLineLength += 4;
                        break;
                    }
                default:
                    goto Label_0677;
            }
            index += 2;
        Label_0677:
            return (index - offset);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            throw new NotSupportedException();
            //if (asyncResult == null)
            //{
            //    throw new ArgumentNullException("asyncResult");
            //}
            //return ReadAsyncResult.End(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            throw new NotSupportedException();
            //if (asyncResult == null)
            //{
            //    throw new ArgumentNullException("asyncResult");
            //}
            //WriteAsyncResult.End(asyncResult);
        }

        public override void Flush()
        {
            if ((this.writeState != null) && (this.WriteState.Length > 0))
            {
                this.FlushInternal();
            }
            base.Flush();
        }

        private void FlushInternal()
        {
            base.Write(this.WriteState.Buffer, 0, this.WriteState.Length);
            this.WriteState.Length = 0;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num;
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((offset < 0) || (offset > buffer.Length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((offset + count) > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            do
            {
                num = base.Read(buffer, offset, count);
                if (num == 0)
                {
                    return 0;
                }
                num = this.DecodeBytes(buffer, offset, num);
            }
            while (num <= 0);
            return num;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((offset < 0) || (offset > buffer.Length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((offset + count) > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            int num = 0;
            while (true)
            {
                num += this.EncodeBytes(buffer, offset + num, count - num, false);
                if (num >= count)
                {
                    break;
                }
                this.FlushInternal();
            }
        }

        // Properties
        public override bool CanWrite
        {
            get
            {
                return base.CanWrite;
            }
        }

        private ReadStateInfo ReadState
        {
            get
            {
                if (this.readState == null)
                {
                    this.readState = new ReadStateInfo();
                }
                return this.readState;
            }
        }

        internal WriteStateInfo WriteState
        {
            get
            {
                if (this.writeState == null)
                {
                    this.writeState = new WriteStateInfo(0x400);
                }
                return this.writeState;
            }
        }

        //// Nested Types
        //private class ReadAsyncResult
        //{
        //    // Fields
        //    private byte[] buffer;
        //    private int count;
        //    private int offset;
        //    private static AsyncCallback onRead = new AsyncCallback(Base64Stream.ReadAsyncResult.OnRead);
        //    private Base64Stream parent;
        //    private int read;

        //    // Methods
        //    internal ReadAsyncResult(Base64Stream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        //        : base(null, state, callback)
        //    {
        //        this.parent = parent;
        //        this.buffer = buffer;
        //        this.offset = offset;
        //        this.count = count;
        //    }

        //    private bool CompleteRead(IAsyncResult result)
        //    {
        //        this.read = this.parent.BaseStream.EndRead(result);
        //        if (this.read == 0)
        //        {
        //            base.InvokeCallback();
        //            return true;
        //        }
        //        this.read = this.parent.DecodeBytes(this.buffer, this.offset, this.read);
        //        if (this.read > 0)
        //        {
        //            base.InvokeCallback();
        //            return true;
        //        }
        //        return false;
        //    }

        //    internal static int End(IAsyncResult result)
        //    {
        //        Base64Stream.ReadAsyncResult result2 = (Base64Stream.ReadAsyncResult)result;
        //        result2.InternalWaitForCompletion();
        //        return result2.read;
        //    }

        //    private static void OnRead(IAsyncResult result)
        //    {
        //        if (!result.CompletedSynchronously)
        //        {
        //            Base64Stream.ReadAsyncResult asyncState = (Base64Stream.ReadAsyncResult)result.AsyncState;
        //            try
        //            {
        //                if (!asyncState.CompleteRead(result))
        //                {
        //                    asyncState.Read();
        //                }
        //            }
        //            catch (Exception exception)
        //            {
        //                if (asyncState.IsCompleted)
        //                {
        //                    throw;
        //                }
        //                asyncState.InvokeCallback(exception);
        //            }
        //            catch
        //            {
        //                if (asyncState.IsCompleted)
        //                {
        //                    throw;
        //                }
        //                asyncState.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
        //            }
        //        }
        //    }

        //    internal void Read()
        //    {
        //        IAsyncResult result;
        //        do
        //        {
        //            result = this.parent.BaseStream.BeginRead(this.buffer, this.offset, this.count, onRead, this);
        //        }
        //        while (result.CompletedSynchronously && !this.CompleteRead(result));
        //    }
        //}

        private class ReadStateInfo
        {
            // Fields
            private byte pos;
            private byte val;

            // Properties
            internal byte Pos
            {
                get
                {
                    return this.pos;
                }
                set
                {
                    this.pos = value;
                }
            }

            internal byte Val
            {
                get
                {
                    return this.val;
                }
                set
                {
                    this.val = value;
                }
            }
        }

        //private class WriteAsyncResult
        //{
        //    // Fields
        //    private byte[] buffer;
        //    private int count;
        //    private int offset;
        //    private static AsyncCallback onWrite = new AsyncCallback(Base64Stream.WriteAsyncResult.OnWrite);
        //    private Base64Stream parent;
        //    private int written;

        //    // Methods
        //    internal WriteAsyncResult(Base64Stream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        //        : base(null, state, callback)
        //    {
        //        this.parent = parent;
        //        this.buffer = buffer;
        //        this.offset = offset;
        //        this.count = count;
        //    }

        //    private void CompleteWrite(IAsyncResult result)
        //    {
        //        this.parent.BaseStream.EndWrite(result);
        //        this.parent.WriteState.Length = 0;
        //    }

        //    internal static void End(IAsyncResult result)
        //    {
        //        ((Base64Stream.WriteAsyncResult)result).InternalWaitForCompletion();
        //    }

        //    private static void OnWrite(IAsyncResult result)
        //    {
        //        if (!result.CompletedSynchronously)
        //        {
        //            Base64Stream.WriteAsyncResult asyncState = (Base64Stream.WriteAsyncResult)result.AsyncState;
        //            try
        //            {
        //                asyncState.CompleteWrite(result);
        //                asyncState.Write();
        //            }
        //            catch (Exception exception)
        //            {
        //                if (asyncState.IsCompleted)
        //                {
        //                    throw;
        //                }
        //                asyncState.InvokeCallback(exception);
        //            }
        //            catch
        //            {
        //                if (asyncState.IsCompleted)
        //                {
        //                    throw;
        //                }
        //                asyncState.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
        //            }
        //        }
        //    }

        //    internal void Write()
        //    {
        //        while (true)
        //        {
        //            this.written += this.parent.EncodeBytes(this.buffer, this.offset + this.written, this.count - this.written, false);
        //            if (this.written >= this.count)
        //            {
        //                break;
        //            }
        //            IAsyncResult result = this.parent.BaseStream.BeginWrite(this.parent.WriteState.Buffer, 0, this.parent.WriteState.Length, onWrite, this);
        //            if (!result.CompletedSynchronously)
        //            {
        //                return;
        //            }
        //            this.CompleteWrite(result);
        //        }
        //        base.InvokeCallback();
        //    }
        //}

        internal class WriteStateInfo
        {
            // Fields
            private int currentLineLength;
            private byte lastBits;
            private byte[] outBuffer;
            private int outLength;
            private int padding;

            // Methods
            internal WriteStateInfo(int bufferSize)
            {
                this.outBuffer = new byte[bufferSize];
            }

            // Properties
            internal byte[] Buffer
            {
                get
                {
                    return this.outBuffer;
                }
            }

            internal int CurrentLineLength
            {
                get
                {
                    return this.currentLineLength;
                }
                set
                {
                    this.currentLineLength = value;
                }
            }

            internal byte LastBits
            {
                get
                {
                    return this.lastBits;
                }
                set
                {
                    this.lastBits = value;
                }
            }

            internal int Length
            {
                get
                {
                    return this.outLength;
                }
                set
                {
                    this.outLength = value;
                }
            }

            internal int Padding
            {
                get
                {
                    return this.padding;
                }
                set
                {
                    this.padding = value;
                }
            }
        }
    }
}