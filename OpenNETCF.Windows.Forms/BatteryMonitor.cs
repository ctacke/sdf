using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Represents a component that monitors the battery level.
	/// </summary>
	/// <example>
	/// <code>
	/// [C#]
	/// public class Form1 : System.Windows.Forms.Form
	/// {
	///	private System.ComponentModel.IContainer components = null;
	///	private OpenNETCF.Windows.Forms.BatteryMonitor batteryMonitor1;
	///
	///	public Form1()
	///	{
	///		batteryMonitor1 = new OpenNETCF.Windows.Forms.BatteryMonitor(components);
	///		batteryMonitor1.PrimaryBatteryLifeTrigger = 75;
	///		batteryMonitor1.PrimaryBatteryLifeNotification += new System.EventHandler(batteryMonitor1_PrimaryBatteryLifeNotification);
	///		batteryMonitor1.Enabled = true;
	///	}
	///
	///	protected override void Dispose(bool disposing)
	///	{
	///		if ((disposing) &amp;&amp; (components != null))
	///		{
	///			components.Dispose();
	///		}
	///		base.Dispose(disposing);
	///	}
	///
	///	private void batteryMonitor1_PrimaryBatteryLifeNotification(object sender, EventArgs e)
	///	{
	///		// Do something here.
	///	}
	/// }
	/// </code>
	/// </example>
	public class BatteryMonitor : System.ComponentModel.Component
	{
		#region Fields ==============================================================================

		/// <summary>
		/// Specifies the default value for the PrimaryBatteryLifeTrigger property.
		/// </summary>
		public const int DefaultBatteryLifePercent = 50;

		private bool disposed = false;
		private System.Windows.Forms.Timer internalTimer = new System.Windows.Forms.Timer();

		// Property Correspondents
		private int primaryBatteryLifeTriggerValue = DefaultBatteryLifePercent;

		#endregion ==================================================================================

		#region Properties ==========================================================================

		/// <summary>
		/// Gets or sets a value that represents whether the power notification events will be raised.
		/// </summary>
		/// <value>A <see cref="T:System.Boolean" /> value that represents whether the power notification events will be raised. The default is <b>false</b>.</value>
		public bool Enabled
		{
			get
			{
				return internalTimer.Enabled;
			}
			set
			{
				if (internalTimer.Enabled != value)
				{
					internalTimer.Enabled = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that represents how often the system power is queried, in milliseconds.
		/// </summary>
		/// <value>A <see cref="T:System.Int32" /> value that represents how often the system power is queried, in milliseconds. The default is 1000 (1 second).</value>
		public int Interval
		{
			get
			{
				return internalTimer.Interval;
			}
			set
			{
				if (internalTimer.Interval != value)
				{
					internalTimer.Interval = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that represents the percentage at which the PrimaryBatteryLifeNotification event should be raised.
		/// </summary>
		/// <value>A <see cref="T:System.Int32" /> value that represents the percentage at which the PrimaryBatteryLifeNotification event should be raised. The default is the value of the DefaultBatteryLifePercent constant.</value>
		public int PrimaryBatteryLifeTrigger
		{
			get
			{
				return primaryBatteryLifeTriggerValue;
			}
			set
			{
				if ((value >= 0) && (value <= 100))
				{
					if (primaryBatteryLifeTriggerValue != value)
					{
						primaryBatteryLifeTriggerValue = value;
					}
				}
				else
				{
					throw (new System.ArgumentOutOfRangeException("Value", "The PrimaryBatteryLifeTrigger value must be between 0 and 100."));
				}
			}
		}

		#endregion ==================================================================================

		#region Methods =============================================================================

		/// <summary>
		/// Initializes a new instance of the BatteryMonitor class.
		/// </summary>
		public BatteryMonitor()
		{
			this.Enabled = false;
			this.Interval = 1000;
			this.internalTimer.Tick += new System.EventHandler(Timer_Tick);
		}

		/// <summary>
		/// Initializes a new instance of the BatteryMonitor class with the specified container.
		/// </summary>
		/// <param name="container">An IContainer that represents the container for the BatteryMonitor.</param>
		public BatteryMonitor(IContainer container) : this()
		{
			container.Add(this);
		}

		/// <summary>
		/// Allows an instance of the BatteryMonitor class to attempt to free resources and perform other cleanup operations.
		/// </summary>
		~BatteryMonitor()
		{
			Dispose(false);
		}

		/// <summary>
		/// Releases all resources used by the BatteryMonitor instance.
		/// </summary>
		public new void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases the unmanaged resources used by the BatteryMonitor instance and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					if (disposing)
					{
						// Dispose managed resources.
						this.internalTimer.Enabled = false;
						this.internalTimer.Tick -= new System.EventHandler(Timer_Tick);
						this.internalTimer.Dispose();
					}
					// Dispose unmanaged resources.
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		/// <summary>
		/// Raises the PrimaryBatteryLifeNotification event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnPrimaryBatteryLifeNotification(System.EventArgs e)
		{
			if (PrimaryBatteryLifeNotification != null)
			{
				PrimaryBatteryLifeNotification(this, e);
			}
		}

		/// <summary>
		/// The event handler used to determine if notifications should be sent to subscribers.
		/// </summary>
		private void Timer_Tick(object sender, System.EventArgs e)
		{
			// If the end-developer has requested to be notified, check the power status.
			if (PrimaryBatteryLifeNotification != null)
			{
				PowerStatus status = new PowerStatus();
				if (this.PrimaryBatteryLifeTrigger == status.BatteryLifePercent)
				{
					// Stop the internal timer when the trigger value has been satisfied. It will 
					// be the end-developers responsibility to enable the timer (via this components 
					// Enabled property) to request event notifications again.
					this.Enabled = false;
					OnPrimaryBatteryLifeNotification(EventArgs.Empty);
				}
			}
		}

		#endregion ==================================================================================

		#region Events ==============================================================================

		/// <summary>
		/// Occurs when the primary battery life percentage is equal to the value of the PrimaryBatteryLifeTrigger property.
		/// </summary>
		/// <remarks>
		/// The Enabled property will be set to <b>false</b> when this event is raised. This prevents 
		/// event handlers from being called multiple times while the trigger is satisfied. To receive 
		/// further power notifications the Enabled property must once again be set to <b>true</b>.
		/// </remarks>
		public event System.EventHandler PrimaryBatteryLifeNotification;

		#endregion ==================================================================================
	}
}