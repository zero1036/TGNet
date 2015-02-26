using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace TG.Example
{
    public class ThreadClass
    {
        public static void ThreadQueueTest()
        {
            int iAcountCount = 100;
            double iLimitMoney = 1000;

            Bank_Queue pBank = new Bank_Queue(iAcountCount, iLimitMoney);

            Thread pThBack = new Thread(pBank.TransferX);
            Thread pThBack2 = new Thread(pBank.Listener);
            pThBack.Start();
            pThBack2.Start();

            for (int i = 0; i < iAcountCount; i++)
            {
                BankClient clie = new BankClient(pBank, i, iLimitMoney);
                Thread thread = new Thread(clie.run);
                thread.Start();
            }
        }
    }
    public class Transformer : IComparable<Transformer>
    {
        public int CompareTo(Transformer other)
        {
            // 数字小，优先级高
            //return this.Money > other.Money ? 1 : this.Money < other.Money ? -1 : 0;
            // 数字大，优先级高---转账金额高的vip客户优先
            return this.Money < other.Money ? 1 : this.Money > other.Money ? -1 : 0;
        }

        public int From
        { get; set; }
        public int To
        { get; set; }
        public double Money
        { get; set; }
        public string Name
        { get; set; }
    }
    public class BankClient
    {
        private Bank_Queue m_bBank;
        private int m_iFromAccount;
        private double m_maxAmount;
        private int m_Delay = 100;

        public BankClient(Bank_Queue pBank, int iFromAccount, double dbMaxAccount)
        {
            this.m_bBank = pBank;
            // this.m_pQueue = pQueue;
            this.m_iFromAccount = iFromAccount;
            this.m_maxAmount = dbMaxAccount;
        }

        public void run()
        {

            while (true)
            {
                try
                {

                    Random pRam = new Random();
                    // 确保转出与转入账户不一样
                    int toAccount = pRam.Next(1, m_bBank.size);
                    while (toAccount == this.m_iFromAccount)
                    {
                        toAccount = pRam.Next(1, m_bBank.size);
                    }
                    double dbTransferMoney = pRam.NextDouble() * m_maxAmount;


                    Transformer pTransformer = new Transformer();
                    pTransformer.From = m_iFromAccount;
                    pTransformer.To = toAccount;
                    pTransformer.Money = Math.Round(dbTransferMoney, 2);
                    //pTransformer.Name=Thread.().getName();

                    // 阻塞队列的方式，客户类只执行将自己的转账需求登入阻塞队列当中，银行自己
                    // 的单独线程另外根据多个线程客户登入的队列，逐个处理转账
                    m_bBank.m_Queue.Enqueue(pTransformer);
                    // 按照上锁的方式，客户类应该再此执行银行转账操作，并转出转入资金
                    // m_bBank.Transfer(m_iFromAccount, toAccount, amount);


                    Thread.Sleep(pRam.Next(1000, 1000 * 3));
                }
                catch (Exception e)
                {
                    // TODO: handle exception
                    //System.out.printf("错误信息%s%n", e.getMessage());
                }
            }
        }

    }
    public class Bank_Queue : Bank
    {
        //public BlockingQueue<Transformer> m_Queue;
        public ConcurrentQueue<Transformer> m_Queue;

        public Bank_Queue(int i, double dbAcountBalance)
            : base(i, dbAcountBalance)
        {
            //m_Queue = new BlockingQueue<Transformer>(base.size);
            m_Queue = new ConcurrentQueue<Transformer>();
        }

        public void TransferX()
        {

            try
            {
                bool done = false;

                while (!done)
                {
                    Console.WriteLine(string.Format("队列数：{0}", m_Queue.Count));
                    //Transformer pTransformer = m_Queue.Dequeue();
                    Transformer pTransformer = new Transformer();
                    if (!m_Queue.TryDequeue(out pTransformer))
                        continue;

                    if (this.m_AllAcount[pTransformer.From] < pTransformer.Money)
                    {
                        Console.WriteLine(string.Format("@@@@@@@@@@{0}账户余额{1}不足{2}，操作无效", pTransformer.From, this.m_AllAcount[pTransformer.From], pTransformer.Money));
                        continue;
                    }

                    Console.WriteLine(string.Format("线程名称：{0}", pTransformer.Name));
                    this.m_AllAcount[pTransformer.From] -= pTransformer.Money;
                    Console.WriteLine(string.Format("————{0}元从{1}账户转账至{2}账户——全账户移出总额{3} ", pTransformer.Money, pTransformer.To, pTransformer.From, this.total));
                    this.m_AllAcount[pTransformer.To] += pTransformer.Money;
                    Console.WriteLine(string.Format("————全账户总额{0}", this.total));

                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {

            }
        }

        public void Listener()
        {
            try
            {
                while (true)
                {

                    for (int i = 0; i < this.m_AllAcount.Length; i++)
                    {
                        if (m_AllAcount[i] <= 0)
                            Console.WriteLine(string.Format("@@@@@@@@@@@警告{0}账户余额{1}", i, m_AllAcount[i]));
                    }
                    Thread.Sleep(1000);
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
    public class Bank
    {
        protected double[] m_AllAcount;

        public Bank(int n, double dbAcountBalance)
        {
            m_AllAcount = new double[n];
            for (int i = 0; i < n; i++)
            {
                m_AllAcount[i] = dbAcountBalance;
            }
        }

        public int size
        {
            get
            {
                return m_AllAcount.Length;
            }
        }

        public double total
        {
            get
            {
                double sum = 0;
                foreach (double dbValue in m_AllAcount)
                {
                    sum += dbValue;
                }
                return sum;
            }
        }
    }

}
