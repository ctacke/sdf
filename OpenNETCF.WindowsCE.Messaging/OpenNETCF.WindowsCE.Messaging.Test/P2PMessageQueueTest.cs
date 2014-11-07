using OpenNETCF.WindowsCE.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.WindowsCE.Messaging.Test
{
    [TestClass()]
    public class P2PMessageQueueTest : TestBase
    {
        private const int SYS_HANDLE_BASE = 64;
        private const int SH_CURPROC = 2;

        private static IntPtr GetCurrentProcessHandle()
        {
            return new IntPtr(SH_CURPROC + SYS_HANDLE_BASE);
        }

        [TestMethod()]
        [Description("Test the static OpenExisting method to ensure invalid handle inputs throw ArgumentExceptions")]
        public void OpenExistingBadHandleTest()
        {
            ArgumentException expected = null;

            // create a "valid" (non zero, non negative 1) handle
            IntPtr validHandleValue = new IntPtr(0x8000000);
            IntPtr zeroHandle = IntPtr.Zero;
            IntPtr minusOneHandle = new IntPtr(-1);

            try
            {
                P2PMessageQueue.OpenExisting(true, zeroHandle, validHandleValue);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);

            expected = null;
            try
            {
                P2PMessageQueue.OpenExisting(true, minusOneHandle, validHandleValue);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);

            expected = null;
            try
            {
                P2PMessageQueue.OpenExisting(true, validHandleValue, zeroHandle);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);

            expected = null;
            try
            {
                P2PMessageQueue.OpenExisting(true, validHandleValue, minusOneHandle);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Test the P2PMessageQueue constructor to ensure a name longer than supported by the OS throws an ArgumentException")]
        public void CTorNameTooLongTest()
        {
            ArgumentException expected = null;

            P2PMessageQueue queue = null;
            string longName = new string('N', 261);

            try
            {
                queue = new P2PMessageQueue(true, longName);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Test the P2PMessageQueue constructor to ensure a non-positive message length throws an ArgumentException")]
        public void CTorInvalidMessageLengthTest()
        {
            ArgumentException expected = null;

            P2PMessageQueue queue = null;

            try
            {
                queue = new P2PMessageQueue(true, "queueName", 0, 10);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);

            expected = null;
            try
            {
                queue = new P2PMessageQueue(true, "queueName", -1, 10);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Test the P2PMessageQueue constructor to ensure a negative value for max messages throws an ArgumentException")]
        public void CTorInvalidMaxMessagesTest()
        {
            ArgumentException expected = null;

            P2PMessageQueue queue = null;

            try
            {
                queue = new P2PMessageQueue(true, "queueName", 4096, -1);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Test the P2PMessageQueue constructor to ensure that providing valid inputs yields *valid* construction of a inbound queue")]
        public void CTorReadQueuePositiveTest()
        {
            string queueName = "readQueueName";
            int maxMessageLength = 0x1234;
            int maxMessageCount = 0x56;

            P2PMessageQueue queue = new P2PMessageQueue(true, queueName, maxMessageLength, maxMessageCount);
            Assert.IsNotNull(queue);

            // property checks
            Assert.IsTrue(queue.CanRead, "CanRead failed");
            Assert.IsFalse(queue.CanWrite, "CanWrite failed");
            Assert.AreEqual(queue.MaxMessageLength, maxMessageLength, "MaxMessageLength failed");
            Assert.AreEqual(queue.MaxMessagesAllowed, maxMessageCount, "MaxMessagesAllowed failed");
            Assert.AreEqual(queue.QueueName, queueName, "QueueName failed");
            Assert.AreEqual(queue.MessagesInQueueNow, 0, "MessagesInQueueNow failed");
            Assert.AreEqual(queue.MostMessagesSoFar, 0, "MostMessagesSoFar failed");
            Assert.AreEqual(queue.CurrentReaders, 1, "CurrentReaders failed");  // we're a reader on this queue
            Assert.AreEqual(queue.CurrentWriters, 0, "CurrentWriters failed");
            Assert.AreNotEqual(queue.Handle, 0, "Handle failed");
            Assert.AreNotEqual(queue.Handle, -1, "Handle failed");
        }

        [TestMethod()]
        [Description("Test the P2PMessageQueue constructor to ensure that providing valid inputs yields *valid* construction of a outbound queue")]
        public void CTorWriteQueuePositiveTest()
        {
            string queueName = "writeQueueName";
            int maxMessageLength = 0xABCD;
            int maxMessageCount = P2PMessageQueue.InfiniteQueueSize;

            P2PMessageQueue queue = new P2PMessageQueue(false, queueName, maxMessageLength, maxMessageCount);
            Assert.IsNotNull(queue);

            // property checks
            Assert.IsFalse(queue.CanRead, "CanRead failed");
            Assert.IsTrue(queue.CanWrite, "CanWrite failed");
            Assert.AreEqual(queue.MaxMessageLength, maxMessageLength, "MaxMessageLength failed");
            Assert.AreEqual(queue.MaxMessagesAllowed, maxMessageCount, "MaxMessagesAllowed failed");
            Assert.AreEqual(queue.QueueName, queueName, "QueueName failed");
            Assert.AreEqual(queue.MessagesInQueueNow, 0, "MessagesInQueueNow failed");
            Assert.AreEqual(queue.MostMessagesSoFar, 0, "MostMessagesSoFar failed");
            Assert.AreEqual(queue.CurrentReaders, 0, "CurrentReaders failed");
            Assert.AreEqual(queue.CurrentWriters, 1, "CurrentWriters failed"); // we're a writer on this queue
            Assert.AreNotEqual(queue.Handle, 0, "Handle failed");
            Assert.AreNotEqual(queue.Handle, -1, "Handle failed");
        }

        [TestMethod()]
        [Description("Ensures that two queues with the same name see one another")]
        public void CTorSameNameCheckTest()
        {
            string queueName = "queueName";
            int maxMessageLength = 0xABCD;
            int maxMessageCount = P2PMessageQueue.InfiniteQueueSize;

            // create a matched pair 
            P2PMessageQueue readQueue = new P2PMessageQueue(true, queueName, maxMessageLength, maxMessageCount);
            Assert.IsNotNull(readQueue);

            P2PMessageQueue writeQueue = new P2PMessageQueue(false, queueName, maxMessageLength, maxMessageCount);
            Assert.IsNotNull(readQueue);

            Assert.AreEqual(readQueue.CurrentReaders, 1, "readQueue CurrentReaders failed");
            Assert.AreEqual(readQueue.CurrentWriters, 1, "readQueue CurrentWriters failed");
            Assert.AreEqual(writeQueue.CurrentReaders, 1, "writeQueue CurrentReaders failed");
            Assert.AreEqual(writeQueue.CurrentWriters, 1, "writeQueue CurrentWriters failed");
        }

        [TestMethod()]
        [Description("Ensures that the created new output parameter on the ctor behaves properly")]
        public void CTorCreatedNewTest()
        {
            string queueName = "cnQueue";
            int maxMessageLength = 0xABCD;
            int maxMessageCount = P2PMessageQueue.InfiniteQueueSize;
            bool createdNew;

            P2PMessageQueue originalQueue = new P2PMessageQueue(true, queueName, maxMessageLength, maxMessageCount, out createdNew);
            Assert.IsNotNull(originalQueue);
            Assert.IsTrue(createdNew);

            P2PMessageQueue copyQueue = new P2PMessageQueue(true, queueName, maxMessageLength, maxMessageCount, out createdNew);
            Assert.IsNotNull(copyQueue);
            Assert.IsFalse(createdNew);
        }

        [TestMethod()]
        [Description("Ensures that the created new output parameter on the ctor behaves properly")]
        public void OpenExistingPositiveTest()
        {
            P2PMessageQueue originalQueue = new P2PMessageQueue(true);
            Assert.IsNotNull(originalQueue);

            P2PMessageQueue copyQueue = P2PMessageQueue.OpenExisting(false, GetCurrentProcessHandle(), originalQueue.Handle);
            Assert.IsNotNull(copyQueue);
        }

        [TestMethod()]
        [Description("Ensures that trying to send a null Message throws an ArgumentNullException")]
        public void SendNullTest()
        {
            P2PMessageQueue queue = new P2PMessageQueue(true);
            Assert.IsNotNull(queue);

            ArgumentNullException expected = null;

            try
            {
                queue.Send(null);
            }
            catch (ArgumentNullException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that trying to send a Message with null or empty data throws an ArgumentException")]
        public void SendNullDataTest()
        {
            P2PMessageQueue queue = new P2PMessageQueue(true);
            Assert.IsNotNull(queue);

            ArgumentException expected = null;
            Message msg = new Message();

            try
            {
                queue.Send(null);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }
            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that trying to send a Message with null or empty data throws an ArgumentException")]
        public void SendEmptyDataTest()
        {
            P2PMessageQueue queue = new P2PMessageQueue(true);
            Assert.IsNotNull(queue);

            ArgumentException expected = null;
            Message msg = new Message();  
            msg.MessageBytes = new byte[0];

            try
            {
                queue.Send(msg);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }
    }
}
