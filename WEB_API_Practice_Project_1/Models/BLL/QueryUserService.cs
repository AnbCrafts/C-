using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WEB_API_Practice_Project_1.Models.BEL;
using WEB_API_Practice_Project_1.Models.DAL;

namespace WEB_API_Practice_Project_1.Models.BLL
{
    public class QueryUserService
    {
        private readonly DBFunction _df;

        public QueryUserService()
        {
            _df = new DBFunction();
        }

        public List<QueryUser> GetAllUsers()
        {
            DataTable dt = _df.DTWithProc("labTest.sp_GetAllQueryUsers", null);
            List<QueryUser> userList = new List<QueryUser>();

            if (dt != null && dt.Rows.Count > 0)
            {
                userList = dt.AsEnumerable().Select(row => new QueryUser
                {
                    UserId = row.Field<int>("UserId"),
                    UserName = row.Field<string>("UserName"),
                    Email = row.Field<string>("Email"),
                    FullName = row.Field<string>("FullName"),
                    MobileNo = row.Field<string>("MobileNo"),
                    City = row.Field<string>("City"),
                    StateName = row.Field<string>("StateName"),
                    Country = row.Field<string>("Country"),
                    Gender = row.Field<string>("Gender"),
                    QueryText = row.Field<string>("QueryText"),
                    CreatedDate = row.Field<DateTime?>("CreatedDate"),
                    IsActive = row.Field<bool>("IsActive"),
                    QueryAmount = row.Field<decimal?>("QueryAmount")
                }).ToList();
            }

            return userList;
        }

        public QueryUser GetUserById(int userId)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId)
            };

            DataTable dt = _df.DTWithProc("labTest.sp_GetQueryUserById", param);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.AsEnumerable().Select(row => new QueryUser
                {
                    UserId = row.Field<int>("UserId"),
                    UserName = row.Field<string>("UserName"),
                    Email = row.Field<string>("Email"),
                    FullName = row.Field<string>("FullName"),
                    MobileNo = row.Field<string>("MobileNo"),
                    City = row.Field<string>("City"),
                    StateName = row.Field<string>("StateName"),
                    Country = row.Field<string>("Country"),
                    Gender = row.Field<string>("Gender"),
                    QueryText = row.Field<string>("QueryText"),
                    CreatedDate = row.Field<DateTime?>("CreatedDate"),
                    IsActive = row.Field<bool>("IsActive"),
                    QueryAmount = row.Field<decimal?>("QueryAmount")
                }).FirstOrDefault();
            }

            return null;
        }

        public string InsertUser(QueryUser user)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@UserName", (object)user.UserName ?? DBNull.Value),
                new SqlParameter("@Email", (object)user.Email ?? DBNull.Value),
                new SqlParameter("@FullName", (object)user.FullName ?? DBNull.Value),
                new SqlParameter("@MobileNo", (object)user.MobileNo ?? DBNull.Value),
                new SqlParameter("@City", (object)user.City ?? DBNull.Value),
                new SqlParameter("@StateName", (object)user.StateName ?? DBNull.Value),
                new SqlParameter("@Country", (object)user.Country ?? DBNull.Value),
                new SqlParameter("@Gender", (object)user.Gender ?? DBNull.Value),
                new SqlParameter("@QueryText", (object)user.QueryText ?? DBNull.Value),
                new SqlParameter("@IsActive", user.IsActive),
                new SqlParameter("@QueryAmount", user.QueryAmount.HasValue ? (object)user.QueryAmount.Value : DBNull.Value)
            };

            object result = _df.executeScalarWithProc("labTest.sp_InsertQueryUser", param);
            return result != null ? result.ToString() : "Success";
        }

        public string UpdateUser(int userId, QueryUser user)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@UserName", (object)user.UserName ?? DBNull.Value),
                new SqlParameter("@Email", (object)user.Email ?? DBNull.Value),
                new SqlParameter("@FullName", (object)user.FullName ?? DBNull.Value),
                new SqlParameter("@MobileNo", (object)user.MobileNo ?? DBNull.Value),
                new SqlParameter("@City", (object)user.City ?? DBNull.Value),
                new SqlParameter("@StateName", (object)user.StateName ?? DBNull.Value),
                new SqlParameter("@Country", (object)user.Country ?? DBNull.Value),
                new SqlParameter("@Gender", (object)user.Gender ?? DBNull.Value),
                new SqlParameter("@QueryText", (object)user.QueryText ?? DBNull.Value),
                new SqlParameter("@IsActive", user.IsActive),
                new SqlParameter("@QueryAmount", user.QueryAmount.HasValue ? (object)user.QueryAmount.Value : DBNull.Value)
            };

            object result = _df.executeScalarWithProc("labTest.sp_UpdateQueryUser", param);
            return result != null ? result.ToString() : "Success";
        }

        public string DeleteUser(int userId)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId)
            };

            object result = _df.executeScalarWithProc("labTest.sp_DeleteQueryUser", param);
            return result != null ? result.ToString() : "Success";
        }
    }
}
