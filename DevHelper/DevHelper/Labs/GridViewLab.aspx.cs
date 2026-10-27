using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DevHelper.Labs
{
    public partial class GridViewLab : Page
    {
        private const string TableKey = "Lab_GridViewTable";

        private DataTable Store
        {
            get
            {
                if (Session[TableKey] == null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Id", typeof(int));
                    dt.Columns.Add("Name", typeof(string));
                    dt.Columns.Add("Category", typeof(string));
                    dt.Columns.Add("Price", typeof(decimal));

                    dt.Rows.Add(1, "Mechanical Keyboard", "Hardware", 85.00m);
                    dt.Rows.Add(2, "IDE Pro License", "Software", 199.99m);
                    dt.Rows.Add(3, "USB-C Hub", "Accessories", 35.50m);

                    Session[TableKey] = dt;
                }
                return (DataTable)Session[TableKey];
            }
            set
            {
                Session[TableKey] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            DataTable dt = Store;
            gvDemo.DataSource = dt;
            gvDemo.DataBind();
            lblRowCount.Text = $"{dt.Rows.Count} item{(dt.Rows.Count == 1 ? "" : "s")}";
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            string name = txtItemName.Text.Trim();
            string cat = ddlCategory.SelectedValue;
            decimal price;
            if (string.IsNullOrEmpty(name) || !decimal.TryParse(txtPrice.Text, out price))
            {
                ShowAlert("Please enter a valid item name and numeric price.", "alert-warning");
                return;
            }

            DataTable dt = Store;
            int nextId = dt.Rows.Count > 0 ? (int)dt.Compute("MAX(Id)", "") + 1 : 1;
            dt.Rows.Add(nextId, name, cat, price);
            Store = dt;

            txtItemName.Text = string.Empty;
            txtPrice.Text = string.Empty;

            ShowAlert($"Added item <strong>{Server.HtmlEncode(name)}</strong>.", "alert-success");
            BindGrid();
        }

        protected void gvDemo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvDemo.DataKeys[e.RowIndex].Value);
            DataTable dt = Store;

            DataRow[] rows = dt.Select($"Id = {id}");
            if (rows.Length > 0)
            {
                string name = rows[0]["Name"].ToString();
                dt.Rows.Remove(rows[0]);
                Store = dt;
                ShowAlert($"Removed item <strong>{Server.HtmlEncode(name)}</strong>.", "alert-info");
            }

            BindGrid();
        }

        private void ShowAlert(string msg, string cssClass)
        {
            pnlAlert.CssClass = $"alert {cssClass} alert-dismissible fade show";
            lblAlert.Text = msg;
            pnlAlert.Visible = true;
        }
    }
}
