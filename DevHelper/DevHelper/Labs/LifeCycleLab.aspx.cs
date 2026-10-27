using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

namespace DevHelper.Labs
{
    public partial class LifeCycleLab : Page
    {
        private List<string> _executionLog = new List<string>();

        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DevHelperDbConn"]?.ConnectionString
                    ?? "Data Source=ANUBHAW;Initial Catalog=Practice;Integrated Security=True;";
            }
        }

        // 1. PreInit
        protected void Page_PreInit(object sender, EventArgs e)
        {
            LogStep("1. Page_PreInit", "Master Page & Theme evaluated.");
        }

        // 2. Init
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            LogStep("2. Page_Init", "Server controls initialized with default properties.");
        }

        // 4. Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LogStep("4. Page_Load (!IsPostBack)", "Initial GET: Loading default state.");
                lblRequestType.Text = "Initial GET (First Load)";
                lblRequestType.CssClass = "badge bg-info text-dark";

                // Seed test box
                txtTestInput.Text = "Hello Web Forms!";
                LoadNotes();
            }
            else
            {
                LogStep("4. Page_Load (IsPostBack)", "PostBack POST: Control values restored from ViewState & Form.");
                lblRequestType.Text = "POST (PostBack Triggered)";
                lblRequestType.CssClass = "badge bg-warning text-dark";
            }
        }

        // 5. Control Event
        protected void btnPostBack_Click(object sender, EventArgs e)
        {
            string currentVal = txtTestInput.Text;
            LogStep("5. btnPostBack_Click", $"User clicked button. Current text in box: '{Server.HtmlEncode(currentVal)}'");
        }

        protected void btnResetLog_Click(object sender, EventArgs e)
        {
            txtTestInput.Text = "Reset test value";
            LogStep("5. btnResetLog_Click", "Log reset requested.");
        }

        // 6. PreRender
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            LogStep("6. Page_PreRender", "Last chance to modify controls before rendering HTML.");

            // Output all timeline entries to UI
            RenderTimeline();
        }

        private void LogStep(string stage, string detail)
        {
            string time = DateTime.Now.ToString("HH:mm:ss.fff");
            _executionLog.Add($"[{time}] <span class='text-warning fw-bold'>{stage}</span> ➔ {detail}");
        }

        private void RenderTimeline()
        {
            var sb = new StringBuilder();
            foreach (var log in _executionLog)
            {
                sb.AppendLine($"<div class='mb-1 pb-1 border-bottom border-secondary'>{log}</div>");
            }
            litTimelineLog.Text = sb.ToString();
        }

        // Notes Storage (SQL Server with fallback to Session)
        protected void btnSaveNotes_Click(object sender, EventArgs e)
        {
            string notes = txtNotes.Text.Trim();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "UPDATE Topics SET PracticeNotes = @Notes, LastUpdated = GETDATE() WHERE Slug = 'page-lifecycle'";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Notes", notes);
                        cmd.ExecuteNonQuery();
                    }
                }
                lblNoteStatus.Text = "<i class='bi bi-check-circle-fill'></i> Saved to Database!";
            }
            catch
            {
                // Fallback to Session
                Session["Notes_PageLifeCycle"] = notes;
                lblNoteStatus.Text = "<i class='bi bi-check-circle-fill'></i> Saved to Session!";
            }
        }

        private void LoadNotes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "SELECT PracticeNotes FROM Topics WHERE Slug = 'page-lifecycle'";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            txtNotes.Text = result.ToString();
                            return;
                        }
                    }
                }
            }
            catch
            {
                // Ignore if DB not ready
            }

            if (Session["Notes_PageLifeCycle"] != null)
            {
                txtNotes.Text = Session["Notes_PageLifeCycle"].ToString();
            }
        }
    }
}
