//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Configuration;
//using System.IO;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using MongoDB.Shared;
//using MongoDB.Bson.Serialization.Attributes;
//using MongoDB.Bson.Serialization.Serializers;
//using MongoDB.Bson.Serialization;
//using System.Collections.Concurrent;
//using System.Linq.Expressions;


//namespace TG.Example.Mongodb
//{
//    /// <summary>
//    /// MongoDb数据访问基类
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    /// <typeparam name="TKey"></typeparam>
//    public class MongoRepository<T, TKey> : BaseMongoDB<T> where T : class
//    {
//        private readonly Type _keyType;

//        protected MongoRepository(string connectionString = "", string collectionName = "")
//            : base(connectionString, GetCollectionName(collectionName))
//        {
//            _keyType = typeof(TKey);
//            if (_keyType != typeof(string) && _keyType != typeof(long) && _keyType != typeof(ObjectId))
//            {
//                throw new Exception("TKey must these type: string or long or ObjectId");
//            }

//            Initial();
//        }

//        /// <summary>
//        /// 初始化
//        /// 注册序列化器
//        /// </summary>
//        private void Initial()
//        {
//            try
//            {
//                var serializer = BsonSerializer.LookupSerializer(typeof(DateTime));
//                if (serializer == null || serializer.GetType() != typeof(LocalTimeSerializer))
//                {
//                    // remove exist
//                    var cacheFieldInfo = typeof(BsonSerializerRegistry).
//                        GetField("_cache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//                    var _cacheTemp = cacheFieldInfo.GetValue(BsonSerializer.SerializerRegistry);
//                    var _cache = _cacheTemp as ConcurrentDictionary<Type, IBsonSerializer>;
//                    IBsonSerializer removeed;
//                    _cache.TryRemove(typeof(DateTime), out removeed);

//                    // add my owner
//                    BsonSerializer.RegisterSerializer(typeof(DateTime), new LocalTimeSerializer());
//                }
//            }
//            catch
//            {
//            }
//        }

//        /// <summary>
//        /// 依据连接字符串创建数据库访问实例
//        /// </summary>
//        /// <param name="db"></param>
//        /// <returns></returns>
//        public static MongoRepository<T, TKey> CreateRepository(MongoDBs db = MongoDBs.MongoTicketDB)
//        {
//            var connStrName = Enum.GetName(typeof(MongoDBs), db);
//            var connStr = ConfigurationManager.ConnectionStrings[connStrName].ConnectionString;
//            return new MongoRepository<T, TKey>(connStr);
//        }

//        /// <summary>
//        /// 获取集合名
//        /// </summary>
//        /// <param name="specifiedCollectionName"></param>
//        /// <returns></returns>
//        private static string GetCollectionName(string specifiedCollectionName)
//        {
//            string collectionName;

//            if (!specifiedCollectionName.IsNullOrEmpty())
//            {
//                collectionName = specifiedCollectionName;
//            }
//            else
//            {
//                var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionNameAttribute));
//                collectionName = att != null ? ((CollectionNameAttribute)att).Name : typeof(T).Name;

//                if (string.IsNullOrEmpty(collectionName))
//                {
//                    throw new ArgumentException("Collection name cannot be empty for this entity");
//                }
//            }

//            return collectionName;
//        }

//        /// <summary>
//        /// 判断集合是否存在
//        /// </summary>
//        /// <param name="predicate"></param>
//        /// <returns></returns>
//        public bool Exists(Expression<Func<T, bool>> predicate)
//        {
//            var count = this.Collection.Find(predicate).CountAsync().Result;
//            return count > 0;
//        }

//        /// <summary>
//        /// 依据id获取实体(文档)
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public T GetById(TKey id)
//        {
//            var filter = Builders<T>.Filter.Eq("_id", id);
//            var result = this.Collection.Find(filter).FirstOrDefaultAsync().Result;
//            return result;
//        }

//        /// <summary>
//        /// 查询所有实体
//        /// </summary>
//        /// <returns></returns>
//        public IEnumerable<T> GetAll()
//        {
//            return this.Collection.Find(new BsonDocument()).ToListAsync().Result;
//        }

//        /// <summary>
//        /// 依据查询表达式来查询实体
//        /// </summary>
//        /// <param name="predicate"></param>
//        /// <returns></returns>
//        public IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate)
//        {
//            return this.Collection.Find(predicate).ToListAsync().Result;
//        }

//        /// <summary>
//        /// 分页查询
//        /// </summary>
//        /// <param name="predicate"></param>
//        /// <param name="pageIndex"></param>
//        /// <param name="pageSize"></param>
//        /// <param name="orderBy"></param>
//        /// <returns></returns>
//        public IEnumerable<T> GetByPaging(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, IList<OrderBy<T>> orderBy = null)
//        {
//            if (pageIndex < 1)
//            {
//                pageIndex = 1;
//            }

//            if (pageSize < 1)
//            {
//                pageSize = 15;
//            }

//            var data = this.Collection.Find(predicate);

//            if (orderBy != null && orderBy.Count > 0)
//            {
//                var idx = 0;
//                foreach (var sort in orderBy)
//                {
//                    idx++;
//                    if (idx == 1)
//                    {
//                        data = sort.IsAscending ? data.SortBy(sort.Field) : data.SortByDescending(sort.Field);
//                    }
//                    else
//                    {
//                        var sortByData = data as IOrderedFindFluent<T, T>;
//                        if (sortByData == null)
//                        {
//                            continue;
//                        }

//                        data = sort.IsAscending ? sortByData.ThenBy(sort.Field) : sortByData.ThenByDescending(sort.Field);
//                    }
//                }
//            }

//            var result = data.Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync().Result;

//            return result;
//        }

//        /// <summary>
//        /// 新增(单个)
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        public void Insert(T entity)
//        {
//            AsyncHelper.RunSync(() => this.Collection.InsertOneAsync(entity));
//        }

//        /// <summary>
//        /// 新增(批量)
//        /// </summary>
//        /// <param name="entities"></param>
//        /// <returns></returns>
//        public void Insert(IEnumerable<T> entities)
//        {
//            AsyncHelper.RunSync(() => this.Collection.InsertManyAsync(entities));
//        }

//        /// <summary>
//        /// 新增，从json字符串
//        /// </summary>
//        /// <param name="json"></param>
//        /// <returns></returns>
//        public IList<BsonDocument> Insert(string json)
//        {
//            if (string.IsNullOrWhiteSpace(json))
//            {
//                return new List<BsonDocument>();
//            }

//            if (typeof(T) != typeof(BsonDocument))
//            {
//                throw new Exception("this method just for MogoRepository<BsonDocument> ");
//            }

//            var documents = new List<BsonDocument>();

//            if (json.TrimStart().StartsWith("["))
//            {
//                var bsonDocument = BsonSerializer.Deserialize<BsonArray>(json);
//                foreach (var b in bsonDocument)
//                {
//                    var bd = b.ToBsonDocument();
//                    bd.Remove("$id");
//                    documents.Add(bd);
//                }
//            }
//            else
//            {
//                var bsonDocument = BsonSerializer.Deserialize<BsonDocument>(json);
//                bsonDocument.Remove("$id");
//                documents.Add(bsonDocument);
//            }
//            AsyncHelper.RunSync(() => this.DB.GetCollection<BsonDocument>(this.CollectionName).InsertManyAsync(documents));

//            return documents;
//        }

//        /// <summary>
//        /// 更新(单个)
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns>返回影响的行数</returns>
//        public long Update(T entity)
//        {
//            var filter = Builders<T>.Filter.Eq("_id", GetIdValue(entity));
//            var replaceResult = this.Collection.ReplaceOneAsync(filter, entity).Result;
//            return replaceResult.ModifiedCount;
//        }

//        /// <summary>
//        /// 更新(批量)
//        /// </summary>
//        /// <param name="entities"></param>
//        /// <returns></returns>
//        public long Update(IEnumerable<T> entities)
//        {
//            long modifiedCount = 0;
//            foreach (var entity in entities)
//            {
//                var filter = Builders<T>.Filter.Eq("_id", GetIdValue(entity));
//                var replaceResult = this.Collection.ReplaceOneAsync(filter, entity).Result;
//                modifiedCount += replaceResult.ModifiedCount;
//            }

//            return modifiedCount;
//        }

//        /// <summary>
//        /// 更新，如果不存在则新增
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        public bool UpdateOrAdd(T entity)
//        {
//            SaveBigDataToLog(entity);

//            if (Update(entity) < 1)
//            {
//                this.Collection.InsertOneAsync(entity);
//            }

//            return true;
//        }

//        /// <summary>
//        /// 更新(局部更新)
//        /// </summary>
//        /// <param name="query"></param>
//        /// <param name="columnValues">指定字段</param>
//        /// <returns></returns>
//        public long Update(Expression<Func<T, bool>> query, Dictionary<string, object> columnValues)
//        {
//            if (columnValues == null || columnValues.Count == 0)
//            {
//                throw new ArgumentException("Update Columns is Null!", nameof(columnValues));
//            }

//            var fileter = Builders<T>.Filter.Where(query);
//            var update = Builders<T>.Update;
//            UpdateDefinition<T> updateDefinination = null;
//            columnValues.Keys.ToList().ForEach(x =>
//            {
//                updateDefinination = updateDefinination == null ? update.Set(x, columnValues[x]) : updateDefinination.Set(x, columnValues[x]);
//            });
//            var result = this.Collection.UpdateManyAsync<T>(query, updateDefinination).Result;

//            return result.ModifiedCount;
//        }

//        /// <summary>
//        /// 依据id删除数据(文档)
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public long Delete(TKey id)
//        {
//            var filter = Builders<T>.Filter.Eq("_id", id);
//            var result = this.Collection.DeleteOneAsync(filter).Result;
//            return result.DeletedCount;
//        }

//        /// <summary>
//        /// 删除，依据实体
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        public long Delete(T entity)
//        {
//            var filter = Builders<T>.Filter.Eq("_id", GetIdValue(entity));
//            var result = this.Collection.DeleteOneAsync(filter).Result;
//            return result.DeletedCount;
//        }

//        /// <summary>
//        /// 删除，依据查询表达式
//        /// </summary>
//        /// <param name="predicate"></param>
//        /// <returns></returns>
//        public long Delete(Expression<Func<T, bool>> predicate)
//        {
//            var result = this.Collection.DeleteManyAsync<T>(predicate).Result;
//            return result.DeletedCount;
//        }

//        /// <summary>
//        /// 删除所有
//        /// </summary>
//        /// <returns></returns>
//        public void RemoveAll()
//        {
//            AsyncHelper.RunSync(() => this.Collection.DeleteManyAsync(new BsonDocument()));
//        }

//        #region Private method

//        /// <summary>
//        /// 获取实体的id
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        private object GetIdValue(T entity)
//        {
//            object result = null;

//            if (typeof(T).IsSubclassOf(typeof(BaseMongoEntity<TKey>)))
//            {
//                var baseEntity = entity as BaseMongoEntity<TKey>;
//                result = baseEntity.Id;
//            }
//            else if (typeof(T) == typeof(BsonDocument))
//            {
//                var bson = entity as BsonDocument;
//                var objectId = bson["_id"];
//                if (objectId != null && objectId.IsObjectId)
//                {
//                    result = objectId.AsObjectId;
//                }
//            }

//            if (result == null)
//            {
//                throw new Exception("can not get Id (or ObjectId) from the entity, please check!");
//            }

//            return result;
//        }

//        /// <summary>
//        /// 记录日志
//        /// </summary>
//        /// <param name="entity"></param>
//        private void SaveBigDataToLog(T entity)
//        {
//            try
//            {
//                var dataJson = entity.ToJson();

//                var saveDirector = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VLogs", "PackageFH");
//                saveDirector = Path.Combine(saveDirector, "BigDataLog");
//                saveDirector = Path.Combine(saveDirector, DateTime.Now.ToString("yyyyMMdd"));
//                if (!Directory.Exists(saveDirector))
//                {
//                    Directory.CreateDirectory(saveDirector);
//                }
//                var fileName = Path.Combine(saveDirector, @"{typeof(T).Name}_{Guid.NewGuid()}");
//                File.WriteAllText(fileName, dataJson);
//            }
//            catch
//            {
//            }
//        }
//        #endregion
//    }

//    /// <summary>
//    /// MongoDb基类
//    /// 主要封装了创建数据库连接，获取数据库和获取集合的操作
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public class BaseMongoDB<T> where T : class
//    {
//        public BaseMongoDB(string connectionString)
//            : this(connectionString, null)
//        {
//        }

//        public BaseMongoDB(string connectionString, string collectionName)
//        {
//            if (string.IsNullOrEmpty(connectionString))
//            {
//                connectionString = ConfigurationManager.ConnectionStrings["MongoTicketDB"].ConnectionString;
//            }

//            if (string.IsNullOrEmpty(connectionString))
//            {
//                throw new Exception("mongodb connectionString can not be empty!");
//            }

//            var mongoUrl = new MongoUrl(connectionString);
//            this.DB = GetDatabaseFromUrl(mongoUrl);
//            if (!string.IsNullOrEmpty(collectionName))
//            {
//                this.CollectionName = collectionName;
//                this.Collection = DB.GetCollection<T>(collectionName);
//            }
//        }

//        /// <summary>
//        /// 依据连接字符串获取数据库
//        /// </summary>
//        /// <param name="url"></param>
//        /// <returns></returns>
//        private IMongoDatabase GetDatabaseFromUrl(MongoUrl url)
//        {
//            var client = new MongoClient(url);
//            var db = client.GetDatabase(url.DatabaseName);
//            return db;
//        }

//        /// <summary>
//        /// 集合
//        /// </summary>
//        public IMongoCollection<T> Collection { get; private set; }

//        /// <summary>
//        /// 数据库
//        /// </summary>
//        public IMongoDatabase DB { get; private set; }

//        /// <summary>
//        /// 集合名称，默认为空
//        /// </summary>
//        public string CollectionName { get; private set; }
//    }
//}
