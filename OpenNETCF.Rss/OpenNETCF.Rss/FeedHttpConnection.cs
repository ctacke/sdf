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
using System.Net;
using System.Collections;
using System.Text;
using System.Threading;

using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
    /// <summary>
    /// Implements HTTP communication.
    /// </summary>
	public class FeedHttpConnection
    {
		#region fields

		private HttpWebResponse response;
		private IFeedSerializer serializer;
		const int BUFFER_SIZE = 1024; 
		
		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the FeedHttpConnection class.
		/// </summary>
		/// <param name="destination">The Uri object of the RSS feed.</param>
		/// <param name="serializer">The serializer object.</param>
		public FeedHttpConnection(Uri destination, IFeedSerializer serializer)
		{
			this.Destination = destination;
			this.serializer = serializer;
            ReceiveTimeout = 60000;
		}
		
		#endregion

		#region public methods

		/// <summary>
		/// Performs a syncronous HTTP retreival of the RSS feed.
		/// </summary>
		/// <returns>A Feed object.</returns>
		public Feed Receive()
		{
			Stream stream = this.GetStream(Destination.ToString());
			Feed feed = this.Deserialize(stream);
			this.Close();
			return feed;
		}

		/// <summary>
		/// Starts an asyncronous HTTP feed retreival.
		/// </summary>
		/// <param name="callBack"></param>
		/// <returns>An System.IAsyncResult object that represents the result of the BeginReceive operation.</returns>
		public IAsyncResult BeginReceive(AsyncCallback callBack)
		{
			return SendRequestAsync(callBack);
		}

		/// <summary>
		/// Retrieves the return value of the asynchronous operation represented by the System.IAsyncResult object passed.  
		/// </summary>
		/// <param name="ar">The System.IAsyncResult object that represents a specific invoke asynchronous operation, returned when calling BeginReceive.</param>
		/// <returns>A Feed object.</returns>
		internal Feed EndReceive(IAsyncResult ar)
		{
			ReceiveAsyncResult result = ar as ReceiveAsyncResult;
			Feed feed = this.Deserialize(result.DataStream);
			result.DataStream.Close();
			return feed;
		}

		/// <summary>
		/// Closes the current Response object.
		/// </summary>
		public void Close()
		{
			if (response != null)
			{
				response.Close();
			}
		}

		#endregion

		#region callbacks

		private void ResponseCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				// Set the State of request to asynchronous.
				RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;
				WebRequest myWebRequest1 = myRequestState.request;
				// End the Asynchronous response.
				myRequestState.response = myWebRequest1.EndGetResponse(asynchronousResult);
				// Read the response into a 'Stream' object.
				Stream responseStream = myRequestState.response.GetResponseStream();
				myRequestState.responseStream = responseStream;
				// Begin the reading of the contents of the HTML page and print it to the console.
				IAsyncResult asynchronousResultRead = responseStream.BeginRead(myRequestState.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);

			}
			catch (WebException e)
			{
				Console.WriteLine("WebException raised!");
				Console.WriteLine("\n{0}", e.Message);
				Console.WriteLine("\n{0}", e.Status);
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception raised!");
				//Console.WriteLine("Source : " + e.Source);
				Console.WriteLine("Message : " + e.Message);
			}
		}


		private void ReadCallBack(IAsyncResult asyncResult)
		{
			try
			{
				// Result state is set to AsyncState.
				RequestState myRequestState = (RequestState)asyncResult.AsyncState;
				Stream responseStream = myRequestState.responseStream;
				int read = responseStream.EndRead(asyncResult);
				// Read the contents of the HTML page and then print to the console.
				if (read > 0)
				{
					myRequestState.testData.Append(Encoding.ASCII.GetString(myRequestState.bufferRead, 0, read));
					myRequestState.RequestData.Write(myRequestState.bufferRead, 0, read);
					IAsyncResult asynchronousResult = responseStream.BeginRead(myRequestState.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
				}
				else
				{
					//myRequestState.responseStream = ResponseToMemory(myRequestState.response.GetResponseStream());

					myRequestState.RequestData.Position = 0;
					responseStream.Close();
					//Feed feed = this.Deserialize(myRequestState.RequestData);
					//responseStream.Close();
					//myRequestState.RequestData.Close();

					ReceiveAsyncResult result = new ReceiveAsyncResult(this, myRequestState.RequestData);
					myRequestState.CallBack(result);
				}
			}
			catch (WebException e)
			{
				Console.WriteLine("WebException raised!");
				Console.WriteLine("\n{0}", e.Message);
				Console.WriteLine("\n{0}", e.Status);
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception raised!");
				//				Console.WriteLine("Source : {0}" , e.Source);
				Console.WriteLine("Message : {0}", e.Message);
			}

		}


		#endregion

		#region helper methods

		private IAsyncResult SendRequestAsync(AsyncCallback callBack)
		{
			//StreamReader reader = null;
			HttpWebResponse response = null;
			string result = String.Empty;
			WebRequest webRequest = null;
			IAsyncResult asyncResult = null;

			try
			{
				webRequest = WebRequest.Create(Destination.ToString());

				RequestState requestState = new RequestState();
				// The 'WebRequest' object is associated to the 'RequestState' object.
				requestState.request = webRequest;
				requestState.CallBack = callBack;
				// Start the Asynchronous call for response.
				asyncResult = (IAsyncResult)webRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), requestState);
				//allDone.WaitOne();
				RequestState requestStateOut = (RequestState)asyncResult.AsyncState;
			}
			catch (WebException ex)
			{
				string message = ex.Status.ToString();
				response = (HttpWebResponse)ex.Response;
				if (null != response)
				{
					message = response.StatusDescription;
					response.Close();
				}
				throw ex;
			}
			catch (Exception ex)
			{
				//return GenerateErrorXml(ex.Message);
				throw ex;
			}
			finally
			{
				if (response != null)
				{
					response.Close();
				}
			}

			return asyncResult;
		}

		private static Stream ResponseToMemory(Stream input)
		{
			const int BUFFER_SIZE = 4096;	// 4K read buffer
			MemoryStream output = new MemoryStream();
			int size = BUFFER_SIZE;
			byte[] writeData = new byte[BUFFER_SIZE];
			while (true)
			{
				size = input.Read(writeData, 0, size);
				if (size > 0)
				{
					output.Write(writeData, 0, size);
				}
				else
				{
					break;
				}
			}
			output.Seek(0, SeekOrigin.Begin);
			return output;
		}

		private Feed Deserialize(Stream stream)
		{
			return serializer.Deserialize(stream);
		}

		private Stream GetStream(string url)
		{
			Stream stream = null;
			HttpWebRequest request = null;
			string result = String.Empty;

			try
			{
				request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = ReceiveTimeout;
				response = (HttpWebResponse)request.GetResponse();
				stream = response.GetResponseStream();
			}
			catch (Exception ex)
			{
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }

			return stream;
		}

		private string GetData(string url)
		{
			StreamReader reader = null;
			HttpWebResponse response = null;
			HttpWebRequest request = null;
			string result = String.Empty;

			try
			{
				request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = ReceiveTimeout;
				response = (HttpWebResponse)request.GetResponse();
				reader = new StreamReader(response.GetResponseStream());
				result = reader.ReadToEnd();
			}
			catch (Exception ex)
			{
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
			}
			finally
			{
				if (response != null)
				{
					response.Close();
				}
			}

			return result;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets the Uri destination.
		/// </summary>
		public Uri Destination { get; private set; }
		
		/// <summary>
		/// Gets ot sets the receive timeout.
		/// </summary>
		public int ReceiveTimeout { get; set; }

		#endregion
    }


	#region request state class

	internal class RequestState
	{
		// This class stores the state of the request.
		const int BUFFER_SIZE = 1024;
		private MemoryStream requestData;
		public byte[] bufferRead;
		public WebRequest request;
		public WebResponse response;
		public Stream responseStream;
		public AsyncCallback CallBack;

		public StringBuilder testData;

		public RequestState()
		{
			testData = new StringBuilder();
			bufferRead = new byte[BUFFER_SIZE];
			//requestData = new StringBuilder("");
			request = null;
			responseStream = null;
		}


		public Stream RequestData
		{
			get
			{
				if (requestData == null)	//lazy init. "Redirects", or "Not modified" do not need it immediatly
					requestData = new MemoryStream();
				return requestData;
			}
		}
	} 

	#endregion
}
