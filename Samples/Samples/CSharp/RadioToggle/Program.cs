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
using OpenNETCF.WindowsMobile;
using System.Threading;
using System.Diagnostics;

namespace RadioToggle
{
  class Program
  {
    static void Main(string[] args)
    {
      Radios radios = Radios.GetRadios();

      Debug.WriteLine("\nBefore\r\n--------");
      foreach (IRadio radio in radios)
      {
        Debug.WriteLine(string.Format("Name: {0}, Type: {1}, State: {2}", radio.DeviceName, radio.RadioType.ToString(), radio.RadioState.ToString()));

        // toggle all radio states
        radio.RadioState = (radio.RadioState == RadioState.On) ? RadioState.Off : RadioState.On;
      }

      // give the radios enough time to change state - some (like BT) seem to be slow
      Thread.Sleep(1000);

      radios.Refresh();

      // display again
      Debug.WriteLine("\r\nAfter\r\n--------");
      foreach (IRadio radio in radios)
      {
        Debug.WriteLine(string.Format("Name: {0}, Type: {1}, State: {2}", radio.DeviceName, radio.RadioType.ToString(), radio.RadioState.ToString()));
      }
      Debug.WriteLine("\r\n\n");
      Thread.Sleep(100);
    }
  }
}
