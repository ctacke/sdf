#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



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
