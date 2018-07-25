using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Indicates current system power status information.
	/// </summary>
	public class PowerStatus
    {
#pragma warning disable 0169
#pragma warning disable 0414
        private byte acLineStatus = 0;
		private byte batteryFlag = 0;
		private byte batteryLifePercent = 0;
        private byte reserved1 = 0;
		private int batteryLifeTime = 0;
		private int batteryFullLifeTime = 0;
		private byte reserved2 = 0;
		private byte backupBatteryFlag = 0;
		private byte backupBatteryLifePercent = 0;
		private byte reserved3 = 0;
		private int backupBatteryLifeTime = 0;
		private int backupBatteryFullLifeTime = 0;

        private int batteryVoltage = 0;
        private int batteryCurrent = 0;
        private int batteryAverageCurrent = 0;
        private int batteryAverageInterval = 0;
        private int batterymAHourConsumed = 0;
        private int batteryTemperature = 0;
        private int backupBatteryVoltage = 0;
        private byte batteryChemistry = 0;
        private byte reserved4 = 0;
        private byte reserved5 = 0;
        private byte reserved6 = 0;
#pragma warning restore 0414
#pragma warning restore 0169

        internal PowerStatus(){}

		/// <summary>
		/// AC power status.
		/// </summary>
        /// <value>One of the <see cref="PowerLineStatus"/> values indicating the current system power status.</value>
		public PowerLineStatus PowerLineStatus
		{
			get
			{
				Update();
				return (PowerLineStatus)acLineStatus;
			}
		}

		/// <summary>
		/// Gets the current battery charge status.
		/// </summary>
        /// <value>One of the <see cref="BatteryChargeStatus"/> values indicating the current battery charge level or charging status.</value>
		public BatteryChargeStatus BatteryChargeStatus
		{
			get
			{
				Update();
				return (BatteryChargeStatus)batteryFlag;
			}
		}
		/// <summary>
		/// Gets the approximate percentage of full battery time remaining.
		/// </summary>
        /// <value>The approximate percentage, from 0 to 100, of full battery time remaining, or 255 if the percentage is unknown.</value>
		public byte BatteryLifePercent
		{
			get
			{
				Update();
				return batteryLifePercent;
			}
		}

		/// <summary>
		/// Gets the approximate number of seconds of battery time remaining.
		/// </summary>
		/// <value>The approximate number of seconds of battery life remaining, or -1 if the approximate remaining battery life is unknown.</value>
		public int BatteryLifeRemaining
		{
			get
			{
				Update();
				return batteryLifeTime;
			}
		}

		/// <summary>
		/// Gets the reported full charge lifetime of the primary battery power source in seconds.
		/// </summary>
		/// <value>The reported number of seconds of battery life available when the battery is fullly charged, or -1 if the battery life is unknown.</value>
		public int BatteryFullLifeTime
		{
			get
			{
				Update();
				return batteryFullLifeTime;
			}
		}

		/// <summary>
		/// Gets the backup battery charge status.
		/// </summary>
		public BatteryChargeStatus BackupBatteryChargeStatus
		{
			get
			{
				Update();
				return (BatteryChargeStatus)backupBatteryFlag;
			}
		}

		/// <summary>
		/// Percentage of full backup battery charge remaining. Must be in the range 0 to 100.
		/// </summary>
		public byte BackupBatteryLifePercent
		{
			get
			{
				Update();
				return backupBatteryLifePercent;
			}
		}
		
		/// <summary>
		/// Number of seconds of backup battery life remaining.
		/// </summary>
		public int BackupBatteryLifeRemaining
		{
			get
			{
				Update();
				return backupBatteryLifeTime;
			}
		}
		/// <summary>
		/// Number of seconds of backup battery life when at full charge. Or -1 If unknown.
		/// </summary>
		public int BackupBatteryFullLifeTime
		{
			get
			{
				Update();
				return backupBatteryFullLifeTime;
			}
		}

        /// <summary>
        /// Amount of battery voltage in millivolts (mV).
        /// </summary>
        public int BatteryVoltage
        {
            get
            {
                Update();
                return batteryVoltage;
            }
        }

        /// <summary>
        /// Amount of instantaneous current drain in milliamperes (mA).
        /// </summary>
        public int BatteryCurrent
        {
            get
            {
                Update();
                return batteryCurrent;
            }
        }

        /// <summary>
        /// Short-term average of device current drain (mA).
        /// </summary>
        public int BatteryAverageCurrent
        {
            get
            {
                Update();
                return batteryAverageCurrent;
            }
        }

        /// <summary>
        /// Time constant in milliseconds (ms) of integration used in reporting <see cref="BatteryAverageCurrent"/>.
        /// </summary>
        public int BatteryAverageInterval
        {
            get
            {
                Update();
                return batteryAverageInterval;
            }
        }

        /// <summary>
        /// Long-term cumulative average discharge in milliamperes per hour (mAH).
        /// </summary>
        public int BatterymAHourComsumed
        {
            get
            {
                Update();
                return batterymAHourConsumed;
            }
        }

        /// <summary>
        /// Battery temperature in degrees Celsius (°C).
        /// </summary>
        public int BatteryTemperature
        {
            get
            {
                Update();
                return batteryTemperature;
            }
        }

        /// <summary>
        /// Backup battery voltage in mV.
        /// </summary>
        public int BackupBatteryVoltage
        {
            get
            {
                Update();
                return backupBatteryVoltage;
            }
        }

        /// <summary>
        /// Chemistry of the devices main battery.
        /// </summary>
        public BatteryChemistry BatteryChemistry
        {
            get
            {
                Update();
                return (BatteryChemistry)batteryChemistry;
            }
        }

		private void Update()
		{
            //updated to use GetSystemPowerStatusEx2 for additional properties
			bool success = NativeMethods.GetSystemPowerStatusEx2(this, 56, false);
		}

		
	}

	
}
