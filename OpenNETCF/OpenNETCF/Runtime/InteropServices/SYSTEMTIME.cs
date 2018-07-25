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
using System.Runtime.InteropServices;

namespace OpenNETCF.Runtime.InteropServices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMillisecond;

        /*public static SYSTEMTIME FromByteArray(Byte[] array, int offset)
        {
            SYSTEMTIME st = new SYSTEMTIME();
            st.wYear = (short)BitConverter.ToInt16(array, offset);
            st.wMonth = (short)BitConverter.ToInt16(array, offset + 2);
            st.wDay = (short)BitConverter.ToInt16(array, offset + 6);
            st.wHour = (short)BitConverter.ToInt16(array, offset + 8);
            st.wMinute = (short)BitConverter.ToInt16(array, offset + 10);
            st.wSecond = (short)BitConverter.ToInt16(array, offset + 12);

            return st;
        }*/

        public static SYSTEMTIME FromDateTime(DateTime dt)
        {
            SYSTEMTIME st = new SYSTEMTIME();
            st.wYear = (short)dt.Year;
            st.wMonth = (short)dt.Month;
            st.wDayOfWeek = (short)dt.DayOfWeek;
            st.wDay = (short)dt.Day;
            st.wHour = (short)dt.Hour;
            st.wMinute = (short)dt.Minute;
            st.wSecond = (short)dt.Second;
            st.wMillisecond = (short)dt.Millisecond;

            return st;
        }

        public DateTime ToDateTime()
        {
            if (wYear == 0 && wMonth == 0 && wDay == 0 && wHour == 0 && wMinute == 0 && wSecond == 0)
            {
                return DateTime.MinValue;
            }
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMillisecond);
        }
    }
}
