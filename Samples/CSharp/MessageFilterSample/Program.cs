using System;

using System.Collections.Generic;
using System.Windows.Forms;
using OpenNETCF.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace MessageFilterSample
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [MTAThread]
    static void Main()
    {
      Application2.Run(new Form1());
    }
  }
}