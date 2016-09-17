using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TG.Example
{
    public class MonitorClass
    {
        private Queue<MyMessage> _messageQueue = new Queue<MyMessage>();

        private ParameterizedThreadStart _read = state => ((MonitorClass)state).Heartbeat();
        public void StartHeart()
        {
            var thread = new System.Threading.Thread(this._read, 32 * 1024); // don't need a huge stack
            thread.Priority = ThreadPriority.Normal;
            thread.Name = "TestRead";
            thread.IsBackground = true;
            thread.Start(this);
        }


        public void Heartbeat()
        {
            while (true)
            {
                MyMessage message;
                lock (this._messageQueue)
                {
                    if (this._messageQueue.Count == 0)
                    {
                        System.Threading.Thread.Sleep(200);
                        this.WriteLine("等待发送消息。。。");
                        continue;
                    }
                    message = this._messageQueue.Dequeue();
                    //GetRespone()
                    var response = "it s  done";
                    this.WriteLine("接收消息完成，开始同步结果");
                    message.CompleteSync(response);
                    break;
                }
            }
        }

        public void SendMessage()
        {
            System.Threading.Thread.Sleep(2000);

            var msg = new MyMessage()
            {
                Id = System.Threading.Thread.CurrentThread.ManagedThreadId
            };

            lock (msg)
            {
                this.WriteLine("发送消息");
                this._messageQueue.Enqueue(msg);                

                if (System.Threading.Monitor.Wait(msg, 20000))
                {
                    this.WriteLine("完成同步请求，接收结果：" + msg.Result);
                }
                else
                {
                    this.WriteLine("超时报错");
                }
            }
        }
        
        private void WriteLine(string log)
        {
            Console.WriteLine(string.Format("[{0}] {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, log));
        }
    }
    
    public class MyMessage
    {
        public int Id;

        public string Result;

        public bool CompleteSync(string result)
        {
            lock (this)
            { // tell the waiting thread that we're done
                this.Result = result;
                System.Threading.Monitor.PulseAll(this);
            }
            return true;
        }
    }
}
