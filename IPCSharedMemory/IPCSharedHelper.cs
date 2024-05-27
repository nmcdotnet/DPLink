using Cloudtoid.Interprocess;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace IPCSharedMemory
{
    public class IPCSharedHelper : IDisposable
    {
        readonly QueueFactory _Factory;
        readonly QueueOptions _Options;
        readonly string _QueueName;
        readonly long _Capacity;
        readonly IPublisher _Publisher;
        public int countRec = 0;
        public byte[]? ReceiveData;
        public ConcurrentQueue<byte[]> MessageQueue = new();

        // 1024 * 1024 * 100 = 100MB 
        public IPCSharedHelper(int index, string queueName, long capacity = 1024 * 1024 * 100, bool isReceiver = false)
        {
            _QueueName = queueName + (index + 1);
            _Capacity = capacity;
           // _MessageBuffer = messageBuffer;
            _Factory = new();
            _Options = new(queueName: _QueueName, capacity: _Capacity);
            _Publisher = _Factory.CreatePublisher(_Options);
#if DEBUG
           // Console.WriteLine("Device Index: " + index+1);
         //   Console.WriteLine("_queueName: " + _queueName);
#endif
            // Only use for receive data purpose
            if (isReceiver)
            {
              //  ExcuteMesageQueue();
                ListenReceiveData();
            }
        }

        public void SendData(byte[] data)
        {
            try
            {
                _Publisher.TryEnqueue(data);
                // Debug.WriteLine("Enqueue: " + Encoding.ASCII.GetString(data));
            }
            catch (Exception)
            {
                Debug.WriteLine("Send data via IPC fail");
            }
           
        }

        CancellationTokenSource _ctsThreadListenData;
        public void ListenReceiveData()
        {
            Thread listenDataThread = new(() =>
            {
                try
                {
                    _ctsThreadListenData = new();
                    var token = _ctsThreadListenData.Token;
                    using ISubscriber subscriber = _Factory.CreateSubscriber(_Options);
                    while (true)
                    {
                        try
                        {
                            if (token.IsCancellationRequested && MessageQueue.IsEmpty)
                            {
                                Console.WriteLine("Huy Thread !");
                                token.ThrowIfCancellationRequested();
                            }
                            if (subscriber.TryDequeue(default, out ReadOnlyMemory<byte> message))
                            {
                                if (message.Length > 0)
                                {
                                    MessageQueue.Enqueue((byte[])message.ToArray().Clone());
                                    countRec++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            Debug.WriteLine("Error IPC Listen Data" + ex.Message);
#endif
                        }

                        Thread.Sleep(1);
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.WriteLine("Error IPC Listen Data"+ ex.Message);
#endif
                }

            })
            {
                IsBackground = true,
            };
            listenDataThread.Start();
        }

        public void Dispose()
        {
            _Publisher?.Dispose();
            _ctsThreadListenData?.Cancel();
            _ctsThreadListenData?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
