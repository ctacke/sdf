using System;

namespace OpenNETCF.Diagnostics{
	/// <summary>
	/// Provides a set of methods and properties that you can use to accurately measure elapsed time.
	/// </summary>
	public class Stopwatch {

		#region Public Statics
		/// <summary>
		/// Indicates whether the timer is based on a high-resolution performance counter. This field is read-only.
		/// </summary>
		public static readonly bool IsHighResolution = false;

		/// <summary>
		/// Gets the current number of ticks in the timer mechanism.
		/// </summary>
		/// <returns>A long integer representing the tick counter value of the underlying timer mechanism.</returns>
		public static Int64 GetTimestamp(){
			Int64 perfCount;
			Stopwatch.QueryPerformanceCounter(out perfCount);
			return perfCount;
		}
	
		/// <summary>
		/// Initializes a new Stopwatch instance, sets the elapsed time property to zero, and starts measuring elapsed time.
		/// </summary>
		/// <returns>A Stopwatch that has just begun measuring elapsed time.</returns>
		public static Stopwatch StartNew(){
			Stopwatch sw = new Stopwatch();
			sw.Start();
			return sw;
		}

		/// <summary>
		/// Gets the frequency of the timer as the number of ticks per second. This field is read-only.
		/// </summary>
		public static readonly Int64 Frequency;
		#endregion

		#region Public Interface
		/// <summary>
		/// Initializes a new instance of the Stopwatch class.
		/// </summary>
		public Stopwatch(){
			if (!Stopwatch.IsHighResolution) {
				throw new Exception("Stopwatch class. Try a device that supports high frequency timer. On current device use Environment.TickCount instead.");
			}
		}

		/// <summary>
		/// Stops time interval measurement and resets the elapsed time to zero.
		/// </summary>
		public void Reset(){
			mElapsed = 0;	//ticks
			mIsRunning = false;
			mStartPerfCount = 0;
		}

		/// <summary>
		/// Starts, or resumes, measuring elapsed time for an interval.
		/// </summary>
		public void Start(){
			if (mIsRunning) {
				return;
			}

			mStartPerfCount = Stopwatch.GetTimestamp();
			mIsRunning = true;
		}

		/// <summary>
		/// Gets a value indicating whether the Stopwatch timer is running.
		/// </summary>
		public bool IsRunning { 
			get{
				return mIsRunning;
			} 
		}

		/// <summary>
		/// Stops measuring elapsed time for an interval.
		/// </summary>
		public void Stop(){
			if (!mIsRunning) {
				return;
			}

			mElapsed = this.ElapsedTicks;
			mIsRunning = false;
		}

		/// <summary>
		/// Gets the total elapsed time measured by the current instance.
		/// </summary>
		public TimeSpan Elapsed { 
			get{
				return new TimeSpan(this.GetAdjustedTicks()); 
			}
		}

		/// <summary>
		/// Gets the total elapsed time measured by the current instance, in milliseconds.
		/// </summary>
		public Int64 ElapsedMilliseconds { 
			get{
				return (this.GetAdjustedTicks() / 10000);
			} 
		}

		/// <summary>
		/// Gets the total elapsed time measured by the current instance, in timer ticks.
		/// </summary>
		public Int64 ElapsedTicks { 
			get{
				if (! mIsRunning) {
					return mElapsed;
				}

				return (mElapsed + Stopwatch.GetTimestamp() - mStartPerfCount);
			} 
		}
		#endregion

		#region Privates
		static Stopwatch() {
			// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncfhowto/html/uperfcoun.asp
			Int64 freq;
			if (Stopwatch.QueryPerformanceFrequency(out freq) == 0) {
				Stopwatch.IsHighResolution = false;
				return;
			}			
			Stopwatch.IsHighResolution = true;
			Stopwatch.Frequency = freq;

			smFreqInTicks = (MILLIS_IN_TICKS * 1000) / freq;
		}
 
		private Int64 GetAdjustedTicks() {
			double temp = this.ElapsedTicks;
			temp *= Stopwatch.smFreqInTicks;
			return (Int64) Math.Round(temp);
		}
 

		private static readonly double smFreqInTicks;
		private Int64 mElapsed;
		private bool mIsRunning;
		private Int64 mStartPerfCount;
		private const Int64 MILLIS_IN_TICKS = 10000;
		#endregion

		#region PInvokes
		[System.Runtime.InteropServices.DllImport("coredll.dll")]
		internal static extern int QueryPerformanceCounter(out Int64 perfCounter);
		[System.Runtime.InteropServices.DllImport("coredll.dll")]
		internal static extern int QueryPerformanceFrequency(out Int64 frequency);
		#endregion
	}
}