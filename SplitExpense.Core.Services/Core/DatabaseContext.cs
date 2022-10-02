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

        public int Delete<T>(T data)
        {
            return this.db.Delete<T>(data);
        }

        public IEnumerable<T> Fetch<T>(string query, params object[] args)
        {
            return this.db.Fetch<T>(query, args);
        }

        public bool Exists<T>(string query, params object[] args)
        {
            return this.db.Exists<T>(query, args);
        }
    }
}
