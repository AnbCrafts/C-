using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormPracticeProject.Models;

namespace WebFormPracticeProject
{
    public partial class Students : Page
    {
        // Session key for storing our students list in memory
        private const string SessionStudentsKey = "App_StudentsList";

        // Helper property to access the student repository stored in Session
        private List<Student> StudentStore
        {
            get
            {
                if (Session[SessionStudentsKey] == null)
                {
                    // Initialize with some realistic starter data
                    Session[SessionStudentsKey] = new List<Student>
                    {
                        new Student { Id = 1, Name = "Alice Johnson", Email = "alice@example.com", Age = 21, Course = "Computer Science", Gender = "Female", IsActive = true, EnrollmentDate = DateTime.Today.AddDays(-120) },
                        new Student { Id = 2, Name = "Bob Smith", Email = "bob@example.com", Age = 24, Course = "Data Science", Gender = "Male", IsActive = true, EnrollmentDate = DateTime.Today.AddDays(-90) },
                        new Student { Id = 3, Name = "Charlie Davis", Email = "charlie@example.com", Age = 22, Course = "Information Technology", Gender = "Other", IsActive = false, EnrollmentDate = DateTime.Today.AddDays(-45) },
                        new Student { Id = 4, Name = "Diana Prince", Email = "diana@example.com", Age = 20, Course = "Cybersecurity", Gender = "Female", IsActive = true, EnrollmentDate = DateTime.Today.AddDays(-10) }
                    };
                }
                return (List<Student>)Session[SessionStudentsKey];
            }
            set
            {
                Session[SessionStudentsKey] = value;
            }
        }

        /// <summary>
        /// Page_Load is the heart of Web Forms page life cycle.
        /// It fires on EVERY request: initial GET and subsequent POST (PostBack).
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // CRITICAL CONCEPT: IsPostBack
            // If !IsPostBack: this is the first time the user visits the page (HTTP GET).
            // If IsPostBack == true: the user clicked a button or triggered a postback (HTTP POST).
            // We only want to populate initial data on the FIRST visit, otherwise we would wipe out user input!
            if (!IsPostBack)
            {
                BindStudentGrid();
            }
        }

        /// <summary>
        /// Handles the "Save Student" button click.
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Always check Page.IsValid to ensure server-side validation has passed!
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                // Reading values directly from ASP.NET Server Controls
                string name = txtName.Text.Trim();
                string email = txtEmail.Text.Trim();
                int age = int.Parse(txtAge.Text.Trim());
                string course = ddlCourse.SelectedValue;
                string gender = rblGender.SelectedValue;
                bool isActive = chkIsActive.Checked;

                var list = StudentStore;
                int nextId = list.Count > 0 ? list.Max(s => s.Id) + 1 : 1;

                var newStudent = new Student
                {
                    Id = nextId,
                    Name = name,
                    Email = email,
                    Age = age,
                    Course = course,
                    Gender = gender,
                    IsActive = isActive,
                    EnrollmentDate = DateTime.Today
                };

                list.Add(newStudent);
                StudentStore = list; // Update Session

                ShowNotification($"Student <strong>{Server.HtmlEncode(name)}</strong> added successfully!", "alert-success");

                // Reset form fields
                ClearInputForm();

                // Refresh the GridView with the updated data
                BindStudentGrid();
            }
            catch (Exception ex)
            {
                ShowNotification($"An error occurred: {Server.HtmlEncode(ex.Message)}", "alert-danger");
            }
        }

        /// <summary>
        /// Clears all input controls.
        /// </summary>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearInputForm();
            pnlAlert.Visible = false;
        }

        /// <summary>
        /// Filters the GridView based on user search query.
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(query))
            {
                BindStudentGrid();
                return;
            }

            var filtered = StudentStore.Where(s =>
                s.Name.ToLower().Contains(query) ||
                s.Email.ToLower().Contains(query) ||
                s.Course.ToLower().Contains(query)
            ).ToList();

            BindStudentGrid(filtered);
        }

        /// <summary>
        /// Resets search filter and reloads all students.
        /// </summary>
        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            BindStudentGrid();
        }

        /// <summary>
        /// Handles row deletion in the GridView.
        /// </summary>
        protected void gvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Retrieve the ID using DataKeys
            int studentId = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);

            var list = StudentStore;
            var studentToRemove = list.FirstOrDefault(s => s.Id == studentId);

            if (studentToRemove != null)
            {
                list.Remove(studentToRemove);
                StudentStore = list;
                ShowNotification($"Removed student <strong>{Server.HtmlEncode(studentToRemove.Name)}</strong>.", "alert-warning");
            }

            BindStudentGrid();
        }

        /// <summary>
        /// Binds data to the GridView server control.
        /// In Web Forms, DataSource + DataBind() converts objects into HTML table markup.
        /// </summary>
        private void BindStudentGrid(List<Student> data = null)
        {
            var listToBind = data ?? StudentStore;
            gvStudents.DataSource = listToBind;
            gvStudents.DataBind();

            lblTotalCount.Text = $"{listToBind.Count} {(listToBind.Count == 1 ? "Student" : "Students")}";
        }

        private void ClearInputForm()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAge.Text = string.Empty;
            ddlCourse.SelectedIndex = 0;
            rblGender.SelectedIndex = 0;
            chkIsActive.Checked = true;
        }

        private void ShowNotification(string message, string alertCssClass)
        {
            pnlAlert.CssClass = $"alert {alertCssClass} alert-dismissible fade show";
            lblAlertMessage.Text = message;
            pnlAlert.Visible = true;
        }
    }
}
