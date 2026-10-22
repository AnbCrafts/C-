using System;
using System.Web.UI.WebControls;

namespace GridView_ObjectDataSource_Practice
{
    public partial class User_CRUD_Non_ObjectDataSource : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            gvUsers.DataSource = QueryUserDALStatic.GetAllUsers();
            gvUsers.DataBind();
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                gvUsers.EditIndex =
                    Convert.ToInt32(e.CommandArgument);

                BindGrid();
            }

            else if (e.CommandName == "CancelUpdate")
            {
                gvUsers.EditIndex = -1;

                BindGrid();
            }

            else if (e.CommandName == "DeleteRow")
            {
                int userId =
                    Convert.ToInt32(e.CommandArgument);

                QueryUserDALStatic.DeleteUserById(userId);

                BindGrid();
            }

            else if (e.CommandName == "UpdateRow")
            {
                GridViewRow row =
                    ((LinkButton)e.CommandSource).NamingContainer
                    as GridViewRow;

                int userId =
                    Convert.ToInt32(e.CommandArgument);

                string userName =
                    ((TextBox)row.FindControl("txtUserName")).Text;

                string email =
                    ((TextBox)row.FindControl("txtEmail")).Text;

                string fullName =
                    ((TextBox)row.FindControl("txtFullName")).Text;

                string mobileNo =
                    ((TextBox)row.FindControl("txtMobileNo")).Text;

                string city =
                    ((TextBox)row.FindControl("txtCity")).Text;

                string stateName =
                    ((TextBox)row.FindControl("txtStateName")).Text;

                string country =
                    ((TextBox)row.FindControl("txtCountry")).Text;

                string gender =
                    ((DropDownList)row.FindControl("ddlGender")).SelectedValue;

                string queryText =
                    ((TextBox)row.FindControl("txtQueryText")).Text;

                decimal queryAmount = 0;

                decimal.TryParse(
                    ((TextBox)row.FindControl("txtQueryAmount")).Text,
                    out queryAmount);

                bool isActive =
                    ((CheckBox)row.FindControl("chkEditActive")).Checked;

                QueryUserDALStatic.UpdateUser(
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
                    queryAmount,
                    isActive);

                gvUsers.EditIndex = -1;

                BindGrid();
            }

            else if (e.CommandName == "InsertRow")
            {
                string userName =
                    ((TextBox)gvUsers.FooterRow.FindControl("txtInsertUserName")).Text;

                string email =
                    ((TextBox)gvUsers.FooterRow.FindControl("txtInsertEmail")).Text;

                string fullName =
                    ((TextBox)gvUsers.FooterRow.FindControl("txtInsertFullName")).Text;

                string mobileNo =
                    ((TextBox)gvUsers.FooterRow.FindControl("txtInsertMobileNo")).Text;

                string city =
                    ((TextBox)gvUsers.FooterRow.FindControl("txtInsertCity")).Text;

                string stateName =
                    ((TextBox)gvUsers.FooterRow.FindControl("txtInsertStateName")).Text;

                string country =
                    ((TextBox)gvUsers.FooterRow.FindControl("txtInsertCountry")).Text;

                string gender =
                    ((DropDownList)gvUsers.FooterRow.FindControl("ddlInsertGender")).SelectedValue;

                string queryText =
                    ((TextBox)gvUsers.FooterRow.FindControl("txtInsertQueryText")).Text;

                decimal queryAmount = 0;

                decimal.TryParse(
                    ((TextBox)gvUsers.FooterRow
                    .FindControl("txtInsertQueryAmount")).Text,
                    out queryAmount);

                QueryUserDALStatic.InsertUser(
                    userName,
                    email,
                    fullName,
                    mobileNo,
                    city,
                    stateName,
                    country,
                    gender,
                    queryText,
                    queryAmount);

                BindGrid();
            }
        }


        protected void gvUsers_PageIndexChanging(
    object sender,
    GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;

            BindGrid();
        }



    }
}