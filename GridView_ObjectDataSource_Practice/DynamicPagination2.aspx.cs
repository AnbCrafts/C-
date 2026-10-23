using System;
using System.Data;
using System.Web.UI.WebControls;

namespace GridView_ObjectDataSource_Practice
{
    public partial class DynamicPagination2 : System.Web.UI.Page
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

            BindRepeater(totalRows);
        }

        private void BindRepeater(int totalRows)
        {
            int totalPages =
                (int)Math.Ceiling(
                    (double)totalRows / PageSize);

            DataTable dtPages = new DataTable();

            dtPages.Columns.Add("Text");
            dtPages.Columns.Add("Value");

            for (int i = 0; i < totalPages; i++)
            {
                DataRow dr = dtPages.NewRow();

                dr["Text"] = (i + 1).ToString();

                dr["Value"] = i;

                dtPages.Rows.Add(dr);
            }

            rptPaging.DataSource = dtPages;
            rptPaging.DataBind();
        }

        protected void Page_Changed(
            object sender,
            EventArgs e)
        {
            LinkButton btn =
                (LinkButton)sender;

            ViewState["PageIndex"] =
                Convert.ToInt32(
                    btn.CommandArgument);

            BindGrid();
        }
    }
}