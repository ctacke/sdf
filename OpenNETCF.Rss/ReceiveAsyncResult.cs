

using System;
using System.IO;
using System.Text;

namespace OpenNETCF.Rss
{
	
	internal class ReceiveAsyncResult : IAsyncResult
	{
		private Stream stream;
		private FeedHttpConnection connection;

		public ReceiveAsyncResult(FeedHttpConnection connection, Stream stream)
		{
			this.connection = connection;
			this.stream = stream;
		}

		public Stream DataStream
		{
			get
			{
				return stream;
			}
		}

		public FeedHttpConnection Connection
		{
			get
			{
				return connection;
			}
		}



		#region IAsyncResult Members

		public object AsyncState
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public System.Threading.WaitHandle AsyncWaitHandle
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool CompletedSynchronously
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool IsCompleted
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion
}
}
