using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LINQPractice
{
    public partial class LinqCrudPractice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUserData();
            }
        }

        private void BindUserData()
        {
            using (QueryUserContext dataContext = new QueryUserContext())
            {
                var users = dataContext.udp_GetAllUsers();
                           

                gvUsers.DataSource = users.ToList();
                gvUsers.DataBind();
            }
        }
                          
        // 1. Enter Edit Mode
        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            BindUserData();
        }

        // 2. Cancel Edit Mode
        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            BindUserData();
        }

        // 3. Set DropDown selection when entering edit mode
        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlGender = (DropDownList)e.Row.FindControl("ddlEditGender");
                if (ddlGender != null)
                {
                    string currentGender = DataBinder.Eval(e.Row.DataItem, "Gender")?.ToString();
                    ListItem item = ddlGender.Items.FindByValue(currentGender);
                    if (item != null)
                    {
                        ddlGender.SelectedValue = currentGender;
                    }
                }
            }
        }

        // 4. Update User (dataContext.udp_UpdateUser)
        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvUsers.Rows[e.RowIndex];

            // Extract values from controls
            string userName = ((TextBox)row.FindControl("txtEditUserName"))?.Text.Trim();
            string email = ((TextBox)row.FindControl("txtEditEmail"))?.Text.Trim();
            string fullName = ((TextBox)row.FindControl("txtEditFullName"))?.Text.Trim();
            string mobileNo = ((TextBox)row.FindControl("txtEditMobileNo"))?.Text.Trim();
            string city = ((TextBox)row.FindControl("txtEditCity"))?.Text.Trim();
            string stateName = ((TextBox)row.FindControl("txtEditStateName"))?.Text.Trim();
            string country = ((TextBox)row.FindControl("txtEditCountry"))?.Text.Trim();
            string gender = ((DropDownList)row.FindControl("ddlEditGender"))?.SelectedValue;
            string queryText = ((TextBox)row.FindControl("txtEditQueryText"))?.Text.Trim();
            bool isActive = ((CheckBox)row.FindControl("chkEditIsActive")).Checked;

            // Safe Decimal Parse
            decimal? queryAmount = null;
            string amountText = ((TextBox)row.FindControl("txtEditQueryAmount"))?.Text.Trim();
            if (decimal.TryParse(amountText, out decimal parsedAmount))
            {
                queryAmount = parsedAmount;
            }

            using (QueryUserContext dataContext = new QueryUserContext())
            {
                // Call your Update Stored Procedure
                dataContext.udp_UpdateUser(
                    userId,
                    userName,
                    email,
                    fullName,
                    mobileNo,
                    city,
                    stateName,
                    country,
                    gender,
                    queryText,
                    isActive,
                    queryAmount
                );
            }

            gvUsers.EditIndex = -1;
            BindUserData();
        }

        // 5. Insert New User from Footer (dataContext.udp_InsertUser)
        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                GridViewRow footer = gvUsers.FooterRow;

                string userName = ((TextBox)footer.FindControl("txtNewUserName"))?.Text.Trim();
                string email = ((TextBox)footer.FindControl("txtNewEmail"))?.Text.Trim();
                string fullName = ((TextBox)footer.FindControl("txtNewFullName"))?.Text.Trim();
                string mobileNo = ((TextBox)footer.FindControl("txtNewMobileNo"))?.Text.Trim();
                string city = ((TextBox)footer.FindControl("txtNewCity"))?.Text.Trim();
                string stateName = ((TextBox)footer.FindControl("txtNewStateName"))?.Text.Trim();
                string country = ((TextBox)footer.FindControl("txtNewCountry"))?.Text.Trim();
                string gender = ((DropDownList)footer.FindControl("ddlNewGender"))?.SelectedValue;
                string queryText = ((TextBox)footer.FindControl("txtNewQueryText"))?.Text.Trim();
                bool isActive = ((CheckBox)footer.FindControl("chkNewIsActive")).Checked;

                decimal? queryAmount = null;
                string amountText = ((TextBox)footer.FindControl("txtNewQueryAmount"))?.Text.Trim();
                if (decimal.TryParse(amountText, out decimal parsedAmount))
                {
                    queryAmount = parsedAmount;
                }

                using (QueryUserContext dataContext = new QueryUserContext())
                {
                    // Call your Insert Stored Procedure (adjust procedure name if needed)
                    dataContext.udp_CreateUser(
                        userName,
                        email,
                        fullName,
                        mobileNo,
                        city,
                        stateName,
                        country,
                        gender,
                        queryText,
                        queryAmount
                    );
                }

                BindUserData();
            }
        }

        // 6. Delete User (dataContext.udp_DeleteUser)
        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

            using (QueryUserContext dataContext = new QueryUserContext())
            {
                // Call your Delete Stored Procedure (adjust procedure name if needed)
                dataContext.udp_DeleteUserById(userId);
            }

            BindUserData();
        }

        // 7. Paging
        protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            BindUserData();
        }
    }
}