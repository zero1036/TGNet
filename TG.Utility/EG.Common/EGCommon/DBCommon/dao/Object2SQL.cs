using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.ComponentModel;

namespace EG.Utility.DBCommon.dao
{
    public class Object2SQL
    {
        protected readonly static short TODO_INSERT = 1;
        protected readonly static short TODO_UPDATE = 2;
        protected readonly static short TODO_DELETE = 3;
        protected readonly static short TODO_SELECT = 4;

        protected object _entity { get; set; }
        protected Type _type { get; set; }
        protected short dbType { get; set; }
        protected char varPrefix { get; set; }
        public string Sql { get; set; }
        public short Todo { get; set; }

        public bool IsSqlServerIdentityTable { get; set; }


        public IList<String> ParameterNames { get; set; }
        public IList<Object> ParameterValues { get; set; }

        public IList<String> KeyNames { get; set; }
        public IList<Object> KeyValues { get; set; }

        public IList<String> OutputNames { get; set; }
        public IList<Object> OutputValues { get; set; }
        public ADOTemplate adoTemplate { get; set; }


        protected String GetTableName()
        {

            string tableName = _type.GetAttributeValue((TableAttribute ta) => ta.Name);
            if (tableName == null)
            {
                return _type.Name;
            }

            return tableName;
        }


        public void parse(object entity)
        {
            this._entity = entity;
            this._type = entity.GetType();

            SetDBType();

            //获得实体的属性集合 
            PropertyInfo[] props = _type.GetProperties();

            this.ParameterNames = new List<String>(props.Length);
            this.ParameterValues = new List<Object>(props.Length);

            this.OutputNames = new List<String>(props.Length);
            this.OutputValues = new List<Object>(props.Length);

            this.KeyNames = new List<String>(1);
            this.KeyValues = new List<Object>(1);

            this.IsSqlServerIdentityTable = false;

            foreach (PropertyInfo prop in props)
            {
                string colName = prop.Name;
                object colValue = prop.GetValue(entity, null);
                bool isPK = false;

                //if is keyattribute return key
                object[] columnAttrs = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                ColumnAttribute colAttr = null;
                if (columnAttrs.Length > 0)
                {
                    colAttr = columnAttrs[0] as ColumnAttribute;
                    if (colAttr.Name != null)
                    {
                        colName = colAttr.Name;
                    }
                    isPK = colAttr.IsPrimaryKey;

                    if (Object2SQL.TODO_INSERT == Todo)
                    {
                        if (ADOTemplate.DB_TYPE_SQLSERVER == this.dbType
                            && colAttr.IsDbGenerated
                            && !IsSqlServerIdentityTable)
                        {
                            IsSqlServerIdentityTable = true;
                            OutputNames.Add(colName);
                        }

                        // 如果是oracle时，如果有seq定义，则取seq
                        else if (ADOTemplate.DB_TYPE_ORACLE == this.dbType
                            && colAttr.Expression != null
                            && (colValue == null || colValue == (object)0))
                        {
                            String seqSql = String.Format("select {0}.nextval from dual", colAttr.Expression);

                            colValue = adoTemplate.GetLong(seqSql);
                            OutputNames.Add(colName);
                            OutputValues.Add(colValue);
                            // 设置获取的Seq值到属性中
                            // prop.SetValue(entity, colValue, null);
                        }
                    }

                    if (isPK)
                    {
                        this.KeyNames.Add(colName);
                        this.KeyValues.Add(colValue);
                    }
                    else
                    {
                        if (colValue != null || Object2SQL.TODO_UPDATE == Todo)
                        {
                            this.ParameterNames.Add(colName);
                            this.ParameterValues.Add(colValue);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 如果有Output字段（SqlServer中的Identity字段，Oracle的Seqc字段），
        /// 插入DB后用这个方法设置Model这些字段的值
        /// </summary>
        /// <param name="entity"></param>
        public void SetOutputValues(object entity)
        {
            PropertyInfo[] props = _type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                string colName = prop.Name;

                //if is keyattribute return key
                object[] columnAttrs = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                ColumnAttribute colAttr = null;
                if (columnAttrs.Length > 0)
                {
                    colAttr = columnAttrs[0] as ColumnAttribute;
                    if (colAttr.Name != null)
                    {
                        colName = colAttr.Name;
                    }

                    if (Object2SQL.TODO_INSERT == Todo &&
                        OutputNames.Contains(colName))
                    {
                        int index = OutputNames.IndexOf(colName);
                        object outputValue = OutputValues[index];

                        // 设置DB中生成的值到属性中
                        if (outputValue.GetType() != prop.PropertyType)
                        {
                            object a = ChangeType(outputValue, prop.PropertyType);
                            prop.SetValue(entity, a, null);
                        }
                        else
                        {
                            prop.SetValue(entity, outputValue, null);
                        }
                    }
                }
            }
        }

        private object ChangeType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value != null)
                {
                    NullableConverter nullableConverter = new NullableConverter(conversionType);
                    conversionType = nullableConverter.UnderlyingType;
                }
                else
                {
                    return null;
                }
            }

            return Convert.ChangeType(value, conversionType);
        }

        public ICollection<object> Conver2ObjectCollection(object entity)
        {
            if (entity is ICollection<long>)
            {
                ICollection<long> tempCollection = entity as ICollection<long>;

                IList<object> result = new List<object>(tempCollection.Count);

                foreach (long e in tempCollection)
                {
                    result.Add(e);
                }

                return result;
            }
            else if (entity is ICollection<int>)
            {
                ICollection<int> tempCollection = entity as ICollection<int>;

                IList<object> result = new List<object>(tempCollection.Count);

                foreach (int e in tempCollection)
                {
                    result.Add(e);
                }

                return result;
            }
            else if (entity is ICollection<double>)
            {
                ICollection<double> tempCollection = entity as ICollection<double>;

                IList<object> result = new List<object>(tempCollection.Count);

                foreach (double e in tempCollection)
                {
                    result.Add(e);
                }

                return result;
            }
            else if (entity is ICollection<float>)
            {
                ICollection<float> tempCollection = entity as ICollection<float>;

                IList<object> result = new List<object>(tempCollection.Count);

                foreach (double e in tempCollection)
                {
                    result.Add(e);
                }

                return result;
            }

            return entity as ICollection<object>;
        }

        public void SetDBType()
        {
            TransactionContext context = TransactionContext.get();
            this.dbType = context == null ? ADOTemplate.DB_TYPE_SQLSERVER : context.dbType;
        }

    }


    public class Object2Delete : Object2SQL
    {
        public string AsSql()
        {
            StringBuilder result = new StringBuilder();

            this.SetDBType();

            this.varPrefix = DBUtil.GetDBParamFlag(this.dbType);

            result.Append("delete from ");
            result.Append(this.GetTableName());
            result.Append(" where ");

            int size = this.KeyNames.Count;
            for (int i = 0; i < size; i++)
            {
                if (i != 0)
                {
                    result.Append(" and ");
                }

                result.Append(this.KeyNames[i]);
                result.Append("=");
                result.Append(varPrefix);
                result.Append(this.KeyNames[i]);
            }

            return result.ToString();
        }

        public String[] GetSqlParameterNames()
        {
            return this.KeyNames.ToArray<String>();
        }

        public Object[] GetSqlParameterValues()
        {
            return this.KeyValues.ToArray<Object>();
        }
    }


    public class Object2Find : Object2SQL
    {
        public string AsSql()
        {
            StringBuilder result = new StringBuilder();

            this.SetDBType();

            this.varPrefix = DBUtil.GetDBParamFlag(this.dbType);

            result.Append("select * from ");
            result.Append(this.GetTableName());
            result.Append(" where ");

            int size = this.ParameterNames.Count;
            for (int i = 0; i < size; i++)
            {
                if (i != 0)
                {
                    result.Append(" and ");
                }

                result.Append(this.ParameterNames[i]);
                result.Append("=");
                result.Append(varPrefix);
                result.Append(this.ParameterNames[i]);
            }

            return result.ToString();
        }

        public String[] GetSqlParameterNames()
        {
            return this.ParameterNames.ToArray<String>();
        }

        public Object[] GetSqlParameterValues()
        {
            return this.ParameterValues.ToArray<Object>();
        }
    }



    public class Object2Get : Object2SQL
    {
        public string AsSql()
        {
            StringBuilder result = new StringBuilder();

            this.SetDBType();

            this.varPrefix = DBUtil.GetDBParamFlag(this.dbType);

            result.Append("select * from ");
            result.Append(this.GetTableName());
            result.Append(" where ");

            int size = this.KeyNames.Count;
            for (int i = 0; i < size; i++)
            {
                if (i != 0)
                {
                    result.Append(" and ");
                }

                result.Append(this.KeyNames[i]);
                result.Append("=");
                result.Append(varPrefix);
                result.Append(this.KeyNames[i]);
            }

            return result.ToString();
        }

        public String[] GetSqlParameterNames()
        {
            return this.KeyNames.ToArray<String>();
        }

        public Object[] GetSqlParameterValues()
        {
            return this.KeyValues.ToArray<Object>();
        }
    }

    public class Object2Insert : Object2SQL
    {
        private bool hasPK = false;

        public Object2Insert()
            : base()
        {
            this.Todo = Object2SQL.TODO_INSERT;
        }

        public string AsSql()
        {

            this.SetDBType();

            this.varPrefix = DBUtil.GetDBParamFlag(this.dbType);

            StringBuilder insertSQL = new StringBuilder();
            StringBuilder valueSQL = new StringBuilder();

            insertSQL.Append("insert into ").Append(this.GetTableName()).Append("(");
            valueSQL.Append(")values(");


            int size = this.ParameterNames.Count;
            for (int i = 0; i < size; i++)
            {
                if (i != 0)
                {
                    insertSQL.Append(", ");
                    valueSQL.Append(", ");
                }

                insertSQL.Append(this.ParameterNames[i]);
                valueSQL.Append(varPrefix).Append(this.ParameterNames[i]);
            }

            size = this.KeyNames.Count;
            for (int i = 0; i < size; i++)
            {
                if (this.KeyValues[i] == null)
                {
                    continue;// skip null PK
                }

                hasPK = true;
                insertSQL.Append(", ").Append(this.KeyNames[i]);
                valueSQL.Append(", ").Append(varPrefix).Append(this.KeyNames[i]);
            }
            insertSQL.Append(valueSQL).Append(")");

            return insertSQL.ToString();
        }

        /// <summary>
        /// 返回执行的Sql语句。
        /// 如果是SqlServer，而且包括自增长（Identity）列时，会返回 @@rowcount和@@identity
        /// </summary>
        /// <returns></returns>
        public string AsSql4ServerIdentityTable()
        {
            string sql = AsSql();
            return sql + ";select convert(int, @@rowcount), convert(int, @@identity)";
        }

        public String[] GetSqlParameterNames()
        {
            if (hasPK)
            {
                String[] result = new String[this.KeyNames.Count + this.ParameterNames.Count];

                this.ParameterNames.CopyTo(result, 0);
                this.KeyNames.CopyTo(result, this.ParameterNames.Count);

                return result;
            }
            else
            {
                return this.ParameterNames.ToArray();
            }
        }

        public Object[] GetSqlParameterValues()
        {
            if (hasPK)
            {
                Object[] result = new Object[this.KeyValues.Count + this.ParameterValues.Count];

                this.ParameterValues.CopyTo(result, 0);
                this.KeyValues.CopyTo(result, this.ParameterValues.Count);

                return result;
            }
            else
            {
                return this.ParameterValues.ToArray();
            }
        }
    }

    public class Object2Update : Object2SQL
    {
        public Object2Update()
            : base()
        {
            this.Todo = Object2SQL.TODO_UPDATE;
        }
        public string AsSql()
        {
            this.SetDBType();

            this.varPrefix = DBUtil.GetDBParamFlag(this.dbType);

            StringBuilder updateSQL = new StringBuilder();

            updateSQL.Append("update ").Append(this.GetTableName());

            int size = this.ParameterNames.Count;
            for (int i = 0; i < size; i++)
            {
                if (i == 0)
                {
                    updateSQL.Append(" set ");
                }
                else
                {
                    updateSQL.Append(", ");
                }
                updateSQL.Append(
                    this.ParameterNames[i]).Append("=").Append(this.varPrefix).Append(this.ParameterNames[i]);
            }

            size = this.KeyNames.Count;
            for (int i = 0; i < size; i++)
            {
                if (i != 0)
                {
                    updateSQL.Append(", ");
                }
                else
                {
                    updateSQL.Append(" where ");
                }

                updateSQL.Append(this.KeyNames[i]).Append("=").Append(this.varPrefix).Append(this.KeyNames[i]);
            }

            return updateSQL.ToString();
        }

        public String[] GetSqlParameterNames()
        {
            String[] result = new String[this.KeyNames.Count + this.ParameterNames.Count];

            this.ParameterNames.CopyTo(result, 0);
            this.KeyNames.CopyTo(result, this.ParameterNames.Count);

            return result;
        }

        public Object[] GetSqlParameterValues()
        {
            Object[] result = new Object[this.KeyValues.Count + this.ParameterValues.Count];

            this.ParameterValues.CopyTo(result, 0);
            this.KeyValues.CopyTo(result, this.ParameterValues.Count);

            return result;
        }
    }

    public class Dictionary2Where : Object2SQL
    {
        private StringBuilder output = null;

        public void parse(object entity)
        {

            this.SetDBType();

            this.varPrefix = DBUtil.GetDBParamFlag(this.dbType);

            IDictionary<string, object> entityDict = entity as IDictionary<string, object>;

            output = new StringBuilder();

            if (entityDict == null || entityDict.Count == 0)
            {
                return;
            }
            output.Append(" where 1=1");

            this._entity = entity;


            //获得实体的属性集合 
            ICollection<string> props = entityDict.Keys;

            this.ParameterNames = new List<String>(props.Count);
            this.ParameterValues = new List<Object>(props.Count);

            foreach (string prop in props)
            {
                output.Append(" and ");

                string[] keyWithOper = prop.Split('$');
                string colName = keyWithOper[0];
                object colValue = entityDict[prop];

                if (null == colValue)
                {
                    continue;
                }

                string operatorName = "=";
                ICollection<object> colValues = this.Conver2ObjectCollection(colValue);
                bool isArray = (colValues != null);// colValue is ICollection<object>;
                if (keyWithOper.Length == 1)
                {
                    operatorName = isArray ? "in" : "=";
                }
                else
                {
                    operatorName = keyWithOper[1];
                }


                if (isArray)
                {

                    if (colValues.Count != 0)
                    {
                        output.Append(colName).Append(" ").Append(operatorName).Append("(");

                        int i = 0;
                        foreach (object colValueI in colValues)
                        {
                            if (i != 0)
                            {
                                output.Append(", ");
                            }
                            string colNameI = colName + "_" + (i++);
                            output.Append(this.varPrefix).Append(colNameI);
                            this.ParameterNames.Add(colNameI);
                            this.ParameterValues.Add(colValueI);
                        }

                        output.Append(")");
                    }
                }
                else
                {
                    output.Append(colName).Append(operatorName);

                    output.Append(this.varPrefix).Append(colName);
                    this.ParameterNames.Add(colName);
                    this.ParameterValues.Add(colValue);
                }


            }
        }


        public string AsSql()
        {
            return output.ToString();
        }

        public String[] GetSqlParameterNames()
        {
            return ParameterNames.ToArray();
        }

        public Object[] GetSqlParameterValues()
        {
            return this.ParameterValues.ToArray();
        }
    }
}
