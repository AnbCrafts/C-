using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevHelper.Models;

namespace DevHelper
{
    public partial class _Default : Page
    {
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DevHelperDbConn"]?.ConnectionString
                    ?? "Data Source=ANUBHAW;Initial Catalog=Practice;Integrated Security=True;";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadModulesAndTopics();
            }
        }

        private void LoadModulesAndTopics()
        {
            List<ModuleItem> modules = null;

            // 1. Try loading dynamically from SQL Server Database
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    // Check if Modules table exists
                    string checkSql = "SELECT COUNT(1) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Modules'";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (exists > 0)
                        {
                            modules = FetchFromDatabase(conn);
                            lblDbStatus.Text = "Connected (SQL Server)";
                            lblDbStatus.CssClass = "badge bg-success";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Show friendly alert if SQL Server is not reached
                lblDbStatus.Text = "Offline Mode (In-Memory)";
                lblDbStatus.CssClass = "badge bg-warning text-dark";
                pnlDbAlert.Visible = true;
                lblDbAlert.Text = "SQL Server connection note: " + ex.Message + ". Running in In-Memory mode. To use SQL Server, execute <code>F:\\MCC_Practice\\SQL\\DevHelper_DB.sql</code> in SSMS.";
            }

            // 2. Fallback to In-Memory Seed Data if database is not yet initialized
            if (modules == null || modules.Count == 0)
            {
                modules = GetFallbackSeedData();
            }

            // 3. Compute Summary Statistics
            int totalMods = modules.Count;
            int totalTop = modules.Sum(m => m.Topics.Count);
            int compTop = modules.Sum(m => m.Topics.Count(t => t.Status == "Completed"));

            lblTotalModules.Text = totalMods.ToString();
            lblTotalTopics.Text = totalTop.ToString();
            lblCompletedTopics.Text = compTop.ToString();

            // 4. Bind to Repeater
            rptModules.DataSource = modules;
            rptModules.DataBind();
        }

        private List<ModuleItem> FetchFromDatabase(SqlConnection conn)
        {
            var moduleList = new List<ModuleItem>();

            // Query Modules
            string modQuery = "SELECT ModuleId, ModuleName, ShortDesc, IconClass, BadgeColor, DisplayOrder FROM Modules ORDER BY DisplayOrder ASC";
            using (SqlCommand cmd = new SqlCommand(modQuery, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    moduleList.Add(new ModuleItem
                    {
                        ModuleId = Convert.ToInt32(reader["ModuleId"]),
                        ModuleName = reader["ModuleName"].ToString(),
                        ShortDesc = reader["ShortDesc"].ToString(),
                        IconClass = reader["IconClass"].ToString(),
                        BadgeColor = reader["BadgeColor"].ToString(),
                        DisplayOrder = Convert.ToInt32(reader["DisplayOrder"])
                    });
                }
            }

            // Query Topics
            string topQuery = "SELECT TopicId, ModuleId, Title, Slug, PageUrl, Difficulty, Summary, Status FROM Topics ORDER BY TopicId ASC";
            using (SqlCommand cmd = new SqlCommand(topQuery, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int modId = Convert.ToInt32(reader["ModuleId"]);
                    var parentMod = moduleList.FirstOrDefault(m => m.ModuleId == modId);
                    if (parentMod != null)
                    {
                        parentMod.Topics.Add(new TopicItem
                        {
                            TopicId = Convert.ToInt32(reader["TopicId"]),
                            ModuleId = modId,
                            Title = reader["Title"].ToString(),
                            Slug = reader["Slug"].ToString(),
                            PageUrl = reader["PageUrl"].ToString(),
                            Difficulty = reader["Difficulty"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }

            return moduleList;
        }

        protected void rptModules_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var mod = (ModuleItem)e.Item.DataItem;
                var rptTopics = (Repeater)e.Item.FindControl("rptTopics");
                if (rptTopics != null && mod != null)
                {
                    rptTopics.DataSource = mod.Topics;
                    rptTopics.DataBind();
                }
            }
        }

        protected string GetStatusBadgeClass(string status)
        {
            switch (status)
            {
                case "Completed":
                    return "bg-success";
                case "In Progress":
                    return "bg-primary";
                default:
                    return "bg-secondary";
            }
        }

        private List<ModuleItem> GetFallbackSeedData()
        {
            return new List<ModuleItem>
            {
                new ModuleItem
                {
                    ModuleId = 1,
                    ModuleName = "1. Architecture & Core Mental Model",
                    ShortDesc = "How Web Forms works, Code-Behind, and the Page Life Cycle.",
                    IconClass = "bi-diagram-3",
                    BadgeColor = "bg-primary",
                    DisplayOrder = 1,
                    Topics = new List<TopicItem>
                    {
                        new TopicItem { TopicId = 1, ModuleId = 1, Title = "Page Life Cycle & IsPostBack", Slug = "page-lifecycle", PageUrl = "~/Labs/LifeCycleLab.aspx", Difficulty = "Beginner", Summary = "Understand the 9 lifecycle stages and prevent wiping data on postback.", Status = "Completed" }
                    }
                },
                new ModuleItem
                {
                    ModuleId = 2,
                    ModuleName = "2. Server Controls & Validation",
                    ShortDesc = "Standard web controls and two-layer validation.",
                    IconClass = "bi-input-cursor-text",
                    BadgeColor = "bg-success",
                    DisplayOrder = 2,
                    Topics = new List<TopicItem>
                    {
                        new TopicItem { TopicId = 2, ModuleId = 2, Title = "Server Controls & Two-Layer Validation", Slug = "validation-controls", PageUrl = "~/Labs/ValidationLab.aspx", Difficulty = "Beginner", Summary = "RequiredField, Range, RegularExpression, and Compare validators.", Status = "In Progress" }
                    }
                },
                new ModuleItem
                {
                    ModuleId = 3,
                    ModuleName = "3. Master Pages & Reusable Layouts",
                    ShortDesc = "Layout templates and reusable user controls (.ascx).",
                    IconClass = "bi-window-stack",
                    BadgeColor = "bg-info",
                    DisplayOrder = 3,
                    Topics = new List<TopicItem>
                    {
                        new TopicItem { TopicId = 3, ModuleId = 3, Title = "Master Pages & User Controls (.ascx)", Slug = "master-pages", PageUrl = "~/Labs/MasterPagesLab.aspx", Difficulty = "Beginner", Summary = "ContentPlaceHolder, Site.Master, and modular widgets.", Status = "Not Started" }
                    }
                },
                new ModuleItem
                {
                    ModuleId = 4,
                    ModuleName = "4. State Management",
                    ShortDesc = "ViewState, Session, Cookies, Application state, and query strings.",
                    IconClass = "bi-database-check",
                    BadgeColor = "bg-warning text-dark",
                    DisplayOrder = 4,
                    Topics = new List<TopicItem>
                    {
                        new TopicItem { TopicId = 4, ModuleId = 4, Title = "ViewState vs Session vs Application", Slug = "state-management", PageUrl = "~/Labs/StateLab.aspx", Difficulty = "Intermediate", Summary = "How Web Forms preserves state across stateless HTTP requests.", Status = "Not Started" }
                    }
                },
                new ModuleItem
                {
                    ModuleId = 5,
                    ModuleName = "5. Data Binding & Rich Grid Controls",
                    ShortDesc = "GridView, Repeater, ListView, DataTables, and ADO.NET.",
                    IconClass = "bi-table",
                    BadgeColor = "bg-danger",
                    DisplayOrder = 5,
                    Topics = new List<TopicItem>
                    {
                        new TopicItem { TopicId = 5, ModuleId = 5, Title = "GridView CRUD & Data Binding Lab", Slug = "gridview-crud", PageUrl = "~/Labs/GridViewLab.aspx", Difficulty = "Intermediate", Summary = "Connect SQL Server DataTables to GridView with sorting, editing, and deleting.", Status = "Not Started" }
                    }
                },
                new ModuleItem
                {
                    ModuleId = 6,
                    ModuleName = "6. ASP.NET AJAX & Partial Rendering",
                    ShortDesc = "ScriptManager, UpdatePanel, and flicker-free partial updates.",
                    IconClass = "bi-lightning-charge",
                    BadgeColor = "bg-secondary",
                    DisplayOrder = 6,
                    Topics = new List<TopicItem>
                    {
                        new TopicItem { TopicId = 6, ModuleId = 6, Title = "UpdatePanel & Partial PostBack (AJAX)", Slug = "updatepanel-ajax", PageUrl = "~/Labs/AjaxLab.aspx", Difficulty = "Intermediate", Summary = "Update parts of your webpage asynchronously without page flickers.", Status = "Not Started" }
                    }
                },
                new ModuleItem
                {
                    ModuleId = 7,
                    ModuleName = "7. Security & Authentication",
                    ShortDesc = "Forms Authentication, user sessions, role management, and SQL safety.",
                    IconClass = "bi-shield-lock",
                    BadgeColor = "bg-dark",
                    DisplayOrder = 7,
                    Topics = new List<TopicItem>
                    {
                        new TopicItem { TopicId = 7, ModuleId = 7, Title = "Forms Authentication & User Sessions", Slug = "auth-security", PageUrl = "~/Labs/AuthLab.aspx", Difficulty = "Advanced", Summary = "Implement login tickets, password protection, and role-based page guards.", Status = "Not Started" }
                    }
                },
                new ModuleItem
                {
                    ModuleId = 8,
                    ModuleName = "8. Bridge: Web Forms to Web API",
                    ShortDesc = "From server-rendered HTML pages to JSON/RESTful HTTP services.",
                    IconClass = "bi-arrow-left-right",
                    BadgeColor = "bg-primary",
                    DisplayOrder = 8,
                    Topics = new List<TopicItem>
                    {
                        new TopicItem { TopicId = 8, ModuleId = 8, Title = "Calling Web Methods & Transition to Web API", Slug = "webforms-to-webapi", PageUrl = "~/Labs/WebMethodLab.aspx", Difficulty = "Advanced", Summary = "Expose [WebMethod] endpoints for client fetch() and prepare for Web API.", Status = "Not Started" }
                    }
                }
            };
        }
    }
}
