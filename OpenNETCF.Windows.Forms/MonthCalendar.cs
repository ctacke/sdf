//==========================================================================================
//
//		OpenNETCF.Windows.Forms.MonthCalendar
//		Copyright (c) 2004, OpenNETCF.org
//
//		This library is free software; you can redistribute it and/or modify it under 
//		the terms of the OpenNETCF.org Shared Source License.
//
//		This library is distributed in the hope that it will be useful, but 
//		WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
//		FITNESS FOR A PARTICULAR PURPOSE. See the OpenNETCF.org Shared Source License 
//		for more details.
//
//		You should have received a copy of the OpenNETCF.org Shared Source License 
//		along with this library; if not, email licensing@opennetcf.org to request a copy.
//
//		If you wish to contact the OpenNETCF Advisory Board to discuss licensing, please 
//		email licensing@opennetcf.org.
//
//		For general enquiries, email enquiries@opennetcf.org or visit our website at:
//		http://www.opennetcf.org
//
//==========================================================================================
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;


#if !DESIGN
using OpenNETCF.Drawing;
using OpenNETCF.Win32;
using OpenNETCF.Windows.Forms; 
using OpenNETCF.Runtime.InteropServices;
#if !NDOC
using Microsoft.WindowsCE.Forms;
#endif
#endif


namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Represents a standard Windows month calendar control.
	/// <para><b>New in v1.1</b></para>
	/// </summary>
#if DESIGN
	[CLSCompliant(false),
	DefaultEvent("DateChanged")]
	public class MonthCalendar : System.Windows.Forms.MonthCalendar
	{

		private OpenNETCF.Windows.Forms.Day m_day = OpenNETCF.Windows.Forms.Day.Default;

	//hide bolded dates properties (not supported in this version)

	[Browsable(false)]
	public new DateTime[] AnnuallyBoldedDates
	{
		get
		{
			return new DateTime[0]{};
		}
		set
		{
		}
	}

	[Browsable(false)]
	public new OpenNETCF.Windows.Forms.SelectionRange SelectionRange
	{
		get
		{
			return new SelectionRange();
		}
		set
		{
		}
	}

	[Browsable(false)]
	public new DateTime[] BoldedDates
	{
		get
		{
			return new DateTime[0]{};
		}
		set
		{
		}
	}

	[Browsable(false)]
	public new DateTime[] MonthlyBoldedDates
	{
		get
		{
			return new DateTime[0]{};
		}
		set
		{
		}
	}

	//fix at one month visible

	protected override void OnResize(EventArgs e)
	{
		//ensure size is kept to that of a single month control
		this.Size = this.SingleMonthSize;

		base.OnResize(e);
	}

	[Browsable(false)]
	public new Size CalendarDimensions
	{
		get
		{
			return new Size(1,1);
		}
		set
		{
		}
	}

	// replace the events to use our types

	[Category("Action")]
	public new event OpenNETCF.Windows.Forms.DateRangeEventHandler DateChanged;

	[Category("Action")]
	public new event OpenNETCF.Windows.Forms.DateRangeEventHandler DateSelected;

	//convert dayofweek to support designer

	[Category("Behavior")]
	public new OpenNETCF.Windows.Forms.Day FirstDayOfWeek
	{
		get
		{
			return m_day;
		}
		set
		{
			m_day = value;
			switch(value)
			{
				case OpenNETCF.Windows.Forms.Day.Default:
					base.FirstDayOfWeek = System.Windows.Forms.Day.Default;
					break;
				case OpenNETCF.Windows.Forms.Day.Monday:
					base.FirstDayOfWeek = System.Windows.Forms.Day.Monday;
					break;
				case OpenNETCF.Windows.Forms.Day.Tuesday:
					base.FirstDayOfWeek = System.Windows.Forms.Day.Tuesday;
					break;
				case OpenNETCF.Windows.Forms.Day.Wednesday:
					base.FirstDayOfWeek = System.Windows.Forms.Day.Wednesday;
					break;
				case OpenNETCF.Windows.Forms.Day.Thursday:
					base.FirstDayOfWeek = System.Windows.Forms.Day.Thursday;
					break;
				case OpenNETCF.Windows.Forms.Day.Friday:
					base.FirstDayOfWeek = System.Windows.Forms.Day.Friday;
					break;
				case OpenNETCF.Windows.Forms.Day.Saturday:
					base.FirstDayOfWeek = System.Windows.Forms.Day.Saturday;
					break;
				case OpenNETCF.Windows.Forms.Day.Sunday:
					base.FirstDayOfWeek = System.Windows.Forms.Day.Sunday;
					break;
			}
		}
	}

	

	}
#else
	public class MonthCalendar : ControlEx
	{
		//minimum supported date
		private DateTime mindate = new DateTime(1753,1,1);

		private SelectionRange m_maxrange;
		private SelectionRange m_selrange;

#if !DESIGN	
		static MonthCalendar()
		{
			//initialise time classes
			bool success = Win32Window.InitCommonControlsEx(new byte[] {8,0,0,0,0,1,0,0});
		}
#endif

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="MonthCalendar"/> class.
		/// </summary>
		public MonthCalendar() : base(true)
		{
			m_maxrange = new SelectionRange();
			m_selrange = new SelectionRange();
		}
		#endregion

#if !DESIGN
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ClassName = "SysMonthCal32";
				cp.ClassStyle = cp.ClassStyle  | (int)MCS.MULTISELECT;
				return cp;

			}
		}
#endif


		#region INotifiable Members
#if!NDOC
		//processes incoming messages
		protected override void OnNotifyMessage(ref Microsoft.WindowsCE.Forms.Message m)
		{
#if !DESIGN
			//check message is a notification
			if(m.Msg == (int)WM.NOTIFY)
			{
				//marshal notification data into NMHDR struct
				NMHDR hdr = (NMHDR)Marshal.PtrToStructure(m.LParam, typeof(NMHDR));

				switch(hdr.code)
				{
						//definite selection
					case (int)MCN.SELECT:
						//copy date range
						SystemTime stStart = new SystemTime();
						Marshal.Copy((IntPtr)((int)m.LParam +12), stStart.ToByteArray(), 0, stStart.ToByteArray().Length);
						SystemTime stEnd = new SystemTime();
						Marshal.Copy((IntPtr)((int)m.LParam +28), stEnd.ToByteArray(), 0, stEnd.ToByteArray().Length);
						//raise datechanged event
						OnDateSelected(new DateRangeEventArgs(stStart.ToDateTime(), stEnd.ToDateTime()));
						break;
						//change in selection
					case (int)MCN.SELCHANGE:
						//copy date range
						SystemTime stscStart = new SystemTime();
						Marshal.Copy((IntPtr)((int)m.LParam +12), stscStart.ToByteArray(), 0, stscStart.ToByteArray().Length);
						SystemTime stscEnd = new SystemTime();
						Marshal.Copy((IntPtr)((int)m.LParam +28), stscEnd.ToByteArray(), 0, stscEnd.ToByteArray().Length);
						//raise datechanged event
						OnDateChanged(new DateRangeEventArgs(stscStart.ToDateTime(), stscEnd.ToDateTime()));
						break;

						//user selected none
					case (int)MCN.SELECTNONE:
						//raise the NoneSelected event
						OnNoneSelected(new EventArgs());
						break;

						//control requires more daystate information for bold items
						//case (int)MCN.GETDAYSTATE:
						//	break;
				}
#endif
			}

			base.OnNotifyMessage (ref m);
		}
#endif
		#endregion


		#region Color Properties

		#region Back Color
		/// <summary>
		/// Gets or sets the background color of the control.
		/// </summary>
#if DESIGN
		[Category("Appearance"),
		Description("The background color displayed within the month.")]
#endif
		public override Color BackColor
		{
			get
			{
#if !DESIGN
				int colorref = (int)Win32Window.SendMessage(this.ChildHandle, (int)MCM.GETCOLOR, (int)MCSC.MONTHBK, 0);
				return ColorTranslator.FromWin32(colorref);
#else
				return Color.Black;
#endif
			}
			set
			{
#if !DESIGN
				int colorref = ColorTranslator.ToWin32(value);
				Win32Window.SendMessage(this.ChildHandle, (int)MCM.SETCOLOR, (int)MCSC.MONTHBK, colorref);
#endif
			}
		}
		#endregion

		#region Fore Color
		/// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
#if DESIGN
		[Category("Appearance"),
		Description("The color used to display text within the month.")]
#endif
		public override Color ForeColor
		{
			get
			{
#if !DESIGN
				int colorref = (int)Win32Window.SendMessage(this.ChildHandle, (int)MCM.GETCOLOR, (int)MCSC.TEXT, 0);
				return ColorTranslator.FromWin32(colorref);
#else
				return Color.Black;
#endif
			}
			set
			{
#if !DESIGN
				int colorref = ColorTranslator.ToWin32(value);
				Win32Window.SendMessage(this.ChildHandle, (int)MCM.SETCOLOR, (int)MCSC.TEXT, colorref);
#endif
			}
		}
		#endregion

		#region Title Back Color
		/// <summary>
		/// Gets or sets a value indicating the background color of the title area of the calendar.
		/// </summary>
#if DESIGN
		[Category("Appearance"),
		Description("The background color displayed in the calendar's title.")]
#endif
		public Color TitleBackColor
		{
			get
			{
#if !DESIGN
				int colorref = (int)Win32Window.SendMessage(this.ChildHandle, (int)MCM.GETCOLOR, (int)MCSC.TITLEBK, 0);
				return ColorTranslator.FromWin32(colorref);
#else
				return Color.Black;
#endif
			}
			set
			{
#if !DESIGN
				int colorref = ColorTranslator.ToWin32(value);
				Win32Window.SendMessage(this.ChildHandle, (int)MCM.SETCOLOR, (int)MCSC.TITLEBK, colorref);
#endif
			}
		}
		#endregion

		#region Title Fore Color
		/// <summary>
		/// Gets or sets a value indicating the foreground color of the title area of the calendar.
		/// </summary>
#if DESIGN
		[Category("Appearance"),
		Description("The color used to display text within the calendar's title.")]
#endif
		public Color TitleForeColor
		{
			get
			{
#if !DESIGN
				int colorref = (int)Win32Window.SendMessage(this.ChildHandle, (int)MCM.GETCOLOR, (int)MCSC.TITLETEXT, 0);
				return ColorTranslator.FromWin32(colorref);
#else
				return Color.Black;
#endif
			}
			set
			{
#if !DESIGN
				int colorref = ColorTranslator.ToWin32(value);
				Win32Window.SendMessage(this.ChildHandle, (int)MCM.SETCOLOR, (int)MCSC.TITLETEXT, colorref);
#endif
			}
		}
		#endregion

		#region Trailing Fore Color
		/// <summary>
		/// Gets or sets a value indicating the color of days in months that are not fully displayed in the control.
		/// </summary>
#if DESIGN
		[Category("Appearance"),
		Description("The color used to display the previous and following months that appear on the month calendar.")]
#endif
		public Color TrailingForeColor
		{
			get
			{
#if !DESIGN
				int colorref = (int)Win32Window.SendMessage(this.ChildHandle, (int)MCM.GETCOLOR, (int)MCSC.TRAILINGTEXT, 0);
				return ColorTranslator.FromWin32(colorref);
#else
				return Color.Black;
#endif
			}
			set
			{
#if !DESIGN
				int colorref = ColorTranslator.ToWin32(value);
				Win32Window.SendMessage(this.ChildHandle, (int)MCM.SETCOLOR, (int)MCSC.TRAILINGTEXT, colorref);
#endif
			}
		}
		#endregion

		#endregion


		#region First Day Of Week
		/// <summary>
		/// Gets or sets the first day of the week as displayed in the month calendar.
		/// </summary>
#if DESIGN
		[Category("Behavior"),
		Description("The first day of the week.")]
#endif
		public Day FirstDayOfWeek
		{
			get
			{
#if !DESIGN
				int result = (int)Win32Window.SendMessage(this.ChildHandle, (int)MCM.GETFIRSTDAYOFWEEK, 0, 0);
				return (Day)BitConverter.ToInt16(BitConverter.GetBytes(result),0);
#else
				return Day.Default;
#endif
			}
			set
			{
#if !DESIGN
				Win32Window.SendMessage(this.ChildHandle, (int)MCM.SETFIRSTDAYOFWEEK, 0, (short)value);
#endif
			}
		}
		#endregion

		

		#region Hit Test
		/// <summary>
		///  Returns <see cref="T:OpenNETCF.Windows.Forms.MonthCalendar.HitTestInfo"/> with information on which portion of a month calendar control is at a specified x and y location.
		/// </summary>
		/// <param name="x">The <see cref="P:System.Drawing.Point.X"/> coordinate of the point to be hit-tested.</param>
		/// <param name="y">The <see cref="P:System.Drawing.Point.Y"/> coordinate of the point to be hit-tested.</param>
		/// <returns>A System.Windows.Forms.MonthCalendar.HitTestInfo that contains information about the specified point on the <see cref="T:OpenNETCF.Windows.Forms.MonthCalendar"/>.</returns>
		public HitTestInfo HitTest(int x, int y)
		{

			HitTestInfo hti = new HitTestInfo();
#if !DESIGN	
			hti.X = x;
			hti.Y = y;
			byte[] data = hti.ToByteArray();
			//marshal to native memory
			IntPtr ptr = MarshalEx.AllocHGlobal(data.Length);
			Marshal.Copy(data, 0, ptr,data.Length);
			
			Win32Window.SendMessage(this.ChildHandle, (int)MCM.HITTEST, 0, (int)ptr);

			//marshal results back
			Marshal.Copy(ptr, data, 0, data.Length);
			MarshalEx.FreeHGlobal(ptr);
#endif
			return hti;
		}
		/// <summary>
		/// Returns an object with information on which portion of a month calendar control is at a location specified by <see cref="T:System.Drawing.Point"/>.
		/// </summary>
		/// <param name="point">A <see cref="T:System.Drawing.Point"/> containing the <see cref="P:System.Drawing.Point.X"/> and <see cref="P:System.Drawing.Point.Y"/> coordinates of the point to be hit-tested.</param>
		/// <returns>A System.Windows.Forms.MonthCalendar.HitTestInfo that contains information about the specified point on the <see cref="T:OpenNETCF.Windows.Forms.MonthCalendar"/>.</returns>
		public HitTestInfo HitTest(Point point)
		{
			return HitTest(point.X, point.Y);
		}
		#endregion

		#region Maximum Date
		/// <summary>
		/// Gets or sets the maximum allowable date.
		/// </summary>
#if DESIGN
		[Category("Behavior"),
		Description("The maximum date that can be selected for a month calendar control.")]
#endif
		public DateTime MaxDate
		{
			get
			{
				GetMaxRange();
				return m_maxrange.End;
			}
			set
			{
				m_maxrange.End = value;
				//send range to native control
				SetMaxRange();
			}
		}
		#endregion

		#region Maximum Selection Count
		/// <summary>
		/// Gets or sets the maximum number of days that can be selected in a month calendar control.
		/// </summary>
#if DESIGN
		[Category("Behavior"),
		Description("The total number of days that can be selected for the control.")]
#endif
		public int MaxSelectionCount
		{
			get
			{
#if !DESIGN
				return (int)Win32Window.SendMessage(this.ChildHandle, (int)MCM.GETMAXSELCOUNT, 0, 0);
#else
				return 0;
#endif
			}
			set
			{
#if !DESIGN
				Win32Window.SendMessage(this.ChildHandle, (int)MCM.SETMAXSELCOUNT, 0, value);
#endif
			}
		}
		#endregion

		#region Minimum Date
		/// <summary>
		/// Gets or sets the minimum allowable date.
		/// </summary>
		/// <exception cref="T:System.ArgumentException">The date set is greater than the <see cref="P:OpenNETCF.Windows.Forms.MonthCalendar.MaxDate"/>.</exception>
		/// <exception cref="T:System.ArgumentException">The date set is earlier than 01/01/1753.</exception>
#if DESIGN
		[Category("Behavior"),
		Description("The minimum date that can be selected for a month calendar control.")]
#endif
		public DateTime MinDate
		{
			get
			{
				GetMaxRange();
				return m_maxrange.Start;
			}
			set
			{
				if(value > mindate)
				{
					//if(value.CompareTo(this.MaxDate) <= 0)
					//{
						m_maxrange.Start = value;
						//send range to native control
						SetMaxRange();
					//}
					//else
					//{
					//	throw new ArgumentException("The MinDate set is greater than the MaxDate");
					//}
				}
				else
				{
					throw new ArgumentException("The minimum date set is earlier than 01/01/1753");
				}
			}
		}
		#endregion


		#region Scroll Change
		/// <summary>
		/// Gets or sets the scroll rate for a month calendar control.  
		/// </summary>
		/// <exception cref="T:System.ArgumentException">The value is less than zero.  -or- The value is greater than 20,000.</exception>
#if DESIGN
		[Category("Behavior"),
		Description("The number of months one click on a next/prev button moves by.")]
#endif
		public int ScrollChange
		{
			get
			{
#if !DESIGN
				return (int)Win32Window.SendMessage(this.ChildHandle, (int)MCM.GETMONTHDELTA, 0, 0);
#else
				return 0;
#endif
			}
			set
			{
#if !DESIGN
				if(value >=0 & value <= 20000)
				{
					Win32Window.SendMessage(this.ChildHandle, (int)MCM.SETMONTHDELTA, value, 0); 
				}
				else
				{
					throw new ArgumentException("Value is out of ScrollChange limits");
				}
#endif
			}
		}
		#endregion


		#region Selection Start
		/// <summary>
		/// Gets or sets the start date of the selected range of dates.
		/// </summary>
#if DESIGN
		[Browsable(false)]
#endif
		public DateTime SelectionStart
		{
			get
			{
				//get latest values from control
				GetSelRange();
				//return start date
				return m_selrange.Start;
			}
			set
			{
				//get current values from control
				GetSelRange();
				//amend start date
				m_selrange.Start = value;
				//set changes
				SetSelRange();
			}
		}
		#endregion

		#region Selection Range
		/// <summary>
		/// Gets or sets the selected range of dates for a month calendar control.
		/// </summary>
#if DESIGN
		[Category("Behavior"),
		Description("The range of dates selected in the month calendar control.")]
#endif
		public SelectionRange SelectionRange
		{
			get
			{
				//get current selection range
				GetSelRange();

				return m_selrange;
			}
			set
			{
				m_selrange = value;

				SetSelRange();
			}
		}
		#endregion

		#region Selection End
		/// <summary>
		/// Gets or sets the end date of the selected range of dates.
		/// </summary>
#if DESIGN
		[Browsable(false)]
#endif
		public DateTime SelectionEnd
		{
			get
			{
				//get latest values from control
				GetSelRange();
				//return end date
				return m_selrange.End;
			}
			set
			{
				//get current values from control
				GetSelRange();
				//amend end date
				m_selrange.End = value;
				//set changes
				SetSelRange();
			}
		}
		#endregion

		
		#region Set Date
		/// <summary>
		/// Sets a date as the current selected date.
		/// </summary>
		/// <param name="date">The date to be selected.</param>
		/// <exception cref="T:System.ArgumentException">The value is less than the minimum allowable date.
		/// -or- The value is greater than the maximum allowable date.</exception>
		public void SetDate(DateTime date)
		{
			SelectionRange range = new SelectionRange(date, date);
#if !DESIGN
			byte[] data = range.ToByteArray();
			//marshal data to native memory
			IntPtr ptr = MarshalEx.AllocHGlobal(data.Length);
			Marshal.Copy(data, 0, ptr, data.Length);
			//send to control
			Win32Window.SendMessage(this.ChildHandle, (int)MCM.SETSELRANGE, 0, (int)ptr);
			//free native memory
			MarshalEx.FreeHGlobal(ptr);
#endif
		}
		#endregion


		#region Show None
		/// <summary>
		/// Gets or sets whether the None button is displayed
		/// </summary>
#if DESIGN
		[Category("Behavior"),
		Description("Indicates whether the month calendar will display the \"none\" option.")]
#endif
		public bool ShowNone
		{
			get
			{
#if !DESIGN
				return (Win32Window.GetWindowLong(this.ChildHandle, GWL.STYLE) & (int)MCS.SHOWNONE) == (int)MCS.SHOWNONE;
#else
				return false;
#endif
			}
			set
			{
#if !DESIGN
				if(value)
				{
					Win32Window.UpdateWindowStyle(this.ChildHandle, 0, (int)MCS.SHOWNONE);
				}
				else
				{
					Win32Window.UpdateWindowStyle(this.ChildHandle, (int)MCS.SHOWNONE, 0);
				}
#endif
			}
		}
		#endregion

		#region Show Today
		/// <summary>
		/// Gets or sets a value indicating whether the date represented by the <see cref="P:OpenNETCF.Windows.Forms.MonthCalendar.TodayDate"/> property is displayed at the bottom of the control.
		/// </summary>
#if DESIGN
		[Category("Behavior"),
		Description("Indicates whether the month calendar will display the \"today\" date at the bottom of the control.")]
#endif
		public bool ShowToday
		{
			get
			{
#if !DESIGN
				return (Win32Window.GetWindowLong(this.ChildHandle, GWL.STYLE) & (int)MCS.NOTODAY) != (int)MCS.NOTODAY;
#else
				return false;
#endif
			}
			set
			{
#if !DESIGN
				if(value)
				{
					Win32Window.UpdateWindowStyle(this.ChildHandle,(int)MCS.NOTODAY, 0);
				}
				else
				{
					Win32Window.UpdateWindowStyle(this.ChildHandle, 0, (int)MCS.NOTODAY);
				}
#endif
			}
		}
		#endregion

		#region Show Today Circle
		/// <summary>
		/// Gets or sets a value indicating whether today's date is circled.
		/// </summary>
#if DESIGN
		[Category("Behavior"),
		Description("Indicates whether the month calendar will circle the \"today\" date.")]
#endif
		public bool ShowTodayCircle
		{
			get
			{
#if !DESIGN
				return (Win32Window.GetWindowLong(this.ChildHandle, GWL.STYLE) & (int)MCS.NOTODAYCIRCLE) != (int)MCS.NOTODAYCIRCLE;
#else
				return false;
#endif
			}
			set
			{
#if !DESIGN
				if(value)
				{
					Win32Window.UpdateWindowStyle(this.ChildHandle,(int)MCS.NOTODAYCIRCLE, 0);
				}
				else
				{
					Win32Window.UpdateWindowStyle(this.ChildHandle, 0, (int)MCS.NOTODAYCIRCLE);
				}
#endif
			}
		}
		#endregion

		#region Show Week Numbers
		/// <summary>
		/// Gets or sets a value indicating whether the month calendar control displays week numbers (1-52) to the left of each row of days.
		/// </summary>
#if DESIGN
		[Category("Behavior"),
		Description("Indicates whether the month calendar will display week Numbers (1-52) to the left of each row of days.")]
#endif
		public bool ShowWeekNumbers
		{
			get
			{
#if !DESIGN
				return (Win32Window.GetWindowLong(this.ChildHandle, GWL.STYLE) & (int)MCS.WEEKNUMBERS) == (int)MCS.WEEKNUMBERS;
#else
				return false;
#endif
			}
			set
			{
#if !DESIGN
				if(value)
				{
					Win32Window.UpdateWindowStyle(this.ChildHandle, 0, (int)MCS.WEEKNUMBERS);
				}
				else
				{
					Win32Window.UpdateWindowStyle(this.ChildHandle, (int)MCS.WEEKNUMBERS, 0);
				}
#endif
			}
		}
		#endregion

		#region Single Month Size
		/// <summary>
		/// Gets the minimum size to display one month of the calendar.
		/// </summary>
#if DESIGN
		[Browsable(false)]
#endif
		public Size SingleMonthSize
		{
			get
			{
#if !DESIGN
				//alloc memory for RECT
				IntPtr ptr = MarshalEx.AllocHGlobal(16);
				//fill RECT
				Win32Window.SendMessage(this.ChildHandle, (int)MCM.GETMINREQRECT, 0, (int)ptr);

				int left = Marshal.ReadInt16(ptr, 0);
				int top = Marshal.ReadInt16(ptr, 4);
				int right = Marshal.ReadInt16(ptr, 8);
				int bottom = Marshal.ReadInt16(ptr, 12);
				//free native memory
				MarshalEx.FreeHGlobal(ptr);

				return new Size(right-left, bottom-top);
#else
				return new Size(0,0);
#endif
			}
		}
		#endregion

		

		#region Today Date
		/// <summary>
		/// Gets or sets the value that is used by <see cref="T:OpenNETCF.Windows.Forms.MonthCalendar"/> as today's date.
		/// </summary>
		public DateTime TodayDate
		{
			get
			{
#if !DESIGN
				//allocate memory for a SystemTime
				SystemTime st = new SystemTime();
				//allocate native memory
				IntPtr ptr = MarshalEx.AllocHGlobal(st.ToByteArray().Length);
				//send message to control
				Win32Window.SendMessage(this.ChildHandle, (int)MCM.GETTODAY, 0, (int)ptr);
				//copy data to managed array
				Marshal.Copy(ptr, st.ToByteArray(), 0, st.ToByteArray().Length);
				//free native memory
				MarshalEx.FreeHGlobal(ptr);
				//return datetime
				return st.ToDateTime();
#else
				return DateTime.Today;
#endif
			}
			set
			{
#if !DESIGN
				SystemTime st = new SystemTime(value);
				//marshal to native memory
				IntPtr ptr = MarshalEx.AllocHGlobal(st.ToByteArray().Length);
				Marshal.Copy(st.ToByteArray(), 0, ptr, st.ToByteArray().Length);

				//send message
				Win32Window.SendMessage(this.ChildHandle, (int)MCM.SETTODAY, 0, (int)ptr);

				//free native memory
				MarshalEx.FreeHGlobal(ptr);
#endif
			}
		}
		#endregion

		#region Unsupported Properties
		/// <summary>
		/// Gets or sets the array of <see cref="DateTime"/> objects that determines which annual days are displayed in bold.
		/// </summary>
		/// <value>An array of <see cref="DateTime"/> objects.</value>
		/// <remarks>Provided for compatibility only, this property always returns an empty collection.</remarks>
		public System.DateTime[] AnnuallyBoldedDates
		{
			get
			{
				return new DateTime[0]{};
			}
			set
			{}
		}

		/// <summary>
		/// Gets or sets the array of <see cref="DateTime"/> objects that determines which nonrecurring dates are displayed in bold.
		/// </summary>
		/// <value>The array of bolded dates.</value>
		/// <remarks>Provided for compatibility only, this property always returns an empty collection.</remarks>
		public System.DateTime[] BoldedDates
		{
			get
			{
				return new DateTime[0]{};
			}
			set
			{}
		}

		/// <summary>
		/// Gets or sets the array of <see cref="DateTime"/> objects that determine which monthly days to bold.
		/// </summary>
		/// <value>An array of <see cref="DateTime"/> objects.</value>
		/// <remarks>Provided for compatibility only, this property always returns an empty collection.</remarks>
		public System.DateTime[] MonthlyBoldedDates
		{
			get
			{
				return new DateTime[0]{};
			}
			set
			{}
		}

		/// <summary>
		/// Gets or sets the number of columns and rows of months displayed.
		/// </summary>
		/// <value>A <see cref="Size"/> with the number of columns and rows to use to display the calendar.</value>
		/// <remarks>Provided for compatibility, in current version this property always returns 1x1.</remarks>
		public Size CalendarDimensions
		{
			get
			{
				return new Size(1,1);
			}
			set
			{}
		}
		#endregion
		

		#region Resize
#if !DESIGN
		protected override void OnResize(EventArgs e)
		{
			//ensure size is kept to that of a single month control - otherwise blank screen areas
			this.Size = this.SingleMonthSize;

			base.OnResize (e);
		}
#endif
		#endregion


		#region Events

		/// <summary>
		/// Occurs when the user makes an explicit date selection using the mouse.
		/// </summary>
		public event DateRangeEventHandler DateSelected;

		/// <summary>
		/// Raises <see cref="DateSelected"/> event.
		/// </summary>
		/// <param name="drevent">A <see cref="DateRangeEventArgs"/> that contains the event data.</param>
		protected virtual void OnDateSelected(DateRangeEventArgs drevent)
		{
			//only raise if there is a subscriber
			if(this.DateSelected!=null)
			{
				this.DateSelected(this, drevent);
			}
		}

		/// <summary>
		/// Occurs when the date selected in the <see cref="MonthCalendar"/> changes.
		/// </summary>
		public event DateRangeEventHandler DateChanged;

		/// <summary>
		/// Raises the <see cref="DateChanged"/> event.
		/// </summary>
		/// <param name="drevent">A <see cref="DateRangeEventArgs"/> that contains the event data.</param>
		protected virtual void OnDateChanged(DateRangeEventArgs drevent)
		{
			//only raise if there is a subscriber
			if(this.DateChanged!=null)
			{
				this.DateChanged(this, drevent);
			}
		}
		
		/// <summary>
		/// Occurs when the user selects the "None" button (if set).
		/// </summary>
		public event EventHandler NoneSelected;

		/// <summary>
		/// Raises the <see cref="NoneSelected"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnNoneSelected(EventArgs e)
		{
			//only raise if there is a subscriber
			if(this.NoneSelected!=null)
			{
				this.NoneSelected(this, e);
			}
		}

		#endregion


		#region Range Methods

		private void GetMaxRange()
		{
#if !DESIGN
			IntPtr ptr = MarshalEx.AllocHGlobal(m_maxrange.ToByteArray().Length);
			Win32Window.SendMessage(this.ChildHandle, (int)MCM.GETRANGE, 0, (int)ptr);
			Marshal.Copy(ptr, m_maxrange.ToByteArray(), 0, m_maxrange.ToByteArray().Length);
			MarshalEx.FreeHGlobal(ptr);
#endif
		}

		private void SetMaxRange()
		{
#if !DESIGN
			//copy range to native memory
			IntPtr ptr = MarshalEx.AllocHGlobal(m_maxrange.ToByteArray().Length);
			Marshal.Copy(m_maxrange.ToByteArray(), 0, ptr, m_maxrange.ToByteArray().Length);
			Win32Window.SendMessage(this.ChildHandle, (int)MCM.SETRANGE, (int)(GDTR.MIN | GDTR.MAX), (int)ptr);
			MarshalEx.FreeHGlobal(ptr);
#endif
		}
		private void GetSelRange()
		{
#if !DESIGN
			IntPtr ptr = MarshalEx.AllocHGlobal(m_selrange.ToByteArray().Length);
			Win32Window.SendMessage(this.ChildHandle, (int)MCM.GETSELRANGE, 0, (int)ptr);
			Marshal.Copy(ptr, m_selrange.ToByteArray(), 0, m_selrange.ToByteArray().Length);
			MarshalEx.FreeHGlobal(ptr);
#endif
		}

		private void SetSelRange()
		{
#if !DESIGN
			//copy range to native memory
			IntPtr ptr = MarshalEx.AllocHGlobal(m_selrange.ToByteArray().Length);
			Marshal.Copy(m_selrange.ToByteArray(), 0, ptr, m_selrange.ToByteArray().Length);
			Win32Window.SendMessage(this.ChildHandle, (int)MCM.SETSELRANGE, 0, (int)ptr);
			MarshalEx.FreeHGlobal(ptr);
#endif
		}
		#endregion


		#region Hit Area
		/// <summary>
		/// Defines constants that represent areas in an <see cref="MonthCalendar"/> control.
		/// <para><b>New in v1.1</b></para>
		/// </summary>
		public enum HitArea
		{
			/// <summary>
			/// The specified point is on the today link at the bottom of the month calendar control.
			/// </summary>
			TodayLink          = 0x00030000,
			/// <summary>
			/// The specified point is on the "None" link at the bottom of the month calendar control.
			/// </summary>
			NoneLink           = 0x00040000,
			/// <summary>
			/// The specified point is either not on the month calendar control, or it is in an inactive portion of the control.
			/// </summary>
			Nowhere            = 0x00000000,
			/// <summary>
			/// The specified point is over the background of a month's title.
			/// </summary>
			TitleBackground    = 0x00010000,
			/// <summary>
			/// The specified point is in a month's title bar, over a month name.
			/// </summary>
			TitleMonth         = (TitleBackground | 0x0001),
			/// <summary>
			/// The specified point is in a month's title bar, over the year value.
			/// </summary>
			TitleYear          = (TitleBackground | 0x0002),
			/// <summary>
			/// The specified point is over the button at the upper-right corner of the control.
			/// If the user clicks here, the month calendar scrolls its display to the next month or set of months.
			/// </summary>
			NextMonthButton    = (TitleBackground | 0x01000000 | 0x0003),
			/// <summary>
			/// The specified point is over the button at the upper-left corner of the control.
			/// If the user clicks here, the month calendar scrolls its display to the previous month or set of months.
			/// </summary>
			PrevMonthButton    = (TitleBackground | 0x02000000 | 0x0003),
			/// <summary>
			/// The specified point is part of the calendar's background.
			/// </summary>
			CalendarBackground = 0x00020000,
			/// <summary>
			/// The specified point is on a date within the calendar.
			/// The <see cref="MonthCalendar.HitTestInfo.Time"/> property of <see cref="MonthCalendar.HitTestInfo"/> is set to the date at the specified point.
			/// </summary>
			Date       = (CalendarBackground | 0x0001),
			/// <summary>
			/// The specified point is over a date from the next month (partially displayed at the end of the currently displayed month).
			/// If the user clicks here, the month calendar scrolls its display to the next month or set of months.
			/// </summary>
			NextMonthDate	= (CalendarBackground | 0x01000000 | Date),
			/// <summary>
			/// The specified point is over a date from the previous month (partially displayed at the end of the currently displayed month).
			/// If the user clicks here, the month calendar scrolls its display to the previous month or set of months.
			/// </summary>
			PrevMonthDate   = (CalendarBackground | 0x02000000 | Date),
			/// <summary>
			/// The specified point is over a day abbreviation ("Fri", for example).
			/// The <see cref="P:OpenNETCF.Windows.Forms.MonthCalendar.HitTestInfo.Time"/> property of <see cref="MonthCalendar.HitTestInfo"/> is set to the corresponding date on the top row.
			/// </summary>
			DayOfWeek        = (CalendarBackground | 0x0002),
			/// <summary>
			/// The specified point is over a week number.
			/// This occurs only if the <see cref="MonthCalendar.ShowWeekNumbers"/> property of <see cref="MonthCalendar"/> is enabled.
			/// The <see cref="MonthCalendar.HitTestInfo.Time"/> property of <see cref="MonthCalendar.HitTestInfo"/> is set to the corresponding date in the leftmost column.
			/// </summary>
			WeekNumbers    = (CalendarBackground | 0x0003),
		}
		#endregion

		#region Hit Test Info
		/// <summary>
		/// Contains information about an area of an <see cref="MonthCalendar"/> control.
		/// <para><b>New in v1.1</b></para>
		/// </summary>
		public sealed class HitTestInfo
		{
			private byte[] m_data;

			internal HitTestInfo()
			{
				m_data = new byte[32];
				//write the size to the first 4 bytes
				BitConverter.GetBytes(m_data.Length).CopyTo(m_data, 0);
			}

			internal byte[] ToByteArray()
			{
				return m_data;
			}

			/// <summary>
			/// Gets the <see cref="MonthCalendar.HitArea"/> that represents the area of the calendar evaluated by the hit-test operation.
			/// </summary>
			public HitArea HitArea
			{
				get
				{
					return (HitArea)BitConverter.ToInt32(m_data, 12);
				}
			}

			/// <summary>
			/// Gets the point that was hit-tested.
			/// </summary>
			public Point Point
			{
				get
				{
					return new Point(X, Y);
				}
			}

			internal int X
			{
				get
				{
					return BitConverter.ToInt32(m_data, 4);
				}
				set
				{
					BitConverter.GetBytes(value).CopyTo(m_data, 4);
				}
			}

			internal int Y
			{
				get
				{
					return BitConverter.ToInt32(m_data, 8);
				}
				set
				{
					BitConverter.GetBytes(value).CopyTo(m_data, 8);
				}
			}

			/// <summary>
			/// Gets the time information specific to the location that was hit-tested.
			/// </summary>
			public DateTime Time
			{
				get
				{
					SystemTime st = new SystemTime(m_data, 16);
					return st.ToDateTime();
				}
			}
		}
		#endregion
	}
	#endif
}
