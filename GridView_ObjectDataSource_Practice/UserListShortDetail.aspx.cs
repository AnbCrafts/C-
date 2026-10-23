using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GridView_ObjectDataSource_Practice
{
    public partial class UserListShortDetail : System.Web.UI.Page
    {
       
        
            private const int PageSize = 10;

            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    BindGrid();
                }
            }

            private void BindGrid()
            {
                int pageIndex = 0;

                if (Request.QueryString["pageIndex"] != null)
                {
                    int.TryParse(
                        Request.QueryString["pageIndex"],
                        out pageIndex);
                }

                int totalRows;

                DataTable dt =
                    QueryUserDALStatic.GetAllUsersShortDetailList(
                        pageIndex,
                        PageSize,
                        out totalRows);

                gvUsers.DataSource = dt;
                gvUsers.DataBind();

                int totalPages =
                    (int)Math.Ceiling(
                        (double)totalRows / PageSize);

                lblPageInfo.Text =
                    "Page " +
                    (pageIndex + 1) +
                    " of " +
                    totalPages;

                if (pageIndex > 0)
                {
                    lnkPrevious.NavigateUrl =
                        "UserListShortDetail.aspx?pageIndex="
                        + (pageIndex - 1);
                }
                else
                {
                    lnkPrevious.Visible = false;
                }

                if (pageIndex < totalPages - 1)
                {
                    lnkNext.NavigateUrl =
                        "UserListShortDetail.aspx?pageIndex="
                        + (pageIndex + 1);
                }
                else
                {
                    lnkNext.Visible = false;
                }
            }

        protected void gvUsers_RowDataBound(
    object sender,
    GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string userId =
                    DataBinder.Eval(
                        e.Row.DataItem,
                        "UserId").ToString();

                e.Row.Attributes["style"] =
                    "cursor:pointer;";

                e.Row.Attributes["onclick"] =
                    "window.location='UserListDetailedDescription.aspx?UserId="
                    + userId + "'";
            }
        }


    }
}