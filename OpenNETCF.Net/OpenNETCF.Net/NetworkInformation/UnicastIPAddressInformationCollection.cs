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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace OpenNETCF.Net.NetworkInformation
{
  /// <summary>
  /// Stores a set of OpenNETCF.Net.NetworkInformation.UnicastIPAddressInformation
  /// types.
  /// </summary>
  public class UnicastIPAddressInformationCollection : ICollection<UnicastIPAddressInformation>, IEnumerable<UnicastIPAddressInformation>, IEnumerable
  {
    private List<UnicastIPAddressInformation> m_list;
    internal string m_adapterName;

    /// <summary>
    /// Initializes a new instance of the OpenNETCF.Net.NetworkInformation.UnicastIPAddressInformationCollection
    /// class.
    /// </summary>
    protected internal UnicastIPAddressInformationCollection(string adapterName)
    {
      m_list = new List<UnicastIPAddressInformation>();
      m_adapterName = adapterName;
    }

    /// <summary>
    /// Gets the number of OpenNETCF.Net.NetworkInformation.UnicastIPAddressInformation
    /// types in this collection.
    /// </summary>
    public virtual int Count
    {
      get { return m_list.Count; }
    }

    /// <summary>
    /// Gets a value that indicates whether access to this collection is read-only.
    /// </summary>
    public virtual bool IsReadOnly
    {
      get { return true; }
    }

    /// <summary>
    /// Gets the OpenNETCF.Net.NetworkInformation.UnicastIPAddressInformation instance
    /// at the specified index in the collection.
    /// </summary>
    /// <param name="index">The zero-based index of the element.</param>
    /// <returns>
    /// The OpenNETCF.Net.NetworkInformation.UnicastIPAddressInformation at the specified
    /// location.
    /// </returns>
    public virtual UnicastIPAddressInformation this[int index]
    {
      get { return m_list[index]; }
    }

    /// <summary>
    /// Throws a System.NotSupportedException because this operation is not supported
    /// for this collection.
    /// </summary>
    /// <param name="address">The object to be added to the collection.</param>
    public void Add(UnicastIPAddressInformation address)
    {
      throw new NotSupportedException("This collection is read-only");
    }
    internal void InternalAdd(UnicastIPAddressInformation address)
    {
      m_list.Add(address);
    }

    /// <summary>
    /// Throws a System.NotSupportedException because this operation is not supported
    /// for this collection.
    /// </summary>
    public void Clear()
    {
      throw new NotSupportedException("This collection is read-only");
    }
    internal void InternalClear()
    {
      m_list.Clear();
    }

    /// <summary>
    /// Checks whether the collection contains the specified System.Net.NetworkInformation.UnicastIPAddressInformation
    /// object.
    /// </summary>
    /// <param name="address">
    /// The System.Net.NetworkInformation.UnicastIPAddressInformation object to be
    /// searched in the collection.
    /// </param>
    /// <returns>
    /// true if the System.Net.NetworkInformation.UnicastIPAddressInformation object
    /// exists in the collection; otherwise, false.
    /// </returns>
    public virtual bool Contains(UnicastIPAddressInformation address)
    {
      return m_list.Contains(address);
    }

    /// <summary>
    /// Copies the collection to the specified array.
    /// </summary>
    /// <param name="array">A one-dimensional array that receives a copy of the collection.</param>
    /// <param name="offset">The zero-based index in array at which the copy begins.</param>
    /// <exception cref="System.ArgumentNullException">array is null.</exception>
    /// <exception cref="System.ArgumentException">
    /// array is multidimensional.-or- offset is equal to or greater than the length
    /// of array.-or- The number of elements in this OpenNETCF.Net.NetworkInformation.IPAddressInformation
    /// is greater than the available space from offset to the end of the destination
    /// array.
    /// </exception>
    public virtual void CopyTo(UnicastIPAddressInformation[] array, int offset)
    {
      if (array == null)
      {
        throw new ArgumentNullException();
      }

      if ((array.Rank > 1) || (array.Length > m_list.Count - offset))
      {
        throw new ArgumentException();
      }

      m_list.CopyTo(array, offset);
    }

    /// <summary>
    /// Returns an object that can be used to iterate through this collection.
    /// </summary>
    /// <returns>
    /// An object that implements the System.Collections.IEnumerator interface and
    /// provides access to the OpenNETCF.Net.NetworkInformation.IPAddressInformation
    /// types in this collection.
    /// </returns>
    public IEnumerator<UnicastIPAddressInformation> GetEnumerator()
    {
      return m_list.GetEnumerator();
    }

    /// <summary>
    /// Throws a System.NotSupportedException because this operation is not supported
    /// for this collection.
    /// </summary>
    /// <param name="address">The object to be removed.</param>
    /// <returns>Always throws a System.NotSupportedException.</returns>
    public bool Remove(UnicastIPAddressInformation address)
    {
      throw new NotSupportedException("This collection is read-only");
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return m_list.GetEnumerator();
    }

    /// <summary>
    /// Gets an IPAddressCollection containing the IP addresses of the UnicastIPAddressInformationCollection
    /// </summary>
    /// <param name="c">a UnicastIPAddressInformationCollection</param>
    /// <returns>An IPAddressCollection</returns>
    public static explicit operator IPAddressCollection(UnicastIPAddressInformationCollection c)
    {
      IPAddressCollection coll = new IPAddressCollection();

      foreach (UnicastIPAddressInformation ui in c)
      {
        coll.InternalAdd(ui.Address);
      }
      return coll;
    }
  }
}
