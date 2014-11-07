using System;

namespace sc
{
	public class StreamCtx
	{
		// note do not set these variables manually
		// always use StreamCiper.MakeStreamCtx(...)
		private IBlockCipher ibc = null;
		private byte[] iv = null;
		private StreamCipher.Mode mode = StreamCipher.Mode.CBC;
		private bool fReadOnly = false;

		public int BlockSize()
		{
			return ibc.BlockSizeInBytes();
		}

		public IBlockCipher Ibc
		{
			get{ return ibc; }
			set
			{
				CheckReadOnly();
				ibc = value;
			}
		}

		public byte[] IV
		{
			get{ return iv; }
			set
			{
				CheckReadOnly();
				iv = value;
			}
		}

		public StreamCipher.Mode Mode
		{
			get{ return mode; }
			set
			{
				CheckReadOnly();
				mode = value;
			}
		}

		public void MakeReadOnly()
		{
			this.fReadOnly = true;
		}

		private void CheckReadOnly()
		{
			if(this.fReadOnly) throw new Exception();
		}

		public StreamCtx()
		{}
	}//EOC
}
