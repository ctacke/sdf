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
