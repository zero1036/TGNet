using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
