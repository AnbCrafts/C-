using System;
using System.Data;
using System.Web.UI;

namespace GridView_ObjectDataSource_Practice
{
    public partial class DynamicPagination : System.Web.UI.Page
    {
        private const int PageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageIndex"] = 0;

                BindGrid();
            }
        }

        private void BindGrid()
        {
            int pageIndex =
                Convert.ToInt32(ViewState["PageIndex"]);

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

            ViewState["TotalPages"] = totalPages;

            lblPageInfo.Text =
                "Page " +
                (pageIndex + 1) +
                " of " +
                totalPages +
                " | Total Records : " +
                totalRows;

            btnPrevious.Enabled = pageIndex > 0;

            btnNext.Enabled =
                pageIndex < totalPages - 1;
        }

        protected void btnPrevious_Click(
            object sender,
            EventArgs e)
        {
            int pageIndex =
                Convert.ToInt32(ViewState["PageIndex"]);

            if (pageIndex > 0)
            {
                ViewState["PageIndex"] =
                    pageIndex - 1;
            }

            BindGrid();
        }

        protected void btnNext_Click(
            object sender,
            EventArgs e)
        {
            int pageIndex =
                Convert.ToInt32(ViewState["PageIndex"]);

            int totalPages =
                Convert.ToInt32(ViewState["TotalPages"]);

            if (pageIndex < totalPages - 1)
            {
                ViewState["PageIndex"] =
                    pageIndex + 1;
            }

            BindGrid();
        }

        protected void btnGo_Click(
            object sender,
            EventArgs e)
        {
            int requestedPage;

            if (!int.TryParse(
                txtPageNo.Text.Trim(),
                out requestedPage))
            {
                return;
            }

            int totalPages =
                Convert.ToInt32(ViewState["TotalPages"]);

            if (requestedPage < 1)
            {
                requestedPage = 1;
            }

            if (requestedPage > totalPages)
            {
                requestedPage = totalPages;
            }

            ViewState["PageIndex"] =
                requestedPage - 1;

            BindGrid();
        }
    }
}
