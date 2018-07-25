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
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsMobile
{
  public class Radios : IEnumerable<IRadio>
  {
    private const string GET_DEVICE_LIST_ORDINAL = "#276";
    private const string SET_RADIO_STATE_ORDINAL = "#273";
    private const string FREE_DEVICE_LIST_ORDINAL = "#280";

    private List<IRadio> m_radios;
    private static Radios m_instance;

    public static Radios GetRadios()
    {
      if (m_instance == null)
      {
        m_instance = new Radios();
      }

      return m_instance;
    }

    private Radios()
    {
      m_radios = new List<IRadio>();
      Refresh();
    }

    private enum SaveAction
    {
      NoSave = 0,
      PreSave = 1,
      PostSave = 2

    }

    public int Count
    {
      get { return m_radios.Count; }
    }

    public void Refresh()
    {
      lock (m_radios)
      {
        IntPtr pDeviceList = IntPtr.Zero;

        try
        {
          if (GetDeviceList(ref pDeviceList, 0) == -1)
          {
            throw new Exception("Failed to get device list");
          }
        }
        catch (MissingMethodException)
        {
          throw new PlatformNotSupportedException("Radios are only supported on Windows Mobile platforms, versions 5.0 and later");
        }

        m_radios.Clear();

        if (pDeviceList != IntPtr.Zero)
        {
          try
          {
            RadioDevice device;
            IntPtr next = pDeviceList;

            do
            {
              device = new RadioDevice(next, 0);

              IRadio radio = null;

              switch (device.RadioType)
              {
                case RadioType.Bluetooth:
                  radio = new BluetoothRadio(device, this);
                  break;
                case RadioType.Phone:
                  radio = new PhoneRadio(device, this);
                  break;
                case RadioType.WiFi:
                  radio = new WiFiRadio(device, this);
                  break;
                default:
                  radio = new UnknownRadio(device, this);
                  break;
              }

              if (radio != null) m_radios.Add(radio);

              next = device.Next;
            } while (next != IntPtr.Zero);
          }
          finally
          {
            FreeDeviceList(pDeviceList);
          }
        }
      }
    }

    internal void SetState(IRadio radio, RadioState newState)
    {
      lock (m_radios)
      {
        IntPtr pDeviceList = IntPtr.Zero;

        if (GetDeviceList(ref pDeviceList, 0) == -1)
        {
          throw new Exception("Failed to get device list");
        }

        if (pDeviceList != IntPtr.Zero)
        {
          try
          {
            RadioDevice device;
            IntPtr next = pDeviceList;

            do
            {
              device = new RadioDevice(next, 0);

              if(device.RadioType == radio.RadioType)
              {
                ChangeRadioState(device.Pointer, newState, SaveAction.PreSave);
                // don't exit the loop - we need to free all name strings
              }
              next = device.Next;
            } while (next != IntPtr.Zero);
          }
          finally
          {
            FreeDeviceList(pDeviceList);
          }
        }
      }
    }

    public IEnumerator<IRadio> GetEnumerator()
    {
      return m_radios.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return m_radios.GetEnumerator();
    }

    [DllImport("ossvcs.dll", SetLastError = true, EntryPoint = GET_DEVICE_LIST_ORDINAL)]
    private static extern int GetDeviceList(ref IntPtr pDevices, int dwFlags);

    [DllImport("ossvcs.dll", SetLastError = true, EntryPoint=FREE_DEVICE_LIST_ORDINAL)]
    private static extern int FreeDeviceList(IntPtr pDevices);

    [DllImport("ossvcs.dll", SetLastError = true, EntryPoint=SET_RADIO_STATE_ORDINAL)]
    private static extern int ChangeRadioState(IntPtr pDevice, RadioState dwState, SaveAction sa);
  }
}
