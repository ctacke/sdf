using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OpenNETCF.Runtime.InteropServices.ComTypes;

namespace OpenNETCF.Drawing.Imaging
{
    public class StreamOnFile: IStream, IDisposable
    {
        private Stream m_str;
        public StreamOnFile(Stream st)
        {
            m_str = st;
        }

        public StreamOnFile(string fileName)
        {
            m_str = File.OpenRead(fileName);
        }

        #region IStream Members

        public void Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            int cbRead = m_str.Read(pv, 0, cb);
            if (pcbRead != IntPtr.Zero)
                Marshal.WriteInt32(pcbRead, cbRead);
        }

        public void Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            m_str.Write(pv, 0, cb);
            if (pcbWritten != IntPtr.Zero)
                Marshal.WriteInt32(pcbWritten, cb);
        }

        public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            long lPos = m_str.Seek(dlibMove, (SeekOrigin)dwOrigin);
            if ( plibNewPosition != IntPtr.Zero )
                Marshal.WriteInt64(plibNewPosition, lPos);
        }

        public void SetSize(long libNewSize)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Commit(int grfCommitFlags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Revert()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void LockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UnlockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Stat(out STATSTG pstatstg, int grfStatFlag)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clone(out IStream ppstm)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (m_str != null)
                    m_str.Close();
            }
            catch { }
        }

        #endregion
    }
}
