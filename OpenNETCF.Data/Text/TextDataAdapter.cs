//==========================================================================================
//
//		OpenNETCF.Data.Text.TextDataAdapter
//		Copyright (c) 2003-2005, OpenNETCF.org
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
using System.Collections;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace OpenNETCF.Data.Text
{
	/// <summary>
	/// Supports the reading and writing of delimited text into a <see cref="DataSet"/>.
	/// </summary>
	public class TextDataAdapter : System.Data.IDataAdapter
	{
		#region Fields

        /// <summary>
        /// name of file
        /// </summary>
		protected string m_filename;

        /// <summary>
        /// faux tablename
        /// </summary>
		protected string m_tablename;

        /// <summary>
        /// stream reader
        /// </summary>
		protected StreamReader m_reader;

        /// <summary>
        /// stream writer (for updates)
        /// </summary>
		protected StreamWriter m_writer;

        /// <summary>
        /// file contains header row?
        /// </summary>
		protected bool m_hasheader;
		
        /// <summary>
        /// store header names (if applicable)
        /// </summary>
		protected string[] m_headers;

        /// <summary>
        /// character used for delimiting
        /// </summary>
		protected char m_delimiter;

        /// <summary>
        /// if true ignore if no changes and write anyway
        /// </summary>
		protected bool m_forcewrite = false;

		#endregion
		
		#region Constructor
		/// <summary>
		/// Creates a new instance of the TextDataAdapter class
		/// </summary>
		/// <param name="filename">Filename of a valid delimited-text data file.</param>
		/// <param name="hasHeader">True if first row is a header row naming the columns contained in the data, False if first row is a data row.</param>
		/// <param name="delimiter">Character used to separate fields.</param>
		public TextDataAdapter(string filename, bool hasHeader, char delimiter)
		{
			//set filename
			m_filename = filename;

			//tablename is filename without path and extension
			m_tablename = Path.GetFileNameWithoutExtension(m_filename);
		
			//set whether file contains a header row with column names
			m_hasheader = hasHeader;

			//set delimiting character
			if(delimiter == '\"')
			{
				throw new ArgumentException("Cannot use Quote as a delimiter");
			}

			m_delimiter = delimiter;

		}
		/// <summary>
		/// Creates a new instance of the CSVDataAdapter class from a file with no header row.
		/// </summary>
		/// <param name="filename">Filename of a valid CSV data file.</param>
		/// <param name="hasHeader">True if first row is a header row naming the columns contained in the data, False if first row is a data row.</param>
		public TextDataAdapter(string filename, bool hasHeader) : this(filename, hasHeader, ','){}
		/// <summary>
		/// Creates a new instance of the CSVDataAdapter class from a file with no header row.
		/// </summary>
		/// <param name="filename">Filename of a valid CSV data file.</param>
		public TextDataAdapter(string filename) : this(filename, false, ','){}
		#endregion

		#region Properties

		#region FileName
		/// <summary>
		/// Full path to the delimited-text file name
		/// </summary>
		public string FileName
		{
			get
			{
				return m_filename;
			}
			set
			{
				m_filename = value;
			}
		}
		#endregion

		#region ForceWrite
		/// <summary>
		/// If set true Update method will always write to file, even if DataSet has no changes.
		/// </summary>
		public bool ForceWrite
		{
			get
			{
				return m_forcewrite;
			}
			set
			{
				m_forcewrite = value;
			}
		}
		#endregion

		#region Header Row
		/// <summary>
		/// Flag indicating if first row of files indicates column headers for the table.
		/// </summary>
		/// <value>True if first row of file contains column headers, else False (Default is False).</value>
		public bool HasHeaderRow
		{
			get
			{
				return m_hasheader;
			}
			set
			{
				m_hasheader = value;
			}
		}
		#endregion

		#region DelimitingCharacter
		/// <summary>
		/// Character used to separate fields.
		/// </summary>
		/// <remarks>Default is the comma ','. Quotes cannot be used as a delimiter.</remarks>
		public char DelimitingCharacter
		{
			get
			{
				return m_delimiter;
			}
			set
			{
				if(value != '\"')
				{
					m_delimiter = value;
				}
				else
				{
					throw new ArgumentException("Cannot use Quote as a delimiter");
				}
			}
		}
		#endregion

		#endregion

		#region IDataAdapter Members

		#region Fill
		/// <summary>
		/// Adds or refreshes rows in the DataSet to match those in the data source
		/// </summary>
		/// <param name="dataSet">A DataSet to fill with records and, if necessary, schema.</param>
		/// <returns>The number of rows successfully added to or refreshed in the DataSet. This does not include rows affected by statements that do not return rows.</returns>
		public int Fill(System.Data.DataSet dataSet)
		{
			//fill with default table name
			return Fill(dataSet, m_tablename);
		}
		/// <summary>
		///  using the DataSet and DataTable names.
		/// </summary>
		/// <param name="dataSet">A DataSet to fill with records and, if necessary, schema.</param>
		/// <param name="tableName">The name of the source table to use for table mapping.</param>
		/// <returns>The number of rows successfully added to or refreshed in the DataSet. This does not include rows affected by statements that do not return rows.</returns>
		public int Fill(System.Data.DataSet dataSet, string tableName)
		{
			//table within the dataset
			DataTable dt;
			if(dataSet.Tables.Contains(tableName))
			{
				dt = dataSet.Tables[tableName];
			}
			else
			{
				dt = dataSet.Tables.Add(tableName);
			}

			//store records affected
			int records = 0;

			//raw row
			string rawrow;

			//store the current row
			string[] rowbuffer = new string[0];

			//more records
			bool morerecords = true;

			//open stream to read from file
			m_reader = new StreamReader(m_filename);

			if(m_hasheader)
			{
                rawrow = m_reader.ReadLine();
                if (rawrow == null)
                {
                    // no data
                    return 0;
                }

				//read header row and construct schema
				m_headers = SplitRow(rawrow);

				foreach(string column in m_headers)
				{
                    string columnName = column;
					//add to columns collection
                    if (dt.Columns.Contains(columnName))
                    {
                        int index = 1;
                        do
                        {
                            columnName = column + index.ToString();
                            index++;
                        } while (dt.Columns.Contains(columnName));
                    }
                    DataColumn dc = dt.Columns.Add(columnName);
					dc.DataType = typeof(string);
					dc.AllowDBNull = true;
				}

				//read first data row
				//Updated (Bug# 000091) Failure if only header row present
				rawrow = m_reader.ReadLine();

				//check for null first - avoid throwing exception
				if(rawrow==null | rawrow==String.Empty)
				{
					morerecords = false;
				}
				else
				{
					rowbuffer = SplitRow(rawrow);
				}
			}
			else
			{
				//read line
				rawrow = m_reader.ReadLine();

				if(rawrow==null | rawrow==String.Empty)
				{
					morerecords = false;
				}
				else
				{
					//read the first row and get the length
					rowbuffer = SplitRow(rawrow);
				}
				

				for(int iColumn = 0; iColumn < rowbuffer.Length; iColumn++)
				{
					//add to columns collection
					DataColumn dc = dt.Columns.Add("Column " + iColumn.ToString());
					dc.DataType = typeof(string);
					dc.AllowDBNull = true;
				}
			}

			//processing of further rows goes here
            int newColNum = 0;
            bool addRow = true;
			while(morerecords)
			{
				//increment rows affected
				if(addRow) records++;
                addRow = true;

                if (rowbuffer.Length != dt.Columns.Count)
                {
                    // support adding new columns at the very end
                    if ((HasHeaderRow) && (string.Compare(rowbuffer[0], dt.Columns[0].ColumnName) == 0))
                    {
                        addRow = false;
                        newColNum = dt.Columns.Count;
                        while (rowbuffer.Length > dt.Columns.Count)
                        {
                            dt.Columns.Add(rowbuffer[newColNum++]);
                        }
                    }
                    else
                    {
                        // if the column count expanded mid-data, account for it
                        while (rowbuffer.Length > dt.Columns.Count)
                        {
                            dt.Columns.Add(string.Format("New column {0}", ++newColNum));
                        }
                    }
                }
				//add values to row and insert into table
                if(addRow) dt.Rows.Add(rowbuffer);

				//read the next row
				rawrow = m_reader.ReadLine();

				if(rawrow==null | rawrow==String.Empty)
				{
					morerecords = false;
				}
				else
				{
					//read the first row and get the length
					rowbuffer = SplitRow(rawrow);
				}
			}

			//close stream
			m_reader.Close();

			//mark dataset as up-to-date
			dataSet.AcceptChanges();

			return records;
		}
		#endregion

		#region Interface Methods

		/// <summary>
		/// Not Supported
		/// </summary>
		System.Data.ITableMappingCollection System.Data.IDataAdapter.TableMappings
		{
			get
			{
				// TODO:  Add CSVDataAdapter.TableMappings getter implementation
				return null;
			}
		}

		/// <summary>
		/// Not Supported
		/// </summary>
		System.Data.MissingSchemaAction System.Data.IDataAdapter.MissingSchemaAction
		{
			get
			{
				return MissingSchemaAction.Add;
			}
			set
			{
				// TODO:  Add CSVDataAdapter.MissingSchemaAction setter implementation
			}
		}

		/// <summary>
		/// Not Supported
		/// </summary>
		System.Data.MissingMappingAction System.Data.IDataAdapter.MissingMappingAction
		{
			get
			{
				return System.Data.MissingMappingAction.Passthrough;
			}
			set
			{
				// TODO:  Add CSVDataAdapter.MissingMappingAction setter implementation
			}
		}

		/// <summary>
		/// Not Supported
		/// </summary>
		/// <returns></returns>
		System.Data.IDataParameter[] System.Data.IDataAdapter.GetFillParameters()
		{
			// TODO:  Add CSVDataAdapter.GetFillParameters implementation
			return null;
		}

		/// <summary>
		/// Not Supported
		/// </summary>
		/// <param name="dataSet"></param>
		/// <param name="schemaType"></param>
		/// <returns></returns>
		System.Data.DataTable[] System.Data.IDataAdapter.FillSchema(System.Data.DataSet dataSet, System.Data.SchemaType schemaType)
		{
			// TODO:  Add CSVDataAdapter.FillSchema implementation
			return null;
		}

		#endregion

		#region Update
		/// <summary>
		/// Writes out the updated DataSet contents to CSV File.
		/// </summary>
		/// <param name="dataSet">The DataSet used to update the data source.</param>
		/// <returns>The number of rows successfully updated from the DataSet.</returns>
		public int Update(System.Data.DataSet dataSet)
		{
			//update default tablename
			return Update(dataSet, m_tablename);
		}
		/// <summary>
		/// Writes out the updated named DataTable from the DataSet to CSV File.
		/// </summary>
		/// <param name="dataSet">The DataSet used to update the data source.</param>
		/// <param name="srcTable">The DataTable.Name to be written.</param>
		/// <returns>The number of rows successfully updated from the DataSet.</returns>
		public int Update(System.Data.DataSet dataSet, string srcTable)
		{
			if(dataSet.HasChanges() || m_forcewrite)
			{
				DataTable table;

				try
				{
					table = dataSet.Tables[srcTable];
				}
				catch
				{
					//could not find the table specified
					throw new ArgumentException("srcTable does not exist in specified dataSet");
				}
				//open filestream (overwrite previous file)
				m_writer = new StreamWriter(m_filename,false);

				if(m_hasheader)
				{
					string columnrow = "";

					//write header row
					foreach(DataColumn dc in table.Columns)
					{
						//write column name
						columnrow += dc.ColumnName + m_delimiter;
					}

					//write assembled column names minus the trailing comma
					m_writer.WriteLine(columnrow.TrimEnd(m_delimiter));
				}

				//count the number of rows written
				int rowsaffected = 0;

				//write out all the rows (unless they were deleted)
				foreach(DataRow thisrow in table.Rows)
				{
					//write all except deleted rows
					if(thisrow.RowState!=DataRowState.Deleted)
					{
						//write assembled row minus the trailing comma
						m_writer.WriteLine(BuildRow(thisrow.ItemArray));

						rowsaffected ++;
					}
				}

				//close filestream
				m_writer.Close();
				
				//mark table as up-to-date
				table.AcceptChanges();

				//return number of rows written
				return rowsaffected;
			}
			else
			{
				//no changes - ignore
				return 0;
			}
		}
		#endregion

		#endregion

		#region Helper Functions

		#region Quote String
		/// <summary>
		/// Add quotes to a string if it contains a space or carriage return
		/// </summary>
		/// <param name="inString"></param>
		/// <returns></returns>
		private string QuoteString(object inString)
		{
			if(inString.ToString().IndexOf(' ') > -1 || inString.ToString().IndexOf(m_delimiter) > -1)
			{
				return "\"" + inString.ToString() + "\"";
			}
			else
			{
				return inString.ToString();
			}
		}
		#endregion

		#region Build Row
		/// <summary>
		/// Builds a row into a single string with delimiting characters
		/// </summary>
		/// <returns></returns>
		private string BuildRow(object[] values)
		{
			//create string for a single output row
			string row = "";

			//loop for each item in the row
			foreach(object column in values)
			{
				if(column != null)
				{
					//add the item (with quotes as appropriate
					row += QuoteString(column);
				}

				//add the delimiting character
				row += m_delimiter;
			}
			
			//remove the extra delimiter at the end
			//Updated (Bug# 000090) to avoid removing additional commas for blank row values
			row = row.Remove(row.Length - 1, 1);

			return row;
		}
		#endregion

		#region Split Row

		// New Regex split
		// Submitted by Gorm Stilbo 11/2004
		private string[] SplitRow(string row)
		{
			Regex r = new Regex(m_delimiter.ToString() + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
			string[] fields = r.Split(row);

			//remove quotes if present
			for(int ifield = 0; ifield < fields.Length; ifield++)
			{
				fields[ifield] = fields[ifield].Trim('"');
			}
			return fields;
		}
		#endregion

		#endregion
	}
}
