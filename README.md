# CustomAuth
Prerequisites:
Visual Studio: Make sure you have Visual Studio installed (Community, Professional, or Enterprise edition).
MySQL Server (XAMPP): You need MySQL running via XAMPP. Make sure that MySQL is running and you have access to phpMyAdmin or can connect to the MySQL database.
MySQL Connector for .NET: You need to install the MySQL.Data NuGet package to connect ASP.NET MVC to MySQL.
Step-by-Step Guide
# Step 1: Create a New ASP.NET MVC Project
Open Visual Studio and create a new ASP.NET Web Application project.
Select MVC template and click on Create.
# Step 2: Install MySQL.Data NuGet Package
In NuGet Package Manager, install MySql.Data to allow connection to MySQL.
Right-click on the Project in the Solution Explorer.
Select Manage NuGet Packages.
Search for MySql.Data and install it.
# Step 3: Create the MySQL Database and Table
Using phpMyAdmin (accessible via XAMPP), create a MySQL database and a Users table.

Open phpMyAdmin in your browser (http://localhost/phpmyadmin).

Create a new database (e.g., TestDB).

Create a Users table with columns like Id, Username, Password, and Email:
# Step 5: Create a User Model
Right-click the Models folder and create a User model class.

# Step 6: Create a Database Context Class
In the Models folder, create a DbContext class to handle MySQL interactions.

# Step 7: Create the Login Controller
Right-click the Controllers folder and add a new AccountController to handle login logic.

# Step 8: Create the Login View
Right-click on the Views folder under Account and create a new Login.cshtml view.

# Step 9: Set Up Authentication in Web.config
Ensure authentication is set up to use forms authentication.

In the Web.config file, set the authentication mode to Forms:

# Step 10: Testing the Application
Run the application.
Navigate to /Account/Login and enter the credentials of a user you inserted into the Users table.
Upon successful login, the user should be redirected to the Home page.
If the username or password is incorrect, an error message will display.
