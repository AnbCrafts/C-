using System;
using System.Data;

namespace GridView_ObjectDataSource_Practice
{
    public partial class DynamicPagination3 : System.Web.UI.Page
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
                QueryUserDALStatic.GetAllUsers(
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
                    "DynamicPagination3.aspx?pageIndex="
                    + (pageIndex - 1);
            }
            else
            {
                lnkPrevious.Visible = false;
            }

            if (pageIndex < totalPages - 1)
            {
                lnkNext.NavigateUrl =
                    "DynamicPagination3.aspx?pageIndex="
                    + (pageIndex + 1);
            }
            else
            {
                lnkNext.Visible = false;
            }
        }
    }
}