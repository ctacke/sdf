using System;
using System.Diagnostics;

namespace OpenNETCF.Diagnostics
{
	public static class Trace2
	{
		private static string appBase = string.Empty;
		private static bool initialized = false;

        public static TraceLevel TraceLevel
        {
            get { return TraceInternal2.TraceLevel; }
            set { TraceInternal2.TraceLevel = value; }
        }
        
        public static bool AutoFlush 
		{ 
			get 
			{
				if(!initialized) 
					  InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
				return TraceInternal2.AutoFlush; 
			}
			set 
			{
				if(!initialized) 
					InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
				TraceInternal2.AutoFlush = value; 
			}
		}

        public static int IndentSize
        {
            get
            {
                if (!initialized)
                    InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
                return TraceInternal2.IndentSize;
            }
            set
            {
                if (!initialized)
                    InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
                TraceInternal2.IndentSize = value;
            }
        }

        public static int IndentLevel
        {
            get
            {
                if (!initialized)
                    InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
                return TraceInternal2.IndentLevel;
            }
            set
            {
                if (!initialized)
                    InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
                TraceInternal2.IndentLevel = value;
            }
        }


        [Conditional("TRACE")]
        public static void Flush()
        {
            TraceInternal2.Flush();
        }

        [Conditional("TRACE")]
        public static void Indent()
        {
            TraceInternal2.Indent();
        }

        [Conditional("TRACE")]
        public static void Unindent()
        {
            TraceInternal2.Unindent();
        }

		[Conditional("TRACE")]
		public static void Assert(bool condition)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternal2.Assert(condition);
		}

		[Conditional("TRACE")]
		public static void Assert(bool condition, string message)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternal2.Assert(condition, message);
		}

		[Conditional("TRACE")]
		public static void Assert(bool condition, string message, string detailMessage)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternal2.Assert(condition, message, detailMessage);
		}

		[Conditional("TRACE")]
		public static void Write(object value)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternal2.Write(value);
		}
		
		[Conditional("TRACE")]
		public static void Write(string message)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternal2.Write(message);
		}

		[Conditional("TRACE")]
		public static void Write(string message, string category)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternal2.Write(message, category);
		}

		[Conditional("TRACE")]
		public static void Write(object value, string category)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternal2.Write(value, category);
		}

		[Conditional("TRACE")]
		public static void WriteLine(object value)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternal2.WriteLine(value);
		}
		
		[Conditional("TRACE")]
		public static void WriteLine(string message)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternal2.WriteLine(message);
		}

		[Conditional("TRACE")]
		public static void WriteLine(string message, string category)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternal2.WriteLine(message, category);
		}

		[Conditional("TRACE")]
		public static void WriteLine(object value, string category)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternal2.WriteLine(value, category);
		}

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, object value)
        {
            if (!condition) return;
            if (!initialized)
                InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
            TraceInternal2.WriteLine(value);
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, string message)
        {
            if (!condition) return;
            if (!initialized)
                InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
            TraceInternal2.WriteLine(message);
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, string message, string category)
        {
            if (!condition) return;
            if (!initialized)
                InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
            TraceInternal2.WriteLine(message, category);
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, object value, string category)
        {
            if (!condition) return;
            if (!initialized)
                InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
            TraceInternal2.WriteLine(value, category);
        }

        [Conditional("TRACE")]
        public static void WriteIf(bool condition, object value)
        {
            if (!condition) return;
            if (!initialized)
                InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
            TraceInternal2.Write(value);
        }

        [Conditional("TRACE")]
        public static void WriteIf(bool condition, string message)
        {
            if (!condition) return;
            if (!initialized)
                InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
            TraceInternal2.Write(message);
        }

        [Conditional("TRACE")]
        public static void WriteIf(bool condition, string message, string category)
        {
            if (!condition) return;
            if (!initialized)
                InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
            TraceInternal2.Write(message, category);
        }

        [Conditional("TRACE")]
        public static void WriteIf(bool condition, object value, string category)
        {
            if (!condition) return;
            if (!initialized)
                InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
            TraceInternal2.Write(value, category);
        }
		
		public static TraceListenerCollection Listeners
		{
			get
			{
                if (!initialized)
                {
                    InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
                }
				return TraceInternal2.Listeners;
			}
		}

		
		private static void InitializeSettings(string path)
		{
			TraceInternal2.appBase = appBase = path;
			initialized = true;
		}
	}
}
