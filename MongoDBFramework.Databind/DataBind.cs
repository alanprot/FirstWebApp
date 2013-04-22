using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Wrappers;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MongoDBFramework.Databind
{   
    [DataContract]
    public abstract class DataBind<T> where T : DataBind<T>
    {
        public static string ConnectionString = "mongodb://localhost";
        public static string DatabaseName = "test";
        public static int DEFAULT_PAGE_SIZE = 15;

        [BsonId]
        [DataMember(Name = "id")]
        public string Id { get; set; }

        private static MongoCollection<T> GetCollection()
        {
            MongoClient client = new MongoClient(ConnectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(DatabaseName);
            var collection = database.GetCollection<T>(typeof(T).ToString());
            return collection;
        }

        public static IEnumerable<T> FindAll()
        {
            var collection = GetCollection();
            return collection.FindAll();
        }

        public static T GetById(string id)
        {
            var collection = GetCollection();
            var query = Query<T>.EQ(e => e.Id, id);
            return collection.FindOne(query);
        }

        public static void RemoveAll()
        {
            var collection = GetCollection();
            collection.RemoveAll();
        }


        public void Save()
        {
            var collection = GetCollection();
            this.Id = ObjectId.GenerateNewId().ToString();
            collection.Insert(this);
        }

        public static IEnumerable<T> FindAll(Expression<Func<T, bool>> expression)
        {
            var collection = GetCollection();
            var query = Query<T>.Where(expression);
            return collection.Find(query);
        }

        public static T FindFirst(Expression<Func<T, bool>> expression)
        {
            return FindAll(expression).FirstOrDefault();
        }

        public static IEnumerable<T> FindAll(Func<T, bool> predicateWhere, Comparison<T> comp)
        {
            var collection = GetCollection();
            var result = from C in collection.AsQueryable<T>()
                         .Where(predicateWhere)
                         .OrderBy(x => x, new ComparisonComparer<T>(comp))
                         select C;

            return result;
        }

        public static IEnumerable<T> FindAll(Func<T, bool> predicateWhere, Func<T, T, bool> comp)
        {
            Comparison<T> converte = (a, b) => comp(a, b) ? 1 : 0;
            return FindAll(predicateWhere, converte);
        }

        public static IEnumerable<T> GetPage(Comparison<T> comp, int pageNumber, Func<T, bool> predicateWhere = null)
        {
            Func<T, bool> toWhereClause = b => predicateWhere == null ? true : predicateWhere(b);

            var collection = GetCollection();
            int startIndex = pageNumber * DEFAULT_PAGE_SIZE - DEFAULT_PAGE_SIZE;

            var result = from C in collection.AsQueryable<T>()
                         .Where(toWhereClause)
                         .OrderBy(x => x, new ComparisonComparer<T>(comp))
                         .Skip(startIndex)
                         .Take(DEFAULT_PAGE_SIZE)
                         select C;
            return result;
        }


        public static IEnumerable<T> GetPage(Func<T, T, bool> comp, int pageNumber, Func<T, bool> predicateWhere = null)
        {
            Comparison<T> converte = (a, b) => comp(a, b) ? 1 : 0;
            return GetPage(converte, pageNumber, predicateWhere);
        }

        public static int GetPageCount(Comparison<T> comp, Func<T, bool> predicateWhere = null)
        {
            Func<T, bool> toWhereClause = b => predicateWhere == null ? true : predicateWhere(b);

            var collection = GetCollection();

            var result = collection.AsQueryable<T>()
                .Where(toWhereClause)
                         .Count();

            result = result / DEFAULT_PAGE_SIZE + (result % DEFAULT_PAGE_SIZE > 0 ? 1 : 0);
            return result;
        }

        public static int GetPageCount(Func<T, T, bool> comp, Func<T, bool> predicateWhere = null)
        {
            Comparison<T> converte = (a, b) => comp(a, b) ? 1 : 0;
            return GetPageCount(converte, predicateWhere);
        }

    }
}
