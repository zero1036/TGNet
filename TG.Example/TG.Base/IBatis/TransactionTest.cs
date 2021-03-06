﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBatisNet.DataMapper;

namespace TG.Example
{
    public class TransactionTest
    {
        public void Commit()
        {
            User account = new User()
            {
                UserId = "123",
                Tid = 3
            };

            ISqlMapper sqlMap = Mapper.GetMaper;

            sqlMap.BeginTransaction();

            Object obj = sqlMap.Insert("User.sql_InsertOne", account);

            sqlMap.CommitTransaction();
        }


        public void Rollback()
        {
            User account = new User()
            {
                UserId = "tgor",
                Tid = 4
            };

            ISqlMapper sqlMap = Mapper.GetMaper;

            sqlMap.BeginTransaction();

            Object obj = sqlMap.Insert("User.sql_InsertOne", account);

            sqlMap.RollBackTransaction();
        }
    }
}
