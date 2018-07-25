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

namespace OpenNETCF.WindowsCE.Services
{
    public class ServiceCollection : IEnumerable<Service>
    {
        private List<Service> m_services = new List<Service>();

        public IEnumerator<Service> GetEnumerator()
        {
            return m_services.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_services.GetEnumerator();
        }

        private void Add(Service service)
        {
            m_services.Add(service);
        }

        internal void Remove(Service service)
        {
            for(int i = 0; i < m_services.Count ; i++)
            {
                if (m_services[i].Prefix == service.Prefix)
                {
                    m_services.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Gets a collection of Services supported on the device
        /// </summary>
        /// <returns></returns>
        public static ServiceCollection GetServices()
        {
            byte[] data = new byte[0];
            int count;
            int size = 0;

            NativeMethods.EnumServices(data, out count, ref size);

            data = new byte[size];

            NativeMethods.EnumServices(data, out count, ref size);

            ServiceCollection collecton = new ServiceCollection();

            for(int i = 0 ; i < count ; i++)
            {
                ServiceEnumInfo info = new ServiceEnumInfo(data, i * ServiceEnumInfo.SIZE);
                collecton.Add(new Service(info, collecton));
            }

            return collecton;
        }
    }
}
