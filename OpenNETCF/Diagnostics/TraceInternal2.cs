using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;

namespace OpenNETCF.Diagnostics
{
	internal static class TraceInternal2
	{
		internal static int AssertFailure(string File, int Line, string Expr)
		{
			Type t = Type.GetType("System.Diagnostics.TraceInternal, System"); 
			if(t == null)
				return -1;

			MethodInfo mi = t.GetMethod("AssertFailure", BindingFlags.NonPublic|BindingFlags.Static);
			if(mi == null)
				return -1;

			return (int)mi.Invoke(null, new object[]{File, Line, Expr});
		}

		private static TraceListenerCollection listeners;
		private static bool autoFlush = true;
		private static int indentLevel;
		private static int indentSize;
		private static bool settingsInitialized = false;
        private static bool wasInitialized = false;
        internal static string appBase = string.Empty;
        internal static TraceLevel TraceLevel = TraceLevel.Off;

		public static TraceListenerCollection Listeners
		{
			get
			{
				if(!settingsInitialized) InitializeSettings();

				if (listeners == null)
				{
                    Type t = Type.GetType("System.Diagnostics.TraceListenerCollection, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=969db8053d3322ac");
					if(t == null)
						return null;

					ConstructorInfo ctorInfo = t.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] {}, null);
					if(ctorInfo == null)
						return null;
					
					listeners = (TraceListenerCollection)(ctorInfo.Invoke(new object[] {}));

					TraceListener defaultListener = new DefaultTraceListener();
					defaultListener.IndentLevel = indentLevel;
					defaultListener.IndentSize = indentSize;
					listeners.Add(defaultListener);
				}
				return listeners;
			}
		}

		public static bool AutoFlush
		{
			get	{ if(!settingsInitialized) InitializeSettings(); return autoFlush;}
			set { if(!settingsInitialized) InitializeSettings(); autoFlush = value; }
 		}

		public static int IndentLevel
		{
			get	{ if(!settingsInitialized) InitializeSettings(); return indentLevel; }
			set
			{
				if (!settingsInitialized) InitializeSettings();
				if (value < 0)
				{
					value = 0;
				}
				
				indentLevel = value;

				foreach(TraceListener listener in Listeners)
				{
					listener.IndentLevel = indentLevel;
				}
			}
		}

		public static int IndentSize
		{
			get { if(!settingsInitialized) InitializeSettings(); return indentSize; }
			set { if(!settingsInitialized) InitializeSettings(); SetIndentSize(value); }
		}

		private static void SetIndentSize(int value) 
		{
			if (value < 0) 
				value = 0;

			indentSize = value;
			if (listeners != null) 
			{
				foreach (TraceListener listener in Listeners) 
				{
					listener.IndentSize = indentSize;
				}
			} 
		}

		public static void Indent() 
		{
			if(!settingsInitialized) InitializeSettings();
			indentLevel++;
			foreach (TraceListener listener in Listeners) 
			{
				listener.IndentLevel = indentLevel;
			}
		}

		public static void Unindent() 
		{
			if(!settingsInitialized) InitializeSettings();
			if(indentLevel > 0) indentLevel--;
			
			foreach(TraceListener listener in Listeners) 
			{
				listener.IndentLevel = indentLevel;
			}
		}

		public static void Flush()
		{
			if(listeners != null)
			{
				foreach(TraceListener listener in Listeners)
				{
					listener.Flush();
				}
			}
		}

		public static void Close()
		{
			if(listeners != null)
			{
				foreach(TraceListener listener in Listeners)
				{
					listener.Close();
				}
			}
		}

		public static void Assert(bool condition)
		{
			if(condition) return;

			Fail(String.Empty, null);
		}

		public static void Assert(bool condition, string message)
		{
			if (condition) return;

			Fail(message, null);
		}

		public static void Assert(bool condition, string message, string detailMessage)
		{
			if (condition) return;
			
			Fail(message, detailMessage);
		}

		public static void Fail(string message, string detailMessage)
		{
			string failMsg = message;
			if (detailMessage != null)
			{
				failMsg = String.Concat(failMsg, "\r\n\r\n", detailMessage);
			}
			AssertFailure("managed code", 0, failMsg);
		}

		public static void Fail(string message)
		{
			Fail(message, null);
		}

		internal static void InitializeSettings() 
		{
            if(TraceInternal2.settingsInitialized || (!TraceInternal2.wasInitialized && DiagnosticsConfiguration.IsInitialized()))
            {
                TraceInternal2.wasInitialized = DiagnosticsConfiguration.IsInitialized();
			    DiagnosticsConfiguration.AppBase = appBase;
			    SetIndentSize(DiagnosticsConfiguration.IndentSize);
			    autoFlush = DiagnosticsConfiguration.AutoFlush;
			    settingsInitialized = true;
            }
		}

		public static void Write(string message)
		{
			foreach(TraceListener listener in Listeners)
			{
				listener.Write(message);
				if(autoFlush)
					listener.Flush();
			}
		}

		public static void Write(object value)
		{
			foreach(TraceListener listener in Listeners)
			{
				listener.Write(value);
				if(autoFlush)
					listener.Flush();
			}
		}

		public static void Write(string message, string category)
		{
			foreach(TraceListener listener in Listeners)
			{
				listener.Write(message, category);
				if(autoFlush)
					listener.Flush();
			}
		}

		public static void Write(object value, string category)
		{
			foreach(TraceListener listener in Listeners)
			{
				listener.Write(value, category);
				if(autoFlush)
					listener.Flush();
			}
		}

		public static void WriteLine(string message)
		{
			foreach(TraceListener listener in Listeners)
			{
				listener.WriteLine(message);
				if(autoFlush)
					listener.Flush();
			}
		}
		
		public static void WriteLine(object value)
		{
			foreach(TraceListener listener in Listeners)
			{
				listener.WriteLine(value);
				if(autoFlush)
					listener.Flush();
			}
		}

		public static void WriteLine(string message, string category)
		{
			foreach(TraceListener listener in Listeners)
			{
				listener.WriteLine(message, category);
				if(autoFlush)
					listener.Flush();
			}
		}

		public static void WriteLine(object value, string category)
		{
			foreach(TraceListener listener in Listeners)
			{
				listener.WriteLine(value, category);
				if(autoFlush)
					listener.Flush();
			}
		}
	}
}
