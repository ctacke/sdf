using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OpenNETCF.IO
{
    public class DelimitedTextWriter : IDisposable
    {
        private StreamWriter m_writer;
        StringBuilder m_outputBuilder = new StringBuilder();

        #region --- ctors ---

        public static implicit operator DelimitedTextWriter(StreamWriter w)
        {
            return new DelimitedTextWriter(w, ",", "\n");
        }

        private DelimitedTextWriter(StreamWriter writer, string fieldDelimiter, string rowDelimiter)
        {
            m_writer = writer;
            FieldDelimiter = fieldDelimiter;
            RowDelimiter = rowDelimiter;
        }

        /// <summary>
        /// Creates a DelimitedTextFileWriter with a comma (',') FieldDelimiter and a newline ('\n') RowDelimiter
        /// </summary>
        public DelimitedTextWriter(string path)
            : this(path, ',', '\n')
        {
        }

        /// <summary>
        /// Creates a DelimitedTextFileWriter with the provided FieldDelimiter and a newline ('\n') RowDelimiter
        /// </summary>
        /// <param name="fieldDelimiter">char that separates fields in a row</param>
        /// <param name="path">Fully-qualified path to the delimited text file</param>
        public DelimitedTextWriter(string path, char fieldDelimiter)
            : this(path, fieldDelimiter, '\n')
        {
        }

        /// <summary>
        /// Creates a DelimitedTextFileWriter with the provided FieldDelimiter and a newline ('\n') RowDelimiter
        /// </summary>
        /// <param name="fieldDelimiter">string that separates fields in a row</param>
        /// <param name="path">Fully-qualified path to the delimited text file</param>
        public DelimitedTextWriter(string path, string fieldDelimiter)
            : this(path, fieldDelimiter, "\n")
        {
        }

        /// <summary>
        /// Creates a DelimitedTextFileWriter with the provided FieldDelimiter and RowDelimiter
        /// </summary>
        /// <param name="path">Fully-qualified path to the delimited text file</param>
        /// <param name="fieldDelimiter">char that separates fields in a row</param>
        /// <param name="rowDelimiter">char that separates rows in the file</param>
        public DelimitedTextWriter(string path, char fieldDelimiter, char rowDelimiter)
            : this(path, false, Encoding.Unicode, new string(fieldDelimiter, 1), new string(rowDelimiter, 1))
        {
        }

        /// <summary>
        /// Creates a DelimitedTextFileWriter with the provided FieldDelimiter and RowDelimiter
        /// </summary>
        /// <param name="path">Fully-qualified path to the delimited text file</param>
        /// <param name="fieldDelimiter">char that separates fields in a row</param>
        /// <param name="rowDelimiter">char that separates rows in the file</param>
        public DelimitedTextWriter(string path, string fieldDelimiter, string rowDelimiter)
            : this(path, false, Encoding.Unicode, fieldDelimiter, rowDelimiter)
        {
        }

        /// <summary>
        /// Creates a DelimitedTextFileWriter with the provided FieldDelimiter and RowDelimiter
        /// </summary>
        /// <param name="fieldDelimiter">string that separates fields in a row</param>
        /// <param name="rowDelimiter">string that separates rows in the file</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="path">Fully-qualified path to the delimited text file</param>
        /// <param name="append">
        /// Determines whether data is to be appended to the file. If the file exists
        /// and append is false, the file is overwritten. If the file exists and append
        /// is true, the data is appended to the file. Otherwise, a new file is created.
        /// </param>
        public DelimitedTextWriter(string path, bool append, Encoding encoding, string fieldDelimiter, string rowDelimiter)
        {
            FieldDelimiter = fieldDelimiter;
            RowDelimiter = rowDelimiter;
            m_writer = new StreamWriter(path, append, encoding);
        }
        #endregion

        /// <summary>
        /// Gets or sets the string that separates fields in a row
        /// </summary>
        public string FieldDelimiter { get; set; }

        /// <summary>
        /// Gets or sets the string that separates rows in the file
        /// </summary>
        public string RowDelimiter { get; set; }

        /// <summary>
        /// Writes a RowDelimiter to the output file
        /// </summary>
        public void NextRow()
        {
            m_writer.Write(RowDelimiter);
        }

        /// <summary>
        /// Writes an array of values to a single row in the target file
        /// </summary>
        /// <param name="values">values to write</param>
        /// <summary>
        /// Writes an array of values to a single row in the target file
        /// </summary>
        public void Write(object[] values)
        {
            m_outputBuilder.Length = 0;
            int i = 0;
            if (values.Length > 1)
            {
                for (; i < values.Length - 1; i++)
                {
                    m_outputBuilder.Append(values[i].ToString());
                    m_outputBuilder.Append(FieldDelimiter);
                }
            }
            m_outputBuilder.Append(values[i].ToString());
            m_outputBuilder.Append(RowDelimiter);

            m_writer.Write(m_outputBuilder.ToString());
        }

        /// <summary>
        /// Writes an array of values to a single row in the target file
        /// </summary>
        /// <param name="values">values to write</param>
        public void Write(string[] values)
        {
            Write((object[])values);
        }

        /// <summary>
        /// Writes any IEnumerable list to a single row in the target file.  
        /// If each element in the list is also IEnumerable then each element becomes a row in the target file
        /// </summary>
        /// <param name="valueList"></param>
        public void Write(IEnumerable valueList)
        {
            List<object> values = new List<object>();
            IEnumerator enumerator = valueList.GetEnumerator();

            enumerator.Reset();
            while(enumerator.MoveNext())
            {
                if (enumerator.Current.GetType().IsArray)
                {
                    Write((IEnumerable)enumerator.Current);
                }
                else
                {
                    values.Add(enumerator.Current);
                }
            }
            Write(values.ToArray());
        }

        /// <summary>
        /// Writes the value followed by the currently set FieldDelimiter to the output file
        /// </summary>
        /// <param name="value">Value to write</param>
        public void Write(string value)
        {
            m_writer.Write(value + FieldDelimiter);
        }

        /// <summary>
        /// Closes the current writer and releases any system resources associated with
        ///  the writer.
        /// </summary>
        public void Close()
        {
            m_writer.Close();
        }

        /// <summary>
        /// Causes any buffered data to be flushed to the target file
        /// </summary>
        public void Flush()
        {
            m_writer.Flush();
        }

        /// <summary>
        ///  Releases the unmanaged resources used by the DelimitedTextWriter
        /// </summary>
        public void Dispose()
        {
            m_writer.Flush();
            m_writer.Close();
        }
    }
}
