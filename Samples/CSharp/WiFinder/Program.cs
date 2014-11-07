using System;

using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WiFinder
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [MTAThread]
    static void Main()
    {
      APView view = new APView();
      IAPPresenter presenter = new APPresenter();
      presenter.Initialize(view);

      Application.Run(view);

      presenter.Dispose();
    }
  }
}