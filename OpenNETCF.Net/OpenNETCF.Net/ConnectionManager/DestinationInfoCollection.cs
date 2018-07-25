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



using System.Collections;
using System.Collections.Generic;

namespace OpenNETCF.Net
{
    /// <summary>
    /// Summary description for DestinationInfoCollection.
    /// </summary>
    public class DestinationInfoCollection : IEnumerable<DestinationInfo>
    {
        private List<DestinationInfo> m_destinations;

        internal DestinationInfoCollection()
        {
            m_destinations = new List<DestinationInfo>();
        }

        internal DestinationInfoCollection(DestinationInfo[] items)
        {
            m_destinations = new List<DestinationInfo>(items);
        }

        internal void Add(DestinationInfo value)
        {
            m_destinations.Add(value);
        }

        public bool Contains(DestinationInfo value)
        {
            return m_destinations.Contains(value);
        }

        public int IndexOf(DestinationInfo value)
        {
            return m_destinations.IndexOf(value);
        }

        public int Count
        {
            get { return m_destinations.Count; }
        }

        public DestinationInfo this[int index]
        {
            get { return (DestinationInfo) m_destinations[index]; }
            internal set { m_destinations[index] = value; }
        }

        public IEnumerator<DestinationInfo> GetEnumerator()
        {
            return m_destinations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_destinations.GetEnumerator();
        }
    }
}