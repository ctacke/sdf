using System;
using System.Text;
using OpenNETCF.IO;
using OpenNETCF.Threading;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace MMFPeer
{
    class Program
    {
        public const string SharedMapName = "MMF_PEER_NAME";
        public const long MaxMapSize = 1024;
        public const string SharedMutexName = "MMF_PEER_MUTEX";
        public const string DataReadyEventName = "MMF_PEER_DATA_READY";

        private MemoryMappedFile m_mmf;
        private NamedMutex m_mutex;
        private EventWaitHandle m_dataReady;

        private bool Sending { get; set; }

        static void Main(string[] args)
        {
            new Program().Run();
        }

        public void Run()
        {
            var processName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().GetName().CodeBase);

            // create the MMF
            m_mmf = MemoryMappedFile.CreateInMemoryMap(SharedMapName, MaxMapSize);

            // create a shared mutex
            m_mutex = new NamedMutex(false, SharedMutexName);

            // create a data-ready event
            m_dataReady = new EventWaitHandle(false, EventResetMode.ManualReset, DataReadyEventName);

            // fire up a "listener"
            new System.Threading.Thread(ReadProc)
            {
                IsBackground = true,
                Name = "MMF Peer Reader"
            }
            .Start();

            Console.WriteLine("Memory Mapped File Created.  Enter text to send to peer(s)");

            // wait for user input
            while (true)
            {
                var input = Console.ReadLine();

                if (input == null)
                {
                    Thread2.Sleep(5000);
                    input = GetMockInput();
                    Debug.WriteLine(string.Format("Platform does not have a Console installed. Sending mock data '{0}'", input));
                }

                if (input == "exit") break;

                // prefix our process name so we can tell who sent the data
                input = processName + ":" + input;

                // grab the mutex
                if (!m_mutex.WaitOne(5000, false))
                {
                    Console.WriteLine("Unable to acquire mutex.  Send Abandoned");
                    Debug.WriteLine("Unable to acquire mutex.  Send Abandoned");
                    continue;
                }

                // mark as "sending" so the listener will ignore what we send
                Sending = true;

                // create a "packet" (length + data)
                var packet = new byte[4 + input.Length];
                Buffer.BlockCopy(BitConverter.GetBytes(input.Length), 0, packet, 0, 4);
                Buffer.BlockCopy(Encoding.ASCII.GetBytes(input), 0, packet, 4, input.Length);

                // write the packet at the start
                m_mmf.Seek(0, System.IO.SeekOrigin.Begin);
                m_mmf.Write(packet, 0, packet.Length);

                // notify all clients that data is ready (manual reset events will release all waiting clients)
                m_dataReady.Set();

                // yield to allow the receiver to unblock and check the "Sending" flag
                Thread2.Sleep(1);

                // reset the event
                m_dataReady.Reset();

                // unmark "sending" 
                Sending = false;

                // release the mutex
                m_mutex.ReleaseMutex();
            }            
        }

        private int m_inputIndex = -1;
        private string[] m_inputs = new string[]
            {
                "Hello",
                "World"
            };

        private string GetMockInput()
        {
            if (++m_inputIndex >= m_inputs.Length) m_inputIndex = 0;
            return m_inputs[m_inputIndex];
        }

        private void ReadProc()
        {
            var lengthBuffer = new byte[4];
            var dataBuffer = new byte[m_mmf.Length - 4];

            while (true)
            {
                // use a timeout so if the app ends, this thread can exit
                if(!m_dataReady.WaitOne(1000, false)) continue;

                // avoid receiving our own data
                if (Sending)
                {
                    // wait long enough for the sender to reset the m_dataReady flag
                    Thread2.Sleep(5);
                    continue;
                }

                // grab the mutex to prevent concurrency issues
                m_mutex.WaitOne(1000, false);

                // read from the start
                m_mmf.Seek(0, System.IO.SeekOrigin.Begin);

                // get the length
                m_mmf.Read(lengthBuffer, 0, 4);
                var length = BitConverter.ToInt32(lengthBuffer, 0);

                // get the data
                m_mmf.Read(dataBuffer, 0, length);

                // release the mutex so any other clients can receive
                m_mutex.ReleaseMutex();

                // convert to a string
                var received = Encoding.ASCII.GetString(dataBuffer, 0, length);

                Console.WriteLine("Received: " + received);
                Debug.WriteLine("Received: " + received);
            }
        }
    }
}
