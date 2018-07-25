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
using OpenNETCF.Net.Mail;

namespace OpenNETCF.Net
{
    internal class BufferBuilder
    {
        // Fields
        private byte[] buffer;
        private int offset;

        // Methods
        internal BufferBuilder()
            : this(0x100)
        {
        }

        internal BufferBuilder(int initialSize)
        {
            this.buffer = new byte[initialSize];
        }

        internal void Append(byte[] value)
        {
            this.Append(value, 0, value.Length);
        }

        internal void Append(byte value)
        {
            this.EnsureBuffer(1);
            this.buffer[this.offset++] = value;
        }

        internal void Append(string value)
        {
            this.Append(value, 0, value.Length);
        }

        internal void Append(byte[] value, int offset, int count)
        {
            this.EnsureBuffer(count);
            Buffer.BlockCopy(value, offset, this.buffer, this.offset, count);
            this.offset += count;
        }

        internal void Append(string value, int offset, int count)
        {
            this.EnsureBuffer(count);
            for (int i = 0; i < count; i++)
            {
                char ch = value[offset + i];
                if (ch > '\x00ff')
                {
                    throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
                }
                this.buffer[this.offset + i] = (byte)ch;
            }
            this.offset += count;
        }

        private void EnsureBuffer(int count)
        {
            if (count > (this.buffer.Length - this.offset))
            {
                byte[] dst = new byte[((this.buffer.Length * 2) > (this.buffer.Length + count)) ? (this.buffer.Length * 2) : (this.buffer.Length + count)];
                Buffer.BlockCopy(this.buffer, 0, dst, 0, this.offset);
                this.buffer = dst;
            }
        }

        internal byte[] GetBuffer()
        {
            return this.buffer;
        }

        internal void Reset()
        {
            this.offset = 0;
        }

        // Properties
        internal int Length
        {
            get
            {
                return this.offset;
            }
        }
    }

}