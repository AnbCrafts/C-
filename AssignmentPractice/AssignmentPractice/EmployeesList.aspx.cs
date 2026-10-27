using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmployeeManagement
{
    public partial class EmployeesList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDesignations();
                BindEmployeeGrid();
            }
        }

        private void LoadDesignations()
        {
            try
            {
                DataTable dt = DbHelper.ExecuteDataTable("sp_GetDesignations");

                string currentSelected = ddlDesignation.SelectedValue;

                ddlDesignation.DataSource = dt;
                ddlDesignation.DataTextField = "Designation";
                ddlDesignation.DataValueField = "Designation";
                ddlDesignation.DataBind();

                ddlDesignation.Items.Insert(0, new ListItem("Select", ""));

                if (!string.IsNullOrEmpty(currentSelected) && ddlDesignation.Items.FindByValue(currentSelected) != null)
                {
                    ddlDesignation.SelectedValue = currentSelected;
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading designations: " + ex.Message);
            }
        }

        private void BindEmployeeGrid()
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@SearchTerm", string.IsNullOrWhiteSpace(txtSearch.Text) ? (object)DBNull.Value : txtSearch.Text.Trim()),
                    new SqlParameter("@Designation", string.IsNullOrWhiteSpace(ddlDesignation.SelectedValue) ? (object)DBNull.Value : ddlDesignation.SelectedValue),
                    new SqlParameter("@SortBy", ddlSortedBy.SelectedValue)
                };

                DataTable dt = DbHelper.ExecuteDataTable("sp_SearchEmployees", prms);

                gvEmployees.DataSource = dt;
                gvEmployees.DataBind();

                upEmployeeGrid.Update();
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading employee data: " + ex.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEmployeeGrid();
        }

        protected void FilterChanged(object sender, EventArgs e)
        {
            BindEmployeeGrid();
        }

        protected void gvEmployees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string empId = e.CommandArgument != null ? e.CommandArgument.ToString() : string.Empty;

            if (string.IsNullOrEmpty(empId)) return;

            if (e.CommandName == "DeleteEmp")
            {
                try
                {
                    SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@EmpId", empId) };
                    DbHelper.ExecuteNonQuery("sp_DeleteEmployee", prms);

                    BindEmployeeGrid();
                    LoadDesignations();

                    ShowAlert("Employee deleted successfully!");
                }
                catch (Exception ex)
                {
                    ShowAlert("Error deleting employee: " + ex.Message);
                }
            }
            else if (e.CommandName == "EditEmp")
            {
                try
                {
                    SqlParameter[] prms = new SqlParameter[]
                    {
                        new SqlParameter("@SearchTerm", empId),
                        new SqlParameter("@Designation", DBNull.Value),
                        new SqlParameter("@SortBy", "EmpId")
                    };

                    DataTable dt = DbHelper.ExecuteDataTable("sp_SearchEmployees", prms);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];

                        txtAddEmpId.Text = row["EmpId"].ToString();
                        txtAddEmpId.ReadOnly = true;
                        txtAddName.Text = row["Name"].ToString();
                        txtAddDesignation.Text = row["Designation"].ToString();

                        DateTime dob, doj;
                        if (DateTime.TryParseExact(row["DOB"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
                        {
                            txtAddDOB.Text = dob.ToString("yyyy-MM-dd");
                        }
                        else if (DateTime.TryParse(row["DOB"].ToString(), out dob))
                        {
                            txtAddDOB.Text = dob.ToString("yyyy-MM-dd");
                        }

                        if (DateTime.TryParseExact(row["DOJ"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out doj))
                        {
                            txtAddDOJ.Text = doj.ToString("yyyy-MM-dd");
                        }
                        else if (DateTime.TryParse(row["DOJ"].ToString(), out doj))
                        {
                            txtAddDOJ.Text = doj.ToString("yyyy-MM-dd");
                        }

                        txtAddSalary.Text = Convert.ToDecimal(row["Salary"]).ToString("F2");
                        txtAddAddress.Text = row["Address"].ToString();

                        ScriptManager.RegisterStartupScript(this, GetType(), "openEditModalScript", "openEditModal();", true);
                    }
                }
                catch (Exception ex)
                {
                    ShowAlert("Error loading employee details: " + ex.Message);
                }
            }
        }

        protected void btnSaveEmployee_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddEmpId.Text) ||
                string.IsNullOrWhiteSpace(txtAddName.Text) ||
                string.IsNullOrWhiteSpace(txtAddDesignation.Text) ||
                string.IsNullOrWhiteSpace(txtAddSalary.Text) ||
                string.IsNullOrWhiteSpace(txtAddDOB.Text) ||
                string.IsNullOrWhiteSpace(txtAddDOJ.Text))
            {
                ShowAlert("Please fill in all required (*) fields.");
                return;
            }

            decimal salary;
            if (!decimal.TryParse(txtAddSalary.Text.Trim(), out salary))
            {
                ShowAlert("Please enter a valid numeric salary.");
                return;
            }

            try
            {
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@EmpId", txtAddEmpId.Text.Trim()),
                    new SqlParameter("@Name", txtAddName.Text.Trim()),
                    new SqlParameter("@Designation", txtAddDesignation.Text.Trim()),
                    new SqlParameter("@DOB", Convert.ToDateTime(txtAddDOB.Text)),
                    new SqlParameter("@DOJ", Convert.ToDateTime(txtAddDOJ.Text)),
                    new SqlParameter("@Salary", salary),
                    new SqlParameter("@Address", string.IsNullOrWhiteSpace(txtAddAddress.Text) ? "N/A" : txtAddAddress.Text.Trim())
                };

                if (txtAddEmpId.ReadOnly)
                {
                    DbHelper.ExecuteNonQuery("sp_UpdateEmployee", prms);
                }
                else
                {
                    DbHelper.ExecuteNonQuery("sp_AddEmployee", prms);
                }

                txtAddEmpId.ReadOnly = false;

                BindEmployeeGrid();
                LoadDesignations();

                ScriptManager.RegisterStartupScript(this, GetType(), "closeModalScript", "closeModal();", true);
            }
            catch (Exception ex)
            {
                ShowAlert("Error saving employee: " + ex.Message);
            }
        }

        private void ShowAlert(string message)
        {
            string cleanMsg = message.Replace("'", "\\'").Replace("\r", "").Replace("\n", "\\n");
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{cleanMsg}');", true);
        }
    }
}