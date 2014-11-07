using System;
using System.IO;
using System.Diagnostics;

namespace OpenNETCF.Diagnostics 
{
	public class TextWriterTraceListener : TraceListener 
	{
		TextWriter writer;
	
		public TextWriterTraceListener() : base("TextWriter") 
		{
		}
        
		public TextWriterTraceListener(Stream stream) : this(stream, string.Empty) 
		{
		}

		public TextWriterTraceListener(Stream stream, string name) : base(name) 
		{
			if (stream == null) throw new ArgumentNullException("stream");
			this.writer = new StreamWriter(stream);                        
		}

		public TextWriterTraceListener(TextWriter writer) : this(writer, string.Empty) 
		{
		}

		public TextWriterTraceListener(TextWriter writer, string name) 
			: base(name) 
		{
			if (writer == null) throw new ArgumentNullException("writer");
			this.writer = writer;
		}

		public TextWriterTraceListener(string fileName) 
			: this(new StreamWriter(fileName, true)) 
		{
		}

		public TextWriterTraceListener(string fileName, string name) 
			: this(new StreamWriter(fileName, true), name) 
		{
		}
        
		public TextWriter Writer 
		{
			get { return writer; }
			set { writer = value; }
		}
        
		public override void Close() 
		{
			if (writer != null) 
				writer.Close();
		}

		protected override void Dispose(bool disposing) 
		{
			if (disposing) 
				this.Close();

            base.Dispose(disposing);
		}                

		public override void Flush() 
		{
			if (writer == null) return;
			writer.Flush();
		}

		public override void Write(string message) 
		{
			if (writer == null) return;   
			if (NeedIndent) WriteIndent();
			writer.Write(message);
		}

		public override void WriteLine(string message) 
		{
			if (writer == null) return;   
			if (NeedIndent) WriteIndent();
			writer.WriteLine(message);
			NeedIndent = true;
		}
	}
}
