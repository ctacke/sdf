using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using OpenNETCF.IO;
using System.IO;
using System.Text;

namespace OpenNETCF.IO.Test
{
    [TestClass()]
    public class MemoryMappedFileTest : TestBase
    {
        #region --- In-memory Map Tests ---
        [TestMethod()]
        [Description("Tests CreateInMemoryMap with no parameters to ensure it creates an valid instance")]
        public void TestInMemoryNoParams()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);
            Assert.AreEqual(mmf.Length, MemoryMappedFile.DefaultInMemoryMapSize, "incorrect length");
            Assert.IsTrue(mmf.CanRead, "CanRead failure");
            Assert.IsTrue(mmf.CanWrite, "CanWrite failure");
            Assert.IsTrue(mmf.CanSeek, "CanSeek failure");
            Assert.AreEqual(mmf.Position, 0, "start Position failure");
        }

        [TestMethod()]
        [Description("Tests CreateInMemoryMap with just name parameter to ensure it creates a valid instance")]
        public void TestInMemoryNameOnlyParam()
        {
            string mapName = "testmap";
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap(mapName);
            Assert.IsNotNull(mmf);
            Assert.AreEqual(mmf.Length, MemoryMappedFile.DefaultInMemoryMapSize, "incorrect length");
            Assert.AreEqual(mmf.Name, mapName, "Name failure");
            Assert.IsTrue(mmf.CanRead, "CanRead failure");
            Assert.IsTrue(mmf.CanWrite, "CanWrite failure");
            Assert.IsTrue(mmf.CanSeek, "CanSeek failure");
            Assert.AreEqual(mmf.Position, 0, "start Position failure");
        }

        [TestMethod()]
        [Description("Tests CreateInMemoryMap with just name parameter to ensure it creates a valid instance")]
        public void TestInMemoryAllParams()
        {
            string mapName = "testmap";
            long maxSize = 0x20000; // 128k
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap(mapName, maxSize);
            Assert.IsNotNull(mmf);
            Assert.AreEqual(mmf.Length, maxSize, "Length failure");
            Assert.AreEqual(mmf.Name, mapName, "Name failure");
            Assert.IsTrue(mmf.CanRead, "CanRead failure");
            Assert.IsTrue(mmf.CanWrite, "CanWrite failure");
            Assert.IsTrue(mmf.CanSeek, "CanSeek failure");
            Assert.AreEqual(mmf.Position, 0, "start Position failure");
        }

        [TestMethod()]
        [Description("Verifies an in-memory map can be written to")]
        public void TestInMemoryWritePositive()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            byte[] data = Encoding.ASCII.GetBytes("This is some test data");
            mmf.Write(data, 0, data.Length);

            Assert.AreEqual(mmf.Position, data.Length, "Write did not properly change Position");
        }

        [TestMethod()]
        [Description("Verifies an in-memory map can be read from")]
        public void TestInMemoryReadPositive()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            byte[] data = new byte[10];
            long oldpos = mmf.Position;
            mmf.Read(data, 0, data.Length);

            Assert.AreEqual(oldpos + data.Length, mmf.Position, "Read caused incorrect movement in position");
        }

        [TestMethod()]
        [Description("Verifies an in-memory map can be written to")]
        public void TestBasicReadWritePositive()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            string sourceString = "This is some test data";
            byte[] outdata = Encoding.ASCII.GetBytes(sourceString);
            mmf.Write(outdata, 0, outdata.Length);

            mmf.Seek(-outdata.Length, SeekOrigin.Current);

            byte[] indata = new byte[outdata.Length];
            mmf.Read(indata, 0, indata.Length);

            string targetString = Encoding.ASCII.GetString(indata, 0, indata.Length);

            Assert.AreEqual(sourceString, targetString);
        }

        [TestMethod()]
        [Description("Verifies an in-memory map can be Seeked")]
        public void TestSeekPositive()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            // begin - from begin
            mmf.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(mmf.Position, 0, "Failed Seek begin from begin");

            // the middle - from begin
            long offset = mmf.Length / 2;
            mmf.Seek(offset, SeekOrigin.Begin);
            Assert.AreEqual(mmf.Position, offset, "Failed Seek middle from begin");

            // end - from begin
            mmf.Seek(MemoryMappedFile.DefaultInMemoryMapSize, SeekOrigin.Begin);
            Assert.AreEqual(mmf.Position, MemoryMappedFile.DefaultInMemoryMapSize, "Failed Seek end from begin");

            // begin - from end
            mmf.Seek(-MemoryMappedFile.DefaultInMemoryMapSize, SeekOrigin.End);
            Assert.AreEqual(mmf.Position, 0, "Failed Seek begin from end");

            // middle - from end
            mmf.Seek(-offset, SeekOrigin.End);
            Assert.AreEqual(mmf.Position, offset, "Failed Seek middle from end");

            // end - from end
            mmf.Seek(0, SeekOrigin.End);
            Assert.AreEqual(mmf.Position, MemoryMappedFile.DefaultInMemoryMapSize, "Failed Seek end from end");

            mmf.Seek(offset, SeekOrigin.Begin);
            mmf.Seek(-offset, SeekOrigin.Current);
            Assert.AreEqual(mmf.Position, 0, "Failed Seek begin from current");

            mmf.Close();
        }

        [TestMethod()]
        [Description("Verifies an in-memory map Position can be directly set")]
        public void TestPositionPositive()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            mmf.Position = 100;
            Assert.AreEqual(mmf.Position, 100, "Failed to set Position");

            mmf.Seek(100, SeekOrigin.Current);
            Assert.AreEqual(mmf.Position, 200, "Failed Seek from current");
        }

        [TestMethod()]
        [Description("Verifies that trying to seek to a position before the start of the stream throws an ArgumentOutOfRangeException")]
        public void TestSeekBeforeStart()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            ArgumentOutOfRangeException expected = null;

            try
            {
                mmf.Seek(-1, SeekOrigin.Begin);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);

            expected = null;
            try
            {
                mmf.Seek(-(MemoryMappedFile.DefaultInMemoryMapSize + 1), SeekOrigin.End);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Verifies that trying to seek to a position after the end of the stream throws an ArgumentOutOfRangeException")]
        public void TestSeekAfterEnd()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            ArgumentOutOfRangeException expected = null;

            try
            {
                mmf.Seek(1, SeekOrigin.End);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);

            expected = null;
            try
            {
                mmf.Seek(MemoryMappedFile.DefaultInMemoryMapSize + 1, SeekOrigin.Begin);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Verifies that trying to set the Position to before the start of the stream throws an ArgumentOutOfRangeException")]
        public void TestPositionBeforeStart()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            ArgumentOutOfRangeException expected = null;

            try
            {
                mmf.Position = -1;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Verifies that trying to set the Position to after the end of the stream throws an ArgumentOutOfRangeException")]
        public void TestPositionAfterEnd()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            ArgumentOutOfRangeException expected = null;

            try
            {
                mmf.Position = MemoryMappedFile.DefaultInMemoryMapSize + 1;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Verifies that trying to Read before the start of the stream throws an ArgumentOutOfRangeException")]
        public void TestReadBeforeStart()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            ArgumentOutOfRangeException expected = null;
            mmf.Position = 0;

            byte[] buffer = new byte[10];
            try
            {
                mmf.Read(buffer, -1, buffer.Length);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Verifies that trying to Read past end of the stream throws an EndOfStreamException")]
        public void TestReadPastEnd()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            EndOfStreamException expected = null;
            mmf.Position = MemoryMappedFile.DefaultInMemoryMapSize - 5;

            byte[] buffer = new byte[10];
            try
            {
                mmf.Read(buffer, 0, buffer.Length);
            }
            catch (EndOfStreamException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Verifies that trying to write before the start of the stream throws an ArgumentOutOfRangeException")]
        public void TestWriteBeforeStart()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            ArgumentOutOfRangeException expected = null;
            mmf.Position = 0;

            byte[] buffer = new byte[10];
            try
            {
                mmf.Write(buffer, -1, buffer.Length);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Verifies that trying to write past the end of the stream throws an EndOfStreamException")]
        public void TestWritePastEnd()
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateInMemoryMap();
            Assert.IsNotNull(mmf);

            EndOfStreamException expected = null;
            mmf.Position = MemoryMappedFile.DefaultInMemoryMapSize - 5;

            byte[] buffer = new byte[10];
            try
            {
                mmf.Write(buffer, 0, buffer.Length);
            }
            catch (EndOfStreamException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }
        #endregion

        #region --- File-Backed Map Tests ---
        [TestMethod()]
        [Description("Ensures a call to CreateWithFileBacking with a null name throws an ArgumentNullException")]
        public void TestFileBackedNullName()
        {
            ArgumentNullException expected = null;
            long maxSize = 0x10000;
            try
            {
                MemoryMappedFile mmf = MemoryMappedFile.CreateWithFileBacking(null, true, maxSize);
            }
            catch (ArgumentNullException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures a call to CreateWithFileBacking with an empty name throws an ArgumentException")]
        public void TestFileBackedEmptyName()
        {
            ArgumentException expected = null;
            long maxSize = 0x10000;
            try
            {
                MemoryMappedFile mmf = MemoryMappedFile.CreateWithFileBacking(string.Empty, true, maxSize);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures a call to CreateWithFileBacking with a length < 0 throws an ArgumentException")]
        public void TestFileBackedLessthanZeroLength()
        {
            ArgumentException expected = null;
            string filename = "testfile";

            try
            {
                MemoryMappedFile mmf = MemoryMappedFile.CreateWithFileBacking(filename, true, -1);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }
        #endregion
    }
}
