using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Net;
using OpenNETCF.Net.Ftp;

namespace FTPSample
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public partial class Form1 : System.Windows.Forms.Form
	{
		private Stream ftpRequestStream;
		private FtpWebRequest request;

		public Form1()
		{
			InitializeComponent();
		}

		private void Connect()
		{
			FtpRequestCreator creator = new FtpRequestCreator();
			WebRequest.RegisterPrefix( "ftp:", creator );

			// Building our URI object
			Uri testUri;
			if( txtServer.Text.IndexOf( "ftp:" ) != 0 )
				testUri = new Uri( "ftp://"+txtServer.Text );
			else
				testUri = new Uri( txtServer.Text );

			// Creating a new FtbRequest object
			request = (FtpWebRequest)WebRequest.Create( testUri );
			request.Credentials = new NetworkCredential( txtUsername.Text, txtPassword.Text );

			// Getting the Request stream
			ftpRequestStream = request.GetRequestStream();

			StreamReader reader = new StreamReader( ftpRequestStream );
			txtServerCtrlResp.Text = reader.ReadToEnd();
		}

		private void btnSendCommand_Click(object sender, System.EventArgs e)
		{
			txtServerCtrlResp.Text = "";
			txtServerDataResp.Text = "";
			inputPanel1.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				// Opening the dataconnection, this must be done BEFORE we issue the command!!
				Stream ftpResponseStream = request.GetResponse().GetResponseStream();
				StreamReader response = new StreamReader( ftpResponseStream );

				// Writing the command to the request stream
				StreamWriter writer = new StreamWriter( ftpRequestStream );
				writer.Write( txtCommand.Text+"\r\n" );
				writer.Flush(); // We MUST flush before we start reading from both response and request

				// Reading the request output
				StreamReader reader = new StreamReader( ftpRequestStream );
				string message = reader.ReadToEnd();
				txtServerCtrlResp.Text = message;

				// Now retreiving the response on the response stream, this can be a file a directory listing or whatever the command
				// dataoutput is
				string responseMessage = response.ReadToEnd();
				txtServerDataResp.Text = responseMessage;
			}
			catch( Exception err )
			{
				MessageBox.Show( err.Message );
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void CloseConnection()
		{
			if( ftpRequestStream != null )
			{
				StreamWriter writer = new StreamWriter( ftpRequestStream );
				writer.WriteLine("QUIT");
				writer.Flush();
				ftpRequestStream.Close();
				ftpRequestStream = null;
			}
			request = null;
		}

		private void btnConnect_Click(object sender, System.EventArgs e)
		{
			txtServerCtrlResp.Text = "";
			txtServerDataResp.Text = "";
			inputPanel1.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				if( ftpRequestStream == null )
				{
					Connect();
					btnConnect.Text = "Disconnect";
					txtServer.Enabled = false;
					txtUsername.Enabled = false;
					txtPassword.Enabled = false;
					btnSendCommand.Enabled = true;
					txtCommand.Enabled = true;

					// Setting default command to retrieval of remote files...
					txtCommand.Text = "LIST";
				}
				else
				{
					CloseConnection();
					btnConnect.Text = "Connect";
					txtServer.Enabled = true;
					txtUsername.Enabled = true;
					txtPassword.Enabled = true;
					btnSendCommand.Enabled = false;
					txtCommand.Enabled = false;
				}
			}
			catch( Exception err )
			{
				MessageBox.Show( err.Message );
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void txtBoxes_GotFocus(object sender, System.EventArgs e)
		{
			inputPanel1.Enabled = true;
		}
	}
}





















