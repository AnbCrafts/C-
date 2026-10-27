namespace DevHelper.Labs
{
    public partial class ValidationLab
    {
        protected global::System.Web.UI.WebControls.Label lblValidationResult;
        protected global::System.Web.UI.WebControls.ValidationSummary valSummary;
        protected global::System.Web.UI.WebControls.TextBox txtName;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvName;
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvEmail;
        protected global::System.Web.UI.WebControls.RegularExpressionValidator revEmail;
        protected global::System.Web.UI.WebControls.TextBox txtAge;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvAge;
        protected global::System.Web.UI.WebControls.RangeValidator rvAge;
        protected global::System.Web.UI.WebControls.TextBox txtPassword;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvPass;
        protected global::System.Web.UI.WebControls.TextBox txtConfirmPassword;
        protected global::System.Web.UI.WebControls.CompareValidator cvPass;
        protected global::System.Web.UI.WebControls.Button btnValidate;
        protected global::System.Web.UI.WebControls.Button btnClear;
        protected global::System.Web.UI.WebControls.Panel pnlFeedback;
        protected global::System.Web.UI.WebControls.Label lblFeedback;
    }
}
