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

namespace OpenNETCF.ComponentModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RunWorkerCompletedEventArgs : System.EventArgs
    {
        // This class should inherit from AsyncCompletedEventArgs but I don't see the point in the CF's case
        private readonly object mResult;
        private readonly bool mCancelled;
        private readonly System.Exception mError;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aResult"></param>
        /// <param name="aError"></param>
        /// <param name="aCancelled"></param>
        public RunWorkerCompletedEventArgs(object aResult, System.Exception aError, bool aCancelled)
        {
            mResult = aResult;
            mError = aError;
            mCancelled = aCancelled;
        }

        /// <summary>
        /// 
        /// </summary>
        public object Result
        {
            get
            {
                return mResult;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Cancelled
        {
            get
            {
                return mCancelled;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Exception Error
        {
            get
            {
                return mError;
            }
        }


        #region These are in the help but never seem to get used
        //		private object mUserState;
        //		public object UserState { 
        //			get{
        //				return mUserState;
        //			}
        //		}
        #endregion
    }
}
