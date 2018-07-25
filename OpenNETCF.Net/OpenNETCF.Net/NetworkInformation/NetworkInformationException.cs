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
using System.ComponentModel;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// The exception that is thrown when an error occurs while retrieving network
    /// information.
    /// </summary>
    public class NetworkInformationException : Exception
    {
        int m_errorCode = 0;

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.NetworkInformationException
        /// class.
        /// </summary>
        public NetworkInformationException()
            : base() 
        {
            m_errorCode = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
        }

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.NetworkInformationException
        /// class with the specified error code.
        /// </summary>
        /// <param name="error">A Win32 error code.</param>
        public NetworkInformationException(int error)
            : base() { m_errorCode = error; }

        /// <summary>
        /// Gets the Win32 error code for this exception. 
        /// </summary>
        public int ErrorCode
        {
            get { return m_errorCode; }
        }
    }
}
