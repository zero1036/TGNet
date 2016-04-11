using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TG.Example
{
    public class UserService
    {
        public int TestInsertOne(User account)
        {
            Object obj = Mapper.GetMaper.Insert("User.sql_InsertOne", account);
            return (int)obj;
        }

        public User GetUser(int tid)
        {
            return (User)Mapper.GetMaper.QueryForObject("User.sql_selectByid", tid);
        }

        public IList<User> GetAccountList()
        {
            return Mapper.GetMaper.QueryForList<User>("User.sql_selectAll", null);
        }


        public int InsertOne()
        {
            Random rand = new Random();
            int tid = rand.Next(1, 20);
            object res = Mapper.GetMaper.Insert("User.sql_InsertOne", new User() { Tid = tid });
            return res != null ? 1 : 0;
        }

        public int Update()
        {
            for (int i = 1; i <= 100; i++)
            {
                Thread th = new Thread(Doit);

                th.Start();
            }
            return 1;
        }

        public void Doit()
        {
            Random rand = new Random();
            int count = 1;
            while (count <= 100000)
            {
                int tid = rand.Next(1, 1000);
                Hashtable hs = new Hashtable();
                hs.Add("sysuserid", 1);
                hs.Add("tid", tid);

                User user = (User)Mapper.GetMaper.QueryForObject("User.sql_selectByid", tid);

                int ou = Mapper.GetMaper.Update("User.sql_update", hs);
                Console.WriteLine("result:" + ou + "|tid:" + tid);
                count += 1;
            }
        }       
    }
}
