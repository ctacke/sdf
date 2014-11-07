using OpenNETCF.WindowsCE.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Text;

namespace OpenNETCF.WindowsCE.Messaging.Integration.Test
{
    [TestClass()]
    public class P2PMessageQueueTest : TestBase
    {
        [TestMethod()]
        [Description("Ensures that a pair of queues can send data from one to the other and that queue properties are correctly updated for that message")]
        [Priority(5)]
        public void SynchronousSendReceiveTest()
        {
            string messageToSend = "This is a test message";            
            Message outbound = new Message(Encoding.ASCII.GetBytes(messageToSend));

            string queueName = "queue";

            P2PMessageQueue sendQueue = new P2PMessageQueue(false, queueName);
            Assert.IsNotNull(sendQueue, "ctor failed");

            P2PMessageQueue recvQueue = new P2PMessageQueue(true, queueName);
            Assert.IsNotNull(recvQueue, "ctor failed");

            Message inbound = new Message();

            ReadWriteResult result = sendQueue.Send(outbound);
            Assert.AreEqual(result, ReadWriteResult.OK, "Send failed");

            Assert.AreEqual(recvQueue.MessagesInQueueNow, 1, "MessagesInQueueNow failure (not added)");

            result = recvQueue.Receive(inbound, 1000);
            Assert.AreEqual(result, ReadWriteResult.OK, "Message was not received");

            string messageReceived = Encoding.ASCII.GetString(inbound.MessageBytes, 0, inbound.MessageBytes.Length);
            Assert.AreEqual(messageToSend, messageReceived, "Received data did not match sent data");

            Assert.AreEqual(recvQueue.MessagesInQueueNow, 0, "MessagesInQueueNow failure (not removed)");
            Assert.AreEqual(recvQueue.MostMessagesSoFar, 1, "MostMessagesSoFar failure");
        }
    }
}
