using PetaPoco;
using PetaPoco.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Services.Core
{
    public class DatabaseContext
    {
        private readonly IDatabase db;

        public DatabaseContext(string connectionString)
        {
            this.db = DatabaseConfiguration.Build()
                                            .UsingConnectionString(connectionString)
                                            .UsingProvider<SqlServerDatabaseProvider>()
                                            .UsingDefaultMapper<ConventionMapper>(m =>
                                            {
                                                m.InflectTableName = (inflector, s) => inflector.Pluralise(inflector.Pascalise(s));
                                            })
                                            .Create();
        }

        public int Insert<T>(T data)
        {
            return (int)this.db.Insert(data);
        }

        public int Update<T>(T data, IEnumerable<string> columns)
        {
            return this.db.Update(data, columns);
        }

        public int Update<T>(string query, params object[] whereArgs)
        {
            var sql = new Sql(query, whereArgs);
            return this.db.Update<T>(sql);
        }

        public int Delete<T>(T data)
        {
            return this.db.Delete<T>(data);
        }

        public IEnumerable<T> Fetch<T>(string query, params object[] args)
        {
            return this.db.Fetch<T>(query, args);
        }

        public T SingleOrDefault<T>(string query, params object[] args)
        {
            return this.db.SingleOrDefault<T>(query, args);
        }

        public T FirstOrDefault<T>(string query, params object[] args)
        {
            return this.db.FirstOrDefault<T>(query, args);
        }

        public bool Exists<T>(string query, params object[] args)
        {
            return this.db.Exists<T>(query, args);
        }

        public void BulkInsert<T>(List<T> data)
        {
            try
            {
                this.db.BeginTransaction();
                foreach (var record in data)
                {
                    this.db.Insert(record);
                }
                this.db.CompleteTransaction();
            }catch(Exception e)
            {
                this.db.AbortTransaction();
                throw;
            }
        }

        public void BeginTransaction()
        {
            this.db.BeginTransaction();
        }

        public void CompleteTransaction()
        {
            this.db.CompleteTransaction();
        }

        public void AbortTransaction()
        {
            this.db.AbortTransaction();
        }
    }
}
