using System;
using System.Configuration;

namespace LINQPractice
{
    public partial class QueryUserContextDataContext
    {
        public QueryUserContextDataContext() : 
            base(ConfigurationManager.ConnectionStrings["LinqPractice"] != null 
                    ? ConfigurationManager.ConnectionStrings["LinqPractice"].ConnectionString 
                    : ConfigurationManager.ConnectionStrings["AssignmentDBConnectionString"].ConnectionString, mappingSource)
        {
            OnCreated();
        }
    }

    public class QueryUserContext : QueryUserContextDataContext
    {
        public QueryUserContext() : base()
        {
        }

        public QueryUserContext(string connection) : base(connection)
        {
        }
    }
}
