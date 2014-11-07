using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Provides methods to place data on and retrieve data from the system clipboard.
	/// </summary>
	/// <remarks>For a list of predefined formats to use with the Clipboard class, see the <see cref="DataFormats"/> class.
	/// <para>Call <see cref="SetDataObject"/> to put data on the clipboard.</para>
	/// <para>Place data on the clipboard in multiple formats to maximize the possibility that a target application, whose format requirements you might not know, can successfully retrieve the data.</para>
	/// <para>Call <see cref="GetDataObject"/> to retrieve data from the clipboard.
	/// The data is returned as an object that implements the <see cref="IDataObject"/> interface.
	/// Use the methods specified by <see cref="IDataObject"/> and fields in <see cref="DataFormats"/> to extract the data from the object. 
	/// If you do not know the format of the data you retrieved, call the <see cref="IDataObject.GetFormats(bool)"/> method of the <see cref="IDataObject"/> interface to get a list of all formats that data is stored in.
	/// Then call the <see cref="IDataObject.GetData(string, bool)"/> method of the <see cref="IDataObject"/> interface, and specify a format that your application can use.</para>
	/// <para>All Windows applications share the system clipboard, so the contents are subject to change when you switch to another application.</para>.
	/// Supports only Unicode text and Image (Bitmap) formats.</remarks>
	public static class Clipboard2
	{
		#region Clear
		/// <summary>
		/// Clears the contents of the Clipboard.
		/// </summary>
		public static void Clear()
		{
			if(NativeMethods.OpenClipboard(IntPtr.Zero))
			{
                NativeMethods.EmptyClipboard();
                NativeMethods.CloseClipboard();
			}
		}
		#endregion

		#region Contains
		/// <summary>
		/// Determines if clipboard contains data in the specified format
		/// </summary>
		/// <param name="format">A clipboard format, see <see cref="DataFormats"/> for possible values.</param>
		/// <returns>True if clipboard contains specified format; otherwise False.</returns>
		public static bool ContainsData(string format)
		{
            return NativeMethods.IsClipboardFormatAvailable(DataFormats2.GetFormat(format).Id);
		}

        /*
		/// <summary>
		/// Determines if clipboard contains audio.
		/// </summary>
		/// <returns>True if clipboard contains audio data; otherwise False.</returns>
		public static bool ContainsAudio()
		{
			return ContainsData(DataFormats.WaveAudio);
		}*/

		/// <summary>
		/// Determines if clipboard contains an Image.
		/// </summary>
		/// <returns>True if clipboard contains Image; otherwise False.</returns>
		public static bool ContainsImage()
		{
			return ContainsData(DataFormats.Bitmap);
		}

		/// <summary>
		/// Determines if clipboard contains Text.
		/// </summary>
		/// <returns>True if clipboard contains Text; otherwise False.</returns>
		public static bool ContainsText()
		{
			return ContainsData(DataFormats.UnicodeText);
		}
		#endregion

		#region Get helper
		/// <summary>
		/// Retrieves data from the Clipboard in the specified format.
		/// </summary>
		/// <param name="format">Clipboard format, see <see cref="DataFormats"/> for possible values.</param>
		/// <returns>Returns the specified data or null if not present.</returns>
		public static object GetData(string format)
		{
			IDataObject obj = GetDataObject();
            if (obj != null)
            {
                return obj.GetData(format);
            }
			return null;
		}
		#endregion

		#region Image
		/// <summary>
        /// Adds an <see cref="Image"/> to the Clipboard in the <see cref="DataFormats.Bitmap"/> format.
		/// </summary>
		/// <param name="image">An Image that must be placed on clipboard.</param>
		public static void SetImage(Image image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			SetDataObject(image);
		}

		/// <summary>
		/// Retrieves an image from the Clipboard.
		/// </summary>
		/// <returns>An Image from clipboard or null if doesn't contain Image.</returns>
        public static Image GetImage()
        {
            if (ContainsImage())
            {
                IDataObject obj = GetDataObject();
                if (obj.GetDataPresent(typeof(Image)))
                    return (Image)obj.GetData(typeof(Image));
                    
            }

            return null;
        }
		#endregion

		#region Text
		/// <summary>
		/// Places specified text onto the clipboard.
		/// </summary>
		/// <param name="text">Text to be added to the clipboard</param>
		public static void SetText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}

			SetDataObject(text);	
		}

		/// <summary>
		/// Retrieves data from the clipboard as text.
		/// </summary>
		/// <returns>Text representation of clipboard contents</returns>
		public static string GetText()   
		{
            if (ContainsText())
            {
                IDataObject obj = GetDataObject();
                if (obj.GetDataPresent(typeof(string)))
                    return (string)obj.GetData(typeof(string));
            }
			
            return null;
		}
		#endregion


		#region Set Data Object
		/// <summary>
		/// Places nonpersistent data on the system clipboard.
		/// </summary>
		/// <param name="data">The data to place on the clipboard.</param>
		/// <exception cref="System.ArgumentNullException">The value of data is null.</exception>
		public static void SetDataObject(object data)
		{
            Clipboard.SetDataObject(data);
		}
		#endregion

		#region Get Data Object
		/// <summary>
		/// Retrieves the data that is currently on the system clipboard.
		/// </summary>
		/// <returns>An <see cref="IDataObject"/> that represents the data currently on the clipboard, or null if there is no data on the clipboard.</returns>
		public static IDataObject GetDataObject()
		{
            return Clipboard.GetDataObject();
		}
		#endregion


		
	}

	#region Clipboard Formats
	// <summary>
	// Represents CF_ constants defined in winuser.h
	// </summary>
	enum ClipboardFormats : int
	{
		 
		Text         =  1,
		Bitmap       =  2,
		SymbolicLink =  4,
		Dif          =  5,
		Tiff         =  6,
		OemText      =  7,
		Dib          =  8,
		Palette      =  9,
		PenData      = 10,
		Riff         = 11,
		WaveAudio    = 12,
		UnicodeText  = 13,
	}
	#endregion

    #region Data Formats
    /// <summary>
    /// Provides static, predefined <see cref="Clipboard"/> format names.
    /// Use them to identify the format of data that you store in an <see cref="IDataObject"/>.
    /// </summary>
    public static class DataFormats2
    {

        #region Data Formats
        
        /// <summary>
        /// Specifies a Windows bitmap format.
        /// </summary>
        public static readonly string Bitmap = ClipboardFormats.Bitmap.ToString();
        // <summary>
        // Specifies the Windows Device Independent Bitmap (DIB) format.
        // </summary>
        //public static readonly string Dib = ClipboardFormats.Dib.ToString();
        // <summary>
        // Specifies the Windows Data Interchange Format (DIF), which Windows Forms does not directly use.
        // </summary>
        //public static readonly string Dif = ClipboardFormats.Dif.ToString();
        // <summary>
        // Specifies the standard Windows original equipment manufacturer (OEM) text format.
        // </summary>
        //public static readonly string OemText = ClipboardFormats.OemText.ToString();
        // <summary>
        // Specifies the Windows palette format.
        // </summary>
        //public static readonly string Palette = ClipboardFormats.Palette.ToString();
        // <summary>
        // Specifies the Windows pen data format, which consists of pen strokes for handwriting software; Windows Forms does not use this format.
        // </summary>
        //public static readonly string PenData = ClipboardFormats.PenData.ToString();
        // <summary>
        // Specifies the Resource Interchange File Format (RIFF) audio format, which Windows Forms does not directly use.
        // </summary>
        //public static readonly string Riff = ClipboardFormats.Riff.ToString();
        // <summary>
        // Specifies the Windows symbolic link format, which Windows Forms does not directly use.
        // </summary>
        //public static readonly string SymbolicLink = ClipboardFormats.SymbolicLink.ToString();
        /// <summary>
        /// Specifies the standard ANSI text format.
        /// </summary>
        public static readonly string Text = ClipboardFormats.Text.ToString();
        // <summary>
        // Specifies the Tagged Image File Format (TIFF), which Windows Forms does not directly use.
        // </summary>
        //public static readonly string Tiff = ClipboardFormats.Tiff.ToString();
        /// <summary>
        /// Specifies the standard Windows Unicode text format.
        /// </summary>
        public static readonly string UnicodeText = ClipboardFormats.UnicodeText.ToString();
        /// <summary>
        /// Specifies the wave audio format, which Windows Forms does not directly use.
        /// </summary>
        public static readonly string WaveAudio = ClipboardFormats.WaveAudio.ToString();
        
        #endregion

        #region Get Format
        /// <summary>
        /// Returns a <see cref="Format"/> with the Windows Clipboard numeric ID and name for the specified ID.
        /// </summary>
        /// <param name="id">The format ID.</param>
        /// <returns>A <see cref="Format"/> that has the Windows Clipboard numeric ID and the name of the format.</returns>
        public static Format GetFormat(int id)
        {
            try
            {
                //return specified format
                return new Format(((ClipboardFormats)id).ToString(), id);
            }
            catch
            {
                //on error return null
                return null;
            }
        }
        /// <summary>
        /// Returns a <see cref="Format"/> with the Windows Clipboard numeric ID and name for the specified format.
        /// </summary>
        /// <param name="format">The format name.</param>
        /// <returns>A <see cref="Format"/> that has the Windows Clipboard numeric ID and the name of the format.</returns>
        public static Format GetFormat(string format)
        {
            try
            {
                //cf.GetType().GetField(format).GetValue(cf)
                return new Format(format, (int)(typeof(ClipboardFormats).GetField(format, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null)));
            }
            catch
            {
                throw new Exception("Resolving Clipboard type failed");
            }
        }
        #endregion

        #region Format
        /// <summary>
        /// Represents a clipboard format type.
        /// </summary>
        public class Format
        {
            int id;
            string name;

            /// <summary>
            /// Create a new instance of Format.
            /// </summary>
            /// <param name="name">Name of the format.</param>
            /// <param name="id">ID number of the format.</param>
            public Format(string name, int id)
            {
                this.name = name;
                this.id = id;
            }

            /// <summary>
            /// Gets the name of this format.
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
            }

            /// <summary>
            /// Gets the ID number for this format.
            /// </summary>
            public int Id
            {
                get
                {
                    return id;
                }
            }
        }
        #endregion
    }
    #endregion

	#region DataObject
    /*
	/// <summary>
	/// Implements a basic data transfer mechanism.
	/// </summary>
	public class DataObject : IDataObject
	{
		//store data formats
		private System.Collections.Hashtable m_data;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DataObject"/> class, which can store arbitrary data.
		/// </summary>
		public DataObject()
		{
			m_data = new System.Collections.Hashtable();
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="DataObject"/> class, containing the specified data and its associated format.
		/// </summary>
		/// <param name="format">The class type associated with the data. See <see cref="OpenNETCF.Windows.Forms.DataFormats"/> for the predefined formats.</param>
		/// <param name="data">The data to store.</param>
		public DataObject(string format, object data) :this()
		{
			m_data.Add(format, data);
		}
		#endregion

		#region IDataObject Members

		/// <summary>
		/// Returns the data associated with the specified data format.
		/// </summary>
		/// <param name="format">The format of the data to retrieve. See <see cref="DataFormats"/> for predefined formats.</param>
		/// <returns>The data associated with the specified format, or null.</returns>
		public object GetData(string format)
		{
			try
			{
				//return the object of specified format
				return m_data[format];
			}
			catch
			{
				//specified format not contained
				return null;
			}
		}

		/// <summary>
		/// Determines whether data stored in this instance is associated with, or can be converted to, the specified format.
		/// </summary>
		/// <param name="format">The format to check for. See <see cref="DataFormats"/> for predefined formats.</param>
		/// <returns>true if data stored in this instance is associated with, or can be converted to, the specified format; otherwise, false.</returns>
		public bool GetDataPresent(string format)
		{
			return m_data.ContainsKey(format);
		}

		/// <summary>
		/// Stores the specified format and data in this instance.
		/// </summary>
		/// <param name="format">The format associated with the data. See <see cref="DataFormats"/> for predefined formats.</param>
		/// <param name="data">The data to store.</param>
		public void SetData(string format, object data)
		{
			m_data.Add(format, data);
		}

		#endregion
	}*/
	#endregion
}
