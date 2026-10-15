namespace EmpExpenseTracker
{
    public partial class Expenses
    {
        protected global::System.Web.UI.WebControls.Panel pnlAlert;
        protected global::System.Web.UI.WebControls.Label lblAlertMessage;
        protected global::System.Web.UI.WebControls.Label lblTotalAmount;
        protected global::System.Web.UI.WebControls.Label lblTotalClaims;
        protected global::System.Web.UI.WebControls.Label lblMaxExpense;
        protected global::System.Web.UI.WebControls.ValidationSummary valSummary;
        protected global::System.Web.UI.WebControls.TextBox txtEmployeeName;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvEmployeeName;
        protected global::System.Web.UI.WebControls.DropDownList ddlCategory;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvCategory;
        protected global::System.Web.UI.WebControls.TextBox txtAmount;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvAmount;
        protected global::System.Web.UI.WebControls.RangeValidator rvAmount;
        protected global::System.Web.UI.WebControls.TextBox txtExpenseDate;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvExpenseDate;
        protected global::System.Web.UI.WebControls.RadioButtonList rblPaymentMethod;
        protected global::System.Web.UI.WebControls.CheckBox chkHasReceipt;
        protected global::System.Web.UI.WebControls.Button btnSubmit;
        protected global::System.Web.UI.WebControls.Button btnClear;
        protected global::System.Web.UI.WebControls.TextBox txtSearch;
        protected global::System.Web.UI.WebControls.Button btnSearch;
        protected global::System.Web.UI.WebControls.Button btnShowAll;
        protected global::System.Web.UI.WebControls.GridView gvExpenses;
    }
}
