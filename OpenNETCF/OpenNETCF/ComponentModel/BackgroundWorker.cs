using System.ComponentModel;
using System;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OpenNETCF.ComponentModel
{
  /// <summary>
  /// Executes an operation on a separate thread.
  /// </summary>
  //[ToolboxItemFilter("NETCF",ToolboxItemFilterType.Require),
  //ToolboxItemFilter("System.CF.Windows.Forms", ToolboxItemFilterType.Custom)]
  public class BackgroundWorker : Component
  {
    private object m_progressSyncRoot = new object();
    private System.Windows.Forms.Control m_guiMarshaller;

    #region Public Interface

    /// <summary>
    /// Occurs when <see cref="RunWorkerAsync()"/> is called.
    /// </summary>
    public event DoWorkEventHandler DoWork;
    /// <summary>
    /// Occurs when <see cref="ReportProgress(System.Int32)"/> is called.
    /// </summary>
    public event ProgressChangedEventHandler ProgressChanged;

    /// <summary>
    /// Occurs when the background operation has completed, has been cancelled, or has raised an exception.
    /// </summary>
    public event RunWorkerCompletedEventHandler RunWorkerCompleted;

    private delegate void MethodInvoker();
    private bool m_stopThreads = false;

    /// <summary>
    /// Initializes a new instance of the BackgroundWorker class.
    /// Call from the desktop code as the other ctor is not good enough
    /// Call it passing in a created control e.g. the Form
    /// </summary>
    public BackgroundWorker(Control c)
      : base()
    {
      if (c == null) throw new ArgumentNullException();

      m_guiMarshaller = c;

      Thread progressThread = new Thread(ProgressDispatcherProc);
      progressThread.IsBackground = true;
      progressThread.Start();
    }

    ~BackgroundWorker()
    {
      m_stopThreads = true;
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      m_stopThreads = true;
    }
    
    /// <summary>
    /// Initializes a new instance of the BackgroundWorker class.
    /// </summary>
    public BackgroundWorker()
      : this(new Control())
    {
      /* 
        ideally we want to call Control.CreateControl()
        without it, running on the desktop will crash (it is OK on the CF)
        [on the full fx simply calling a control's constructor does not create the Handle.]
			  
        The CreateControl method is not supported on the CF so to keep this assembly retargettable
        I have offered the alternative ctor for desktop clients 
        (where they can pass in already created controls)
      */
    }

    /// <summary>
    /// Gets a value indicating whether the application has requested cancellation of a background operation.
    /// </summary>
    public bool CancellationPending { private set; get; }

    /// <summary>
    /// Raises the BackgroundWorker.ProgressChanged event.
    /// </summary>
    /// <param name="aProgressPercent">The percentage, from 0 to 100, of the background operation that is complete. </param>
    public void ReportProgress(int aProgressPercent)
    {
      this.ReportProgress(aProgressPercent, null);
    }

    /// <summary>
    /// Raises the BackgroundWorker.ProgressChanged event.
    /// </summary>
    /// <param name="aProgressPercent">The percentage, from 0 to 100, of the background operation that is complete. </param>
    /// <param name="aUserState">The state object passed to BackgroundWorker.RunWorkerAsync(System.Object).</param>
    public void ReportProgress(int aProgressPercent, object aUserState)
    {
      if (!WorkerReportsProgress)
      {
        throw new System.InvalidOperationException("Doesn't do progress events. You must WorkerReportsProgress=True");
      }

      if ((aProgressPercent < 0) || (aProgressPercent > 100))
      {
        throw new ArgumentException("aProgressPercent out of range");
      }

      // Send the event to the GUI
      m_progressQueue.Enqueue(new ProgressChangedEventArgs(aProgressPercent, aUserState));
    }

    /// <summary>
    /// Starts execution of a background operation.
    /// </summary>
    public void RunWorkerAsync()
    {
      this.RunWorkerAsync(null);
    }

    /// <summary>
    /// Starts execution of a background operation.
    /// </summary>
    /// <param name="aArgument"> A parameter for use by the background operation to be executed in the BackgroundWorker.DoWork event handler.</param>
    public void RunWorkerAsync(object aArgument)
    {
      if (IsBusy)
      {
        throw new System.InvalidOperationException("Already in use");
      }

      if (DoWork == null)
      {
        throw new System.InvalidOperationException("You must subscribe to the DoWork event.");
      }

      IsBusy = true;
      CancellationPending = false;

      System.Threading.ThreadPool.QueueUserWorkItem(
        new System.Threading.WaitCallback(DoTheRealWork), aArgument);
    }

    /// <summary>
    /// Requests cancellation of a pending background operation.
    /// </summary>
    public void CancelAsync()
    {
      if (!WorkerSupportsCancellation)
      {
        throw new System.InvalidOperationException("Does not support cancel. You must WorkerSupportsCancellation=true");
      }
      CancellationPending = true;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the BackgroundWorker object can report progress updates.
    /// </summary>
    public bool WorkerReportsProgress { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the BackgroundWorker object supports asynchronous cancellation.
    /// </summary>
#if DESIGN
		[Category("Asynchronous"),
		Description("Whether the worker supports cancellation.")]
#endif
    public bool WorkerSupportsCancellation { get; set; }

    /// <summary>
    ///  Gets a value indicating whether the System.ComponentModel.BackgroundWorker is running an asynchronous operation.
    /// Returns:
    /// true, if the System.ComponentModel.BackgroundWorker is running an
    /// asynchronous operation; otherwise, false.
    /// </summary>
    public bool IsBusy { get; private set; }

    #endregion

    #region Private Methods
    private Queue<ProgressChangedEventArgs> m_progressQueue = new Queue<ProgressChangedEventArgs>();

    private void ProgressDispatcherProc()
    {
      m_stopThreads = false;

      while (!m_stopThreads)
      {
        while (m_progressQueue.Count > 0)
        {
          ProgressChangedEventArgs args = m_progressQueue.Dequeue();

          if (ProgressChanged != null)
          {
            m_guiMarshaller.BeginInvoke(new MethodInvoker(delegate()
            {
              ProgressChanged(this, args);
            }));
            Application.DoEvents();
          }
        }
        // sleep longer if no pregress events are ever coming in
        // keep the thread though in case they turn progress on at some point
        Thread.Sleep(this.WorkerReportsProgress ? 5 : 1000);
      }
    }

    // Async(ThreadPool) called by RunWorkerAsync [the little engine of this class]
    private void DoTheRealWork(object o)
    {
      // declare/initialise the vars we will pass back to client on completion
      System.Exception er = null;
      bool ca = false;
      object result = null;

      // Raise the event passing the original argument and catching any exceptions
      try
      {
        DoWorkEventArgs inOut = new DoWorkEventArgs(o);
        DoWork(this, inOut);

        ca = inOut.Cancel;
        result = inOut.Result;
      }
      catch (System.Exception ex)
      {
        er = ex;
      }

      // store the completed final result in a temp var
      RunWorkerCompletedEventArgs tempResult = new RunWorkerCompletedEventArgs(result, er, ca);

      // prepare for next use
      IsBusy = false;
      CancellationPending = false;

      // return execution to client by going async here
      System.Threading.ThreadPool.QueueUserWorkItem(
        new System.Threading.WaitCallback(RealWorkHelper), tempResult);
    }

    // Async(ThreadPool) called by DoTheRealWork [to avoid any rentrancy issues at the client end]
    private void RealWorkHelper(object o)
    {
      if (RunWorkerCompleted != null)
      {
        m_guiMarshaller.BeginInvoke(new MethodInvoker(delegate()
        {
          RunWorkerCompleted(this, (RunWorkerCompletedEventArgs)o);
        }));
        Application.DoEvents();
      }
    }

    #endregion
  }

  #region Delegates for 3 events of class
  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public delegate void DoWorkEventHandler(object sender, DoWorkEventArgs e);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public delegate void RunWorkerCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e);
  #endregion
}

