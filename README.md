# Information about the web application

# Main functions
There are 2 roles on this site - User and Admin.
User - can view the Products/API/Swagger pages, but cannot change anything in Products (Even if the User clicks on the link to create a table, he will see Access Denied).
Admin - can do everything User can, but he can also edit the Products table.

This website also has English and Ukrainian translations(I tried to translate everything but I surely missed something).

# MiddleWare
This middleware redirects to NotFound.cshtml if the website encounters 404 exception.

# Swagger page
This page contains default swagger documentation.

# Home/Privacy/API pages
Those pages do not have any particular use, but at least they are translated.

# Product page
This page contains a table from database that Users can see and Admins can change. Also it has a Currency component that is added to the page and it calculates price in different prices.

Create - on this page you can create a new row in the table.
Edit - on this page you can edit elements of the respective row.
Delete - on this page you can delete row.

# Register page
This page contains default register form and default checks for fields. After registering you will NEED to confirm your email to login. 

# Login page
This page contains default login form to fill.

# Account/Manage page
This page contains general information about your profile that you can edit/delete/download.
