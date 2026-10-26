# ASP.NET WebForms GridView & LINQ to SQL CRUD Guide

This documentation tracks our step-by-step masterclass on GridView data presentation, template fields, and full CUD (Create, Update, Delete) operations using LINQ to SQL.

---

## 📋 Course Syllabus

- [x] **Topic 1: `BoundField` Deep Dive**
- [x] **Topic 2: `TemplateField` Deep Dive**
- [ ] **Topic 3: `BoundField` vs `TemplateField`: Head-to-Head Comparison**
- [x] **CRUD Operations with GridView & LINQ to SQL (Create, Read, Update, Delete)**
- [x] **CRUD Operations with GridView & ADO.NET (Traditional Approach)**
- [ ] **Editing / Deleting Specific Single Cells**
- [ ] **Advanced Template Customization & Events (`RowDataBound`, `Eval` vs `Bind`)**

---

## Topic 1: `BoundField` Deep Dive

### 1. What is a `BoundField`?
A `BoundField` is the default, built-in column type in an ASP.NET `GridView`. It represents a field that is bound directly to a data source (such as a database column or LINQ entity property) without requiring custom nested templates.

### 2. Complete List of `BoundField` Attributes

| Attribute | Type | Description | Example |
| :--- | :--- | :--- | :--- |
| **`DataField`** | `string` | The property/column name from the data source. **Case-sensitive.** | `DataField="UserName"` |
| **`HeaderText`** | `string` | The title rendered in the table header (`<th>`). | `HeaderText="User Name"` |
| **`SortExpression`**| `string` | Enables sorting on this column when `AllowSorting="true"`. | `SortExpression="UserName"` |
| **`ReadOnly`** | `bool` | If `true`, the field remains static text during edit mode. Ideal for IDs and timestamps. | `ReadOnly="true"` |
| **`Visible`** | `bool` | Controls column visibility. *(Warning: `Visible="false"` stops the column from rendering in HTML entirely).* | `Visible="false"` |
| **`DataFormatString`**| `string` | Formats values (currency, date, numeric) using .NET standard format specifiers. | `DataFormatString="{0:C2}"` |
| **`ApplyFormatInEditMode`**| `bool` | When `true`, applies the `DataFormatString` even inside the edit textbox. Default is `false`. | `ApplyFormatInEditMode="false"` |
| **`NullDisplayText`**| `string` | Text displayed if the data source contains `null` or `DBNull`. | `NullDisplayText="N/A"` |
| **`ConvertEmptyStringToNull`**| `bool` | When editing, if user leaves the textbox blank, converts `""` to `null` before sending to SQL. Default is `true`. | `ConvertEmptyStringToNull="true"` |
| **`HtmlEncode`** | `bool` | Automatically encodes characters (`<`, `>`, `&`) to prevent Cross-Site Scripting (XSS). Default is `true`. | `HtmlEncode="true"` |
| **`HtmlEncodeFormatString`**| `bool` | If `true`, HTML-encodes the formatted result of `DataFormatString`. Default is `true`. | `HtmlEncodeFormatString="false"` |
| **`ItemStyle-Width`** | `Unit` | Sets cell width in pixels or percentages. | `ItemStyle-Width="150px"` |
| **`ItemStyle-HorizontalAlign`**| `enum` | Aligns text inside cells (`Left`, `Center`, `Right`, `Justify`). | `ItemStyle-HorizontalAlign="Right"` |
| **`HeaderStyle-CssClass`**| `string` | Applies a CSS class to the header cell (`<th>`). | `HeaderStyle-CssClass="col-header"` |

### 3. Common `DataFormatString` Cheat Sheet
* **Currency**: `{0:C}` or `{0:C2}` $\rightarrow$ `$1,234.50`
* **Short Date**: `{0:yyyy-MM-dd}` $\rightarrow$ `2026-10-26`
* **Long Date & Time**: `{0:dd/MM/yyyy hh:mm tt}` $\rightarrow$ `26/10/2026 11:30 AM`
* **Integer**: `{0:N0}` $\rightarrow$ `1,000`
* **Decimal**: `{0:N2}` $\rightarrow$ `1,000.00`
* **Percentage**: `{0:P}` $\rightarrow$ `25.5%`

### 4. Implementation Example in Markup (`.aspx`)
```html
<asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" 
              DataKeyNames="UserId" OnRowUpdating="gvUsers_RowUpdating" 
              OnRowEditing="gvUsers_RowEditing" OnRowCancelingEdit="gvUsers_RowCancelingEdit">
    <Columns>
        <asp:BoundField DataField="UserId" HeaderText="User ID" ReadOnly="true" 
                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px" />
        <asp:BoundField DataField="UserName" HeaderText="Username" 
                        NullDisplayText="[No Username]" />
        <asp:BoundField DataField="QueryAmount" HeaderText="Amount" 
                        DataFormatString="{0:C2}" ApplyFormatInEditMode="false" 
                        ItemStyle-HorizontalAlign="Right" />
        <asp:CommandField ShowEditButton="true" />
    </Columns>
</asp:GridView>
```

### 5. Accessing Values in C# Code-Behind (`.cs`)
```csharp
protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
{
    int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

    // Read new values typed by user
    string newUserName = e.NewValues["UserName"]?.ToString();
    string oldUserName = e.OldValues["UserName"]?.ToString();

    // Call Stored Procedure
    using (QueryUserContext db = new QueryUserContext())
    {
        db.udp_UpdateUser(userId, newUserName, ...);
    }

    gvUsers.EditIndex = -1;
    BindUserData();
}
```

---

## Topic 2: `TemplateField` Deep Dive

### 1. What is a `TemplateField`?
A `TemplateField` provides complete customization of cell layout, HTML, server controls, validation, and data binding. Unlike `BoundField` which only generates plain text or a simple textbox, a `TemplateField` can host any ASP.NET control (`DropDownList`, `CheckBox`, `FileUpload`, `Calendar`, custom buttons).

### 2. The 5 Internal Templates
| Template | When It Renders | Purpose & Typical Controls |
| :--- | :--- | :--- |
| **`<HeaderTemplate>`** | Inside column header (`<th>`) | Custom header labels, icons, "Select All" CheckBoxes, column search filters. |
| **`<ItemTemplate>`** | Normal display mode | Display data using `<asp:Label>`, `<img>`, hyperlinks, or badges. |
| **`<EditItemTemplate>`** | Row is in edit mode (`EditIndex == RowIndex`) | Input controls: `<asp:TextBox>`, `<asp:DropDownList>`, `<asp:CheckBox>`. |
| **`<FooterTemplate>`** | In footer row (`ShowFooter="true"`) | Blank inputs and buttons for adding new records inline. |
| **`<AlternatingItemTemplate>`** | Alternating data rows | Optional alternating row styling or customized layout. |

### 3. Data Binding Expressions: `Eval()` vs `Bind()`
* **`Eval("PropertyName")` (One-Way / Read-Only):**
  * Evaluates data from the data source and renders it as text.
  * Used in `<ItemTemplate>`.
  * Supports format strings: `<%# Eval("QueryAmount", "{0:C2}") %>`.
  * Supports C# helper methods: `<%# FormatStatus(Eval("IsActive")) %>`.
* **`Bind("PropertyName")` (Two-Way / Read-Write):**
  * Displays existing data and extracts user input automatically upon postback.
  * Used in `<EditItemTemplate>`.
  * Example: `<asp:TextBox ID="txtEditEmail" runat="server" Text='<%# Bind("Email") %>' />`.

### 4. Implementation Example with Different Controls (`.aspx`)
```html
<asp:TemplateField HeaderText="Gender">
    <ItemTemplate>
        <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>' />
    </ItemTemplate>
    <EditItemTemplate>
        <asp:DropDownList ID="ddlEditGender" runat="server">
            <asp:ListItem Text="Select" Value="" />
            <asp:ListItem Text="Male" Value="Male" />
            <asp:ListItem Text="Female" Value="Female" />
            <asp:ListItem Text="Other" Value="Other" />
        </asp:DropDownList>
    </EditItemTemplate>
    <FooterTemplate>
        <asp:DropDownList ID="ddlNewGender" runat="server">
            <asp:ListItem Text="Select" Value="" />
            <asp:ListItem Text="Male" Value="Male" />
            <asp:ListItem Text="Female" Value="Female" />
        </asp:DropDownList>
    </FooterTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Active Status">
    <ItemTemplate>
        <asp:CheckBox ID="chkIsActive" runat="server" 
                      Checked='<%# Eval("IsActive") != null && Convert.ToBoolean(Eval("IsActive")) %>' 
                      Enabled="false" />
    </ItemTemplate>
    <EditItemTemplate>
        <asp:CheckBox ID="chkEditIsActive" runat="server" 
                      Checked='<%# Eval("IsActive") != null && Convert.ToBoolean(Eval("IsActive")) %>' />
    </EditItemTemplate>
    <FooterTemplate>
        <asp:CheckBox ID="chkNewIsActive" runat="server" Checked="true" />
    </FooterTemplate>
</asp:TemplateField>
```

### 5. Accessing Controls in C# Code-Behind (`.cs`)
Controls inside a `TemplateField` must be accessed using `.FindControl("ControlID")` scoped to the specific row:

#### A. In `RowUpdating` (Editing Row)
```csharp
protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
{
    GridViewRow row = gvUsers.Rows[e.RowIndex];

    TextBox txtName = (TextBox)row.FindControl("txtEditUserName");
    DropDownList ddlGen = (DropDownList)row.FindControl("ddlEditGender");
    CheckBox chkAct = (CheckBox)row.FindControl("chkEditIsActive");

    string userName = txtName?.Text.Trim();
    string gender = ddlGen?.SelectedValue;
    bool isActive = chkAct != null && chkAct.Checked;
}
```

#### B. In `RowCommand` (Footer Row for Insert)
```csharp
protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
{
    if (e.CommandName == "AddNew")
    {
        GridViewRow footer = gvUsers.FooterRow;

        TextBox txtNewUser = (TextBox)footer.FindControl("txtNewUserName");
        DropDownList ddlNewGen = (DropDownList)footer.FindControl("ddlNewGender");

        string userName = txtNewUser?.Text.Trim();
        string gender = ddlNewGen?.SelectedValue;

        // Call Insert Stored Procedure
    }
}
```

#### C. In `RowDataBound` (Setting DropDown SelectedValue Safely)
```csharp
protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
{
    if (e.Row.RowType == DataControlRowType.DataRow && 
        (e.Row.RowState & DataControlRowState.Edit) > 0)
    {
        DropDownList ddl = (DropDownList)e.Row.FindControl("ddlEditGender");
        if (ddl != null)
        {
            string currentVal = DataBinder.Eval(e.Row.DataItem, "Gender")?.ToString();
            ListItem item = ddl.Items.FindByValue(currentVal);
            if (item != null)
            {
                ddl.SelectedValue = currentVal;
            }
        }
    }
}
```

### 6. Critical Gotchas
1. **`SelectedValue` Crash in Markup**: Avoid `SelectedValue='<%# Bind("Gender") %>'` if database values might be `null` or not present in `<asp:ListItem>`. Set it safely in `RowDataBound` instead.
2. **`FindControl` Returns `null` on ID Mismatch**: If the ID in `.FindControl("id")` doesn't match the `.aspx` control ID exactly, it returns `null` and leads to a `NullReferenceException`.
3. **Template Validation**: You can embed `<asp:RequiredFieldValidator>` or `<asp:RegularExpressionValidator>` directly inside `<EditItemTemplate>` and set `ValidationGroup` to avoid triggering other form buttons.

---

### 7. Deep Dive: The Anatomy of `row.FindControl(...)`

#### Why Direct Variable Access Fails
In a normal WebForm, `<asp:TextBox ID="txtName" runat="server" />` generates a single C# variable `txtName`.
Inside a GridView, controls repeat for **every row**. With 50 users, there are 50 separate textboxes with the same ID. Therefore, ASP.NET cannot generate a page-level variable. You must look inside a specific row instance.

#### Syntax Breakdown
```csharp
TextBox txtUser = (TextBox)row.FindControl("txtEditUserName");
DropDownList ddlGen = (DropDownList)row.FindControl("ddlEditGender");
CheckBox chkAct = (CheckBox)row.FindControl("chkEditIsActive");
```

1. **`row` (`GridViewRow`):** Limits the search scope exclusively to the specific table row (`<tr>`).
2. **`.FindControl("txtEditUserName")`:** Recursively searches the row's control tree for that ID. **Returns a generic `System.Web.UI.Control`**.
3. **`(TextBox)` Type Casting:** 
   * A generic `Control` base class only contains basic properties like `ID` and `Visible`. It does **not** have `.Text`, `.Checked`, or `.SelectedValue`.
   * Casting informs the compiler to treat the object as a concrete control type (`TextBox`, `DropDownList`, `CheckBox`), giving access to all type-specific properties.

#### Defensive Coding Pattern (Avoiding `NullReferenceException`)
If an ID is mistyped, `.FindControl()` returns `null`. Attempting `.Text` on a `null` object will crash the application.
```csharp
// Safe navigation with null-conditional operator:
string userName = txtUser?.Text.Trim();

// Or explicit null check:
if (txtUser != null)
{
    string userName = txtUser.Text.Trim();
}
```

---

### 8. Deep Dive: Understanding `RowCommand`

`RowCommand` is the **Universal Event Dispatcher** for the GridView.

#### Why `RowCommand` Exists
Standard GridView events are specialized:
* `RowEditing` $\rightarrow$ Only handles the built-in "Edit" button.
* `RowDeleting` $\rightarrow$ Only handles the built-in "Delete" button.

If you add **custom buttons** (e.g., "Add New" in the footer, "Download PDF", "Send Email", "Approve"), they all bubble their click events up to the parent GridView's **`RowCommand`** event.

#### Key Event Arguments (`GridViewCommandEventArgs e`)
* **`e.CommandName`**: Identifies *which* button was clicked (e.g., `"AddNew"`, `"SendEmail"`, `"Download"`).
* **`e.CommandArgument`**: Extra payload attached to the button (e.g., row ID: `CommandArgument='<%# Eval("UserId") %>'`).

#### Implementation Example
```csharp
protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
{
    // Handle Footer Insert
    if (e.CommandName == "AddNew")
    {
        GridViewRow footer = gvUsers.FooterRow;
        TextBox txtName = (TextBox)footer.FindControl("txtNewUserName");

        using (QueryUserContext db = new QueryUserContext())
        {
            db.udp_CreateUser(txtName.Text.Trim(), /* other parameters */);
        }

        BindUserData();
    }
    // Handle Custom Row Action
    else if (e.CommandName == "SendWelcomeEmail")
    {
        int userId = Convert.ToInt32(e.CommandArgument);
        // Send email logic for that specific user
    }
}
```

---

### 9. Deep Dive: Understanding `RowDataBound`

`RowDataBound` acts as the **Assembly Line Quality Inspector** during table generation.

#### Lifecycle & Execution
* Fires during `gvUsers.DataBind()`.
* Fires **once for every single row** created:
  1. Header Row (`DataControlRowType.Header`)
  2. Data Rows (`DataControlRowType.DataRow`) — 1 execution per record
  3. Footer Row (`DataControlRowType.Footer`)
  4. Pager Row (`DataControlRowType.Pager`)

#### Top 3 Use Cases

1. **Row-by-Row Conditional Formatting / Styling:**
   ```csharp
   protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
   {
       if (e.Row.RowType == DataControlRowType.DataRow)
       {
           var user = (udp_GetAllUsersResult)e.Row.DataItem;

           // Highlight inactive users in grey
           if (user.IsActive == false)
           {
               e.Row.BackColor = System.Drawing.Color.LightGray;
           }

           // Highlight high-value queries in bold red
           if (user.QueryAmount > 5000)
           {
               e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
               e.Row.Cells[11].Font.Bold = true;
           }
       }
   }
   ```

2. **Safe DropDownList Pre-Selection in Edit Mode:**
   ```csharp
   protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
   {
       if (e.Row.RowType == DataControlRowType.DataRow && 
           (e.Row.RowState & DataControlRowState.Edit) > 0)
       {
           DropDownList ddl = (DropDownList)e.Row.FindControl("ddlEditGender");
           if (ddl != null)
           {
               string gender = DataBinder.Eval(e.Row.DataItem, "Gender")?.ToString();
               ListItem item = ddl.Items.FindByValue(gender);
               if (item != null)
               {
                   ddl.SelectedValue = gender;
               }
           }
       }
   }
   ```

3. **Dynamic Client-Side Confirmation Messages with Row Data:**
   ```csharp
   protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
   {
       if (e.Row.RowType == DataControlRowType.DataRow)
       {
           LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");
           if (btnDelete != null)
           {
               string userName = DataBinder.Eval(e.Row.DataItem, "UserName")?.ToString();
               btnDelete.OnClientClick = $"return confirm('Are you sure you want to delete {userName}?');";
           }
       }
   }
   ```


---

## CRUD Operations with GridView & LINQ to SQL

This section provides the end-to-end implementation of all CRUD (Create, Read, Update, Delete) methods in ASP.NET WebForms using **LINQ to SQL**.

### 1. Architectural Flow

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              ASP.NET GRIDVIEW                               │
├──────────────┬───────────────────────────────┬──────────────────────────────┤
│  Operation   │         GridView Event        │      C# Backend Method       │
├──────────────┼───────────────────────────────┼──────────────────────────────┤
│  READ (R)    │  Page_Load (!IsPostBack)      │  BindUserData()              │
│  CREATE (C)  │  RowCommand (Name="AddNew")   │  gvDetails_RowCommand(...)   │
│  EDIT MODE   │  RowEditing                   │  gvDetails_RowEditing(...)   │
│  CANCEL EDIT │  RowCancelingEdit             │  gvDetails_RowCancelingEdit()│
│  UPDATE (U)  │  RowUpdating                  │  gvDetails_RowUpdating(...)  │
│  DELETE (D)  │  RowDeleting                  │  gvDetails_RowDeleting(...)  │
│  PAGING      │  PageIndexChanging            │  gvDetails_PageIndexChanging │
└──────────────┴───────────────────────────────┴──────────────────────────────┘
                                     │
                                     ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                    LINQ to SQL DATA CONTEXT (QueryUserContext)              │
├─────────────────────────────────────────────────────────────────────────────┤
│  • dataContext.udp_GetAllUsers()                                            │
│  • dataContext.udp_CreateUser(...)                                          │
│  • dataContext.udp_UpdateUser(...)                                          │
│  • dataContext.udp_DeleteUserById(userId)                                   │
│  (Or Pure LINQ Entity Methods: InsertOnSubmit, DeleteOnSubmit, SubmitChanges)│
└─────────────────────────────────────────────────────────────────────────────┘
```

---

### 2. The CRUD Methods in Code-Behind (`.cs`)

#### A. READ (Populating the GridView)

```csharp
private void BindUserData()
{
    using (QueryUserContext dataContext = new QueryUserContext())
    {
        // Method 1: Using Mapped Stored Procedure (Fastest / Recommended)
        gvUsers.DataSource = dataContext.udp_GetAllUsers().ToList();

        // Method 2: Using Pure LINQ Query Expression (Alternative)
        // var users = from u in dataContext.QueryUsers
        //             orderby u.UserId descending
        //             select u;
        // gvUsers.DataSource = users.ToList();

        gvUsers.DataBind();
    }
}
```

* **`!IsPostBack` Rule:** Always wrap the initial `BindUserData()` in `if (!IsPostBack)` in `Page_Load` so that user edits in textboxes are not overwritten during a postback:
  ```csharp
  protected void Page_Load(object sender, EventArgs e)
  {
      if (!IsPostBack)
      {
          BindUserData();
      }
  }
  ```

---

#### B. CREATE (Insert via GridView Footer)

Triggered when the user clicks the "Add New" button in the `<FooterTemplate>`:

```csharp
protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
{
    if (e.CommandName.Equals("AddNew"))
    {
        // 1. Locate the FooterRow
        GridViewRow footer = gvUsers.FooterRow;

        // 2. Extract values using FindControl
        string userName   = ((TextBox)footer.FindControl("txtNewUserName"))?.Text.Trim();
        string email      = ((TextBox)footer.FindControl("txtNewEmail"))?.Text.Trim();
        string fullName   = ((TextBox)footer.FindControl("txtNewFullName"))?.Text.Trim();
        string mobileNo   = ((TextBox)footer.FindControl("txtNewMobileNo"))?.Text.Trim();
        string city       = ((TextBox)footer.FindControl("txtNewCity"))?.Text.Trim();
        string stateName  = ((TextBox)footer.FindControl("txtNewStateName"))?.Text.Trim();
        string country    = ((TextBox)footer.FindControl("txtNewCountry"))?.Text.Trim();
        string gender     = ((DropDownList)footer.FindControl("ddlNewGender"))?.SelectedValue;
        string queryText  = ((TextBox)footer.FindControl("txtNewQueryText"))?.Text.Trim();

        // Parse Decimal safely
        decimal? queryAmount = null;
        string amountText = ((TextBox)footer.FindControl("txtNewQueryAmount"))?.Text.Trim();
        if (decimal.TryParse(amountText, out decimal parsedAmount))
        {
            queryAmount = parsedAmount;
        }

        // 3. Execute Insert via LINQ to SQL
        using (QueryUserContext dataContext = new QueryUserContext())
        {
            // Approach 1: Via Mapped Stored Procedure
            dataContext.udp_CreateUser(
                userName, email, fullName, mobileNo, city,
                stateName, country, gender, queryText, queryAmount
            );

            // Approach 2: Via Pure LINQ Entity Object
            /*
            QueryUser newUser = new QueryUser
            {
                UserName = userName,
                Email = email,
                FullName = fullName,
                MobileNo = mobileNo,
                City = city,
                StateName = stateName,
                Country = country,
                Gender = gender,
                QueryText = queryText,
                QueryAmount = queryAmount,
                CreatedDate = DateTime.Now,
                IsActive = true
            };
            dataContext.QueryUsers.InsertOnSubmit(newUser);
            dataContext.SubmitChanges();
            */
        }

        // 4. Rebind grid to display the newly created user
        BindUserData();
    }
}
```

---

#### C. UPDATE (Full Row Editing Lifecycle)

Updating requires three synchronized event handlers:

##### 1. Start Editing (`RowEditing`)
```csharp
protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
{
    // Set the row into edit mode
    gvUsers.EditIndex = e.NewEditIndex;
    BindUserData();
}
```

##### 2. Cancel Editing (`RowCancelingEdit`)
```csharp
protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
{
    // Reset edit mode back to normal
    gvUsers.EditIndex = -1;
    BindUserData();
}
```

##### 3. Save Changes (`RowUpdating`)
```csharp
protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
{
    // 1. Retrieve the Primary Key from DataKeys
    int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

    // 2. Locate the row currently being updated
    GridViewRow row = gvUsers.Rows[e.RowIndex];

    // 3. Extract edited values
    string userName   = ((TextBox)row.FindControl("txtEditUserName"))?.Text.Trim();
    string email      = ((TextBox)row.FindControl("txtEditEmail"))?.Text.Trim();
    string fullName   = ((TextBox)row.FindControl("txtEditFullName"))?.Text.Trim();
    string mobileNo   = ((TextBox)row.FindControl("txtEditMobileNo"))?.Text.Trim();
    string city       = ((TextBox)row.FindControl("txtEditCity"))?.Text.Trim();
    string stateName  = ((TextBox)row.FindControl("txtEditStateName"))?.Text.Trim();
    string country    = ((TextBox)row.FindControl("txtEditCountry"))?.Text.Trim();
    string gender     = ((DropDownList)row.FindControl("ddlEditGender"))?.SelectedValue;
    string queryText  = ((TextBox)row.FindControl("txtEditQueryText"))?.Text.Trim();
    bool isActive     = ((CheckBox)row.FindControl("chkEditIsActive")).Checked;

    decimal? queryAmount = null;
    string amountText = ((TextBox)row.FindControl("txtEditQueryAmount"))?.Text.Trim();
    if (decimal.TryParse(amountText, out decimal parsedAmount))
    {
        queryAmount = parsedAmount;
    }

    // 4. Update in Database via LINQ
    using (QueryUserContext dataContext = new QueryUserContext())
    {
        // Approach 1: Via Mapped Stored Procedure
        dataContext.udp_UpdateUser(
            userId, userName, email, fullName, mobileNo, city,
            stateName, country, gender, queryText, isActive, queryAmount
        );

        // Approach 2: Via Pure LINQ Entity Object
        /*
        QueryUser user = dataContext.QueryUsers.SingleOrDefault(u => u.UserId == userId);
        if (user != null)
        {
            user.UserName    = userName;
            user.Email       = email;
            user.FullName    = fullName;
            user.MobileNo    = mobileNo;
            user.City        = city;
            user.StateName   = stateName;
            user.Country     = country;
            user.Gender      = gender;
            user.QueryText   = queryText;
            user.IsActive    = isActive;
            user.QueryAmount = queryAmount;
            
            dataContext.SubmitChanges();
        }
        */
    }

    // 5. Exit edit mode and refresh grid
    gvUsers.EditIndex = -1;
    BindUserData();
}
```

---

#### D. DELETE (Deleting a Record)

```csharp
protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
{
    // 1. Retrieve the Primary Key
    int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

    // 2. Delete using LINQ
    using (QueryUserContext dataContext = new QueryUserContext())
    {
        // Approach 1: Via Mapped Stored Procedure
        dataContext.udp_DeleteUserById(userId);

        // Approach 2: Via Pure LINQ Entity Object
        /*
        QueryUser user = dataContext.QueryUsers.SingleOrDefault(u => u.UserId == userId);
        if (user != null)
        {
            dataContext.QueryUsers.DeleteOnSubmit(user);
            dataContext.SubmitChanges();
        }
        */
    }

    // 3. Refresh grid
    BindUserData();
}
```

* **Client-Side Confirmation Popup:** Always attach `OnClientClick` to your delete button in markup to prevent accidental deletions:
  ```html
  <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete"
      OnClientClick="return confirm('Are you sure you want to permanently delete this user?');" />
  ```

---

#### E. PAGING (Supporting Multi-Page Navigation)

```csharp
protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
{
    gvUsers.PageIndex = e.NewPageIndex;
    BindUserData();
}
```

---

### 3. Stored Procedure LINQ vs. Entity LINQ (Which to Use?)

| Factor | Stored Procedure LINQ (`udp_*`) | Pure Entity LINQ (`SubmitChanges()`) |
| :--- | :--- | :--- |
| **Execution** | Runs pre-compiled SQL procedures on SQL Server. | Dynamically generates `INSERT`, `UPDATE`, `DELETE` SQL queries. |
| **Security & DBA** | Preferred in enterprise environments where DBAs restrict direct table access. | Easier for rapid development without writing SQL scripts. |
| **Performance** | Minimal network overhead, pre-optimized execution plan. | Tracks entity states in memory (ChangeSet) before committing. |
| **Our Project** | We use `udp_GetAllUsers`, `udp_CreateUser`, `udp_UpdateUser`, `udp_DeleteUserById`. | Available natively on `dataContext.QueryUsers`. |


---

## CRUD Operations with GridView & ADO.NET

Before ORMs like LINQ to SQL or Entity Framework existed, **ADO.NET** was the standard way to connect ASP.NET applications to SQL Server. Understanding ADO.NET helps you understand what LINQ to SQL is actually doing behind the scenes.

### 1. Core ADO.NET Architecture & Components

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              ASP.NET GRIDVIEW                               │
├─────────────────────────────────────────────────────────────────────────────┤
│  Events: Page_Load, RowCommand, RowEditing, RowUpdating, RowDeleting        │
└──────────────────────────────────────┬──────────────────────────────────────┘
                                       │
                                       ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                          CORE ADO.NET OBJECTS                               │
├─────────────────────────────────────────────────────────────────────────────┤
│  • SqlConnection     : Establishes a physical connection to SQL Server      │
│  • SqlCommand        : Holds the SQL query or Stored Procedure name         │
│  • SqlParameter      : Maps C# values to SQL parameters (@UserId, etc.)      │
│  • SqlDataAdapter    : Disconnected bridge that fills a DataSet or DataTable│
│  • DataTable         : In-memory table structure bound to gvUsers.DataSource│
│  • ExecuteNonQuery() : Runs INSERT, UPDATE, DELETE (returns affected rows)  │
└──────────────────────────────────────┬──────────────────────────────────────┘
                                       │
                                       ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                        SQL SERVER (AssignmentDB)                            │
└─────────────────────────────────────────────────────────────────────────────┘
```

#### Required Namespaces in Code-Behind
```csharp
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
```

#### Connection String Helper
Using your project's connection string defined in `Web.config`:
```csharp
private string GetConnectionString()
{
    return ConfigurationManager.ConnectionStrings["LinqPractice"] != null
        ? ConfigurationManager.ConnectionStrings["LinqPractice"].ConnectionString
        : ConfigurationManager.ConnectionStrings["AssignmentDBConnectionString"].ConnectionString;
}
```

---

### 2. The CRUD Methods in ADO.NET

#### A. READ (Select / DataAdapter & DataTable)

In ADO.NET, reading data for a GridView typically uses **`SqlDataAdapter`** and **`DataTable`** (disconnected mode):

```csharp
private void BindUserData_AdoNet()
{
    using (SqlConnection con = new SqlConnection(GetConnectionString()))
    {
        using (SqlCommand cmd = new SqlCommand("udp_GetAllUsers", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt); // Executes procedure, fills DataTable, and closes connection

                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }
    }
}
```

---

#### B. CREATE (Insert via Stored Procedure & ExecuteNonQuery)

Triggered from the GridView footer row:

```csharp
protected void gvDetails_RowCommand_AdoNet(object sender, GridViewCommandEventArgs e)
{
    if (e.CommandName.Equals("AddNew"))
    {
        // 1. Locate footer row & extract controls
        GridViewRow footer = gvUsers.FooterRow;

        string userName   = ((TextBox)footer.FindControl("txtNewUserName"))?.Text.Trim();
        string email      = ((TextBox)footer.FindControl("txtNewEmail"))?.Text.Trim();
        string fullName   = ((TextBox)footer.FindControl("txtNewFullName"))?.Text.Trim();
        string mobileNo   = ((TextBox)footer.FindControl("txtNewMobileNo"))?.Text.Trim();
        string city       = ((TextBox)footer.FindControl("txtNewCity"))?.Text.Trim();
        string stateName  = ((TextBox)footer.FindControl("txtNewStateName"))?.Text.Trim();
        string country    = ((TextBox)footer.FindControl("txtNewCountry"))?.Text.Trim();
        string gender     = ((DropDownList)footer.FindControl("ddlNewGender"))?.SelectedValue;
        string queryText  = ((TextBox)footer.FindControl("txtNewQueryText"))?.Text.Trim();

        decimal? queryAmount = null;
        string amountText = ((TextBox)footer.FindControl("txtNewQueryAmount"))?.Text.Trim();
        if (decimal.TryParse(amountText, out decimal parsedAmount))
        {
            queryAmount = parsedAmount;
        }

        // 2. Execute Stored Procedure via ADO.NET
        using (SqlConnection con = new SqlConnection(GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand("udp_CreateUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. Explicitly add parameters with data types and null safety
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 100).Value = (object)userName ?? DBNull.Value;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 200).Value = (object)email ?? DBNull.Value;
                cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 200).Value = (object)fullName ?? DBNull.Value;
                cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 20).Value = (object)mobileNo ?? DBNull.Value;
                cmd.Parameters.Add("@City", SqlDbType.VarChar, 100).Value = (object)city ?? DBNull.Value;
                cmd.Parameters.Add("@StateName", SqlDbType.VarChar, 100).Value = (object)stateName ?? DBNull.Value;
                cmd.Parameters.Add("@Country", SqlDbType.VarChar, 100).Value = (object)country ?? DBNull.Value;
                cmd.Parameters.Add("@Gender", SqlDbType.VarChar, 20).Value = (object)gender ?? DBNull.Value;
                cmd.Parameters.Add("@QueryText", SqlDbType.VarChar, -1).Value = (object)queryText ?? DBNull.Value; // -1 for MAX
                cmd.Parameters.Add("@QueryAmount", SqlDbType.Decimal).Value = (object)queryAmount ?? DBNull.Value;
                cmd.Parameters["@QueryAmount"].Precision = 18;
                cmd.Parameters["@QueryAmount"].Scale = 2;

                con.Open();
                cmd.ExecuteNonQuery(); // Executes INSERT
            }
        }

        // 4. Refresh grid
        BindUserData_AdoNet();
    }
}
```

---

#### C. UPDATE (Full Row Update via Stored Procedure)

Updating in GridView requires the same 3-step event lifecycle (`RowEditing`, `RowCancelingEdit`, `RowUpdating`):

##### 1. Edit Mode & Cancel
```csharp
protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
{
    gvUsers.EditIndex = e.NewEditIndex;
    BindUserData_AdoNet();
}

protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
{
    gvUsers.EditIndex = -1;
    BindUserData_AdoNet();
}
```

##### 2. RowUpdating in ADO.NET
```csharp
protected void gvDetails_RowUpdating_AdoNet(object sender, GridViewUpdateEventArgs e)
{
    int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
    GridViewRow row = gvUsers.Rows[e.RowIndex];

    string userName   = ((TextBox)row.FindControl("txtEditUserName"))?.Text.Trim();
    string email      = ((TextBox)row.FindControl("txtEditEmail"))?.Text.Trim();
    string fullName   = ((TextBox)row.FindControl("txtEditFullName"))?.Text.Trim();
    string mobileNo   = ((TextBox)row.FindControl("txtEditMobileNo"))?.Text.Trim();
    string city       = ((TextBox)row.FindControl("txtEditCity"))?.Text.Trim();
    string stateName  = ((TextBox)row.FindControl("txtEditStateName"))?.Text.Trim();
    string country    = ((TextBox)row.FindControl("txtEditCountry"))?.Text.Trim();
    string gender     = ((DropDownList)row.FindControl("ddlEditGender"))?.SelectedValue;
    string queryText  = ((TextBox)row.FindControl("txtEditQueryText"))?.Text.Trim();
    bool isActive     = ((CheckBox)row.FindControl("chkEditIsActive")).Checked;

    decimal? queryAmount = null;
    string amountText = ((TextBox)row.FindControl("txtEditQueryAmount"))?.Text.Trim();
    if (decimal.TryParse(amountText, out decimal parsedAmount))
    {
        queryAmount = parsedAmount;
    }

    using (SqlConnection con = new SqlConnection(GetConnectionString()))
    {
        using (SqlCommand cmd = new SqlCommand("udp_UpdateUser", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 100).Value = (object)userName ?? DBNull.Value;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 200).Value = (object)email ?? DBNull.Value;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 200).Value = (object)fullName ?? DBNull.Value;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 20).Value = (object)mobileNo ?? DBNull.Value;
            cmd.Parameters.Add("@City", SqlDbType.VarChar, 100).Value = (object)city ?? DBNull.Value;
            cmd.Parameters.Add("@StateName", SqlDbType.VarChar, 100).Value = (object)stateName ?? DBNull.Value;
            cmd.Parameters.Add("@Country", SqlDbType.VarChar, 100).Value = (object)country ?? DBNull.Value;
            cmd.Parameters.Add("@Gender", SqlDbType.VarChar, 20).Value = (object)gender ?? DBNull.Value;
            cmd.Parameters.Add("@QueryText", SqlDbType.VarChar, -1).Value = (object)queryText ?? DBNull.Value;
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;
            cmd.Parameters.Add("@QueryAmount", SqlDbType.Decimal).Value = (object)queryAmount ?? DBNull.Value;
            cmd.Parameters["@QueryAmount"].Precision = 18;
            cmd.Parameters["@QueryAmount"].Scale = 2;

            con.Open();
            cmd.ExecuteNonQuery(); // Executes UPDATE
        }
    }

    gvUsers.EditIndex = -1;
    BindUserData_AdoNet();
}
```

---

#### D. DELETE (ExecuteNonQuery by Primary Key)

```csharp
protected void gvDetails_RowDeleting_AdoNet(object sender, GridViewDeleteEventArgs e)
{
    int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

    using (SqlConnection con = new SqlConnection(GetConnectionString()))
    {
        using (SqlCommand cmd = new SqlCommand("udp_DeleteUserById", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            con.Open();
            cmd.ExecuteNonQuery(); // Executes DELETE
        }
    }

    BindUserData_AdoNet();
}
```

---

### 3. Critical ADO.NET Gotcha: `DBNull.Value` vs. C# `null`

In LINQ to SQL, passing a C# `null` string automatically converts to SQL `NULL`.
In **ADO.NET**, if you write:
```csharp
// THIS THROWS AN EXCEPTION IN ADO.NET!
cmd.Parameters.AddWithValue("@City", null);
```
SQL Server treats this as an **omitted parameter**, which throws an `SqlException`: *"Procedure expects parameter '@City', which was not supplied."*

**The Rule in ADO.NET:**
You must pass `DBNull.Value` whenever a value is null:
```csharp
cmd.Parameters.Add("@City", SqlDbType.VarChar, 100).Value = (object)city ?? DBNull.Value;
```

---

### 4. Head-to-Head Comparison: ADO.NET vs. LINQ to SQL

| Feature | ADO.NET | LINQ to SQL |
| :--- | :--- | :--- |
| **Boilerplate Code** | High (50+ lines to manage connections, commands, and parameters). | Low (1 line: `dataContext.udp_UpdateUser(...)`). |
| **Type Safety** | Low (Parameter names and procedure names are strings; typos caught only at runtime). | High (Method names and parameter types are checked at compile time). |
| **Null Handling** | Manual (`DBNull.Value` checks required on every parameter). | Automatic (C# `null` maps seamlessly to SQL `NULL`). |
| **Connection Lifecycle** | Explicit (`con.Open()`, `con.Close()`, `using`). | Automatic (handled by `DataContext.Dispose()`). |
| **Performance** | Maximum raw speed with lowest memory overhead. | Negligible overhead (translates directly to ADO.NET commands internally). |
| **Maintainability** | Lower (changing a column requires updating SQL strings across files). | Higher (updating `.dbml` auto-generates updated C# signatures). |
