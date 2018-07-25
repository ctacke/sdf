using System;
using System.Threading;
using System.Collections;
using System.Text;

using OpenNETCF.Threading;

using EventWaitHandle = OpenNETCF.Threading.EventWaitHandle;
using EventResetMode = OpenNETCF.Threading.EventResetMode;

namespace OpenNETCF.Rss
{
	/// <summary>
	/// Implements a thread worker class.
	/// </summary>
	internal class FeedWorker
	{
		#region fields

		private static int checkInterval;
		private bool stop;
		private ReceiveHandler receiveCallback;
		private Thread thread;
		private bool started = false;
//#if !CF
//			private AutoResetEvent resetEvent;
//#else
			private EventWaitHandle resetEvent;
//#endif
		private ArrayList feedList; 
		private Timer timer;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the FeedWorker.
		/// </summary>
		/// <param name="receiveCallback">A ReceiveHandler callback.</param>
		/// <param name="checkInterval">An interval (milliseconds) to start receiving RSS feeds.</param>
		public FeedWorker(ReceiveHandler receiveCallback, int checkInterval)
		{
			this.receiveCallback = receiveCallback;
			FeedWorker.checkInterval = checkInterval;
			this.stop = false;
//#if !CF
//				resetEvent = new AutoResetEvent(false);
//#else
				resetEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
				
//#endif
			feedList = new ArrayList();
			timer = new Timer(new TimerCallback(TimerCall), resetEvent, Timeout.Infinite, Timeout.Infinite);
		} 

		#endregion


		#region public methods

		/// <summary>
		/// Starts receiving process.
		/// </summary>
		public void Start()
		{
			ThreadStart threadStart = new ThreadStart(WorkerProcess);
			thread = new Thread(threadStart);
            thread.IsBackground = true;
            thread.Name = "SDF AdapterStatusMonitor";
            thread.Start();
			//timer.Change(0, checkInterval);
			started = true;
		}

		/// <summary>
		/// Adds RSS feed destination to the collection.
		/// </summary>
		/// <param name="destination"></param>
		public void AddFeed(Uri destination)
		{
			feedList.Add(destination);
		}

		/// <summary>
		/// Stops feed retreival process.
		/// </summary>
		public void Stop()
		{
			try
			{
				this.stop = true;
				timer.Change(Timeout.Infinite, Timeout.Infinite);
				resetEvent.Set();
				//thread.Abort();
				//thread.Join();
				started = false;
			}
			catch { };

		}
		
		#endregion
	
		private void TimerCall(object state)
		{
			resetEvent.Set();
		}


		#region helper methods

		private void WorkerProcess()
		{
			try
			{
				while (!stop)
				{
					foreach (Uri destination in feedList)
					{
						receiveCallback(destination);
					}
					resetEvent.WaitOne(checkInterval, false);

					//resetEvent.WaitOne();
				}
			}
			catch (Exception abortException)
			{
				Console.WriteLine((string)abortException.Message);
			}

//			catch (ThreadAbortException abortException)
//			{
//				Console.WriteLine((string)abortException.ExceptionState);
//			}

		} 

		#endregion

		#region properties

		public bool Started
		{
			get
			{
				return started;
			}
		} 

		#endregion
	}
}
