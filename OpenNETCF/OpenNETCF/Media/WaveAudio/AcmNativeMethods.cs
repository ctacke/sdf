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

namespace OpenNETCF.Media.WaveAudio
{
    public delegate void acmStreamConvertCallback(IntPtr has, MM uMsg, int dwInstance, int lParam1, int lParam2);

    public class AcmNativeMethods
    {
        [DllImport("coredll")]
        extern static public int acmDriverOpen(out IntPtr phad, int hadid, int fdwOpen);
        [DllImport("coredll")]
        extern static public int acmDriverClose(IntPtr had, int fdwClose);
        [DllImport("coredll")]
        extern static public int acmDriverEnum(ACMDRIVERENUMCB fnCallback, int dwInstance, int fdwEnum);
        [DllImport("coredll")]
        extern static public int acmFormatEnum(IntPtr had, ref ACMFORMATDETAILS pafd, ACMFORMATENUMCB fnCallback, int dwInstance, int fdwEnum);
        [DllImport("coredll")]
        extern static public int acmDriverDetails(IntPtr hadid, ref ACMDRIVERDETAILS details, int fdwDetails);
        [DllImport("coredll")]
        extern static public int acmFormatTagEnum(IntPtr had, /*ref ACMFORMATTAGDETAILS*/ IntPtr paftd, ACMFORMATTAGENUMCB fnCallback, int dwInstance, int fdwEnum);
        [DllImport("coredll")]
        extern static public int acmFormatDetails(IntPtr had, ref ACMFORMATDETAILS pafd, AcmFormatDetailsFlags fdwDetails);
        [DllImport("coredll")]
        extern static public int acmFormatDetails(IntPtr had, IntPtr pafd, AcmFormatDetailsFlags fdwDetails);
        [DllImport("coredll")]
        extern static public int acmStreamOpen(out IntPtr phas, IntPtr had, byte[] pwfxSrc, byte[] pwfxDst, IntPtr pwfltr, int dwCallback, int dwInstance, AcmStreamOpenFlags fdwOpen);
        [DllImport("coredll")]
        extern static public int acmStreamClose(IntPtr has, int fdwClose);
        [DllImport("coredll")]
        extern static public int acmStreamSize(IntPtr has, int cbInput, out int pdwOutputBytes, AcmStreamSizeFlags fdwSize);
        [DllImport("coredll")]
        extern static public int acmStreamConvert(IntPtr has, ref ACMSTREAMHEADER pash, AcmStreamConvertFlags fdwConvert);
        [DllImport("coredll")]
        extern static public int acmStreamPrepareHeader(IntPtr has, ref ACMSTREAMHEADER pash, int fdwPrepare);
        [DllImport("coredll")]
        extern static public int acmStreamUnprepareHeader(IntPtr has, ref ACMSTREAMHEADER pash, int fdwUnprepare);
        [DllImport("coredll")]
        extern static public int acmFormatSuggest(IntPtr had, byte[] pwfxSrc, byte[] pwfxDst, int cbwfxDst, AcmFormatSuggestFlags fdwSuggest);
        [DllImport("coredll")]
        extern static public int acmDriverID(IntPtr hao, out int phadid, int fdwDriverID);
    }
}
