# Installation Guide

Follow these steps to successfully set up and run the web application:

## Clone Repository
Open Windows PowerShell and write down this commands one by one:
```
git clone https://github.com/NobleChad/TestProject
```
```
cd TestProject
```
## Install Dependencies
Yes, we want to run this command again.
```
cd TestProject
```
```
dotnet restore
```
## Apply Migrations
```
dotnet ef database update
```
## Run the Application
Open Visual Studio and run the app. The default path should be C:\Users\USER_NAME\TestProject. Enjoy!

# Information about the web application
## Main functions
There are 2 roles on this site - User and Admin.
User - can view the Products page, but cannot change anything in Products (Even if the User clicks on the link to create a table, he will see Access Denied).
Admin - can do everything User can, but he can also edit the Products table.

## Translations
The website supports both English and Ukrainian translations. If you notice any missing translations, feel free to contribute or notify me.

## MiddleWare
This middleware redirects to NotFound.cshtml if the website encounters 404 exception.

## Swagger page
This page contains API documentation. API uses JWT tokens system, so in order to use API you need an account and then fill your info in /api/token. Once you get a token you need to provide it to methods that you want to use. Alternativly you can login on the website, then you dont need to provide token.

## Home/Privacy pages
Those pages do not have any particular use, but at least they are translated.

## Product page
This page contains a table from database that Users can see and Admins can change. Also it has a Currency component that is added to the page and it calculates price in different currencies.

Create - on this page you can create a new row in the table.
Edit - on this page you can edit elements of the respective row.
Delete - on this page you can delete row.

## Register page
This page contains default register form and default checks for fields. After registering you will NEED to confirm your email to login. 

## Login page
This page contains default login form to fill.

## Account/Manage page
This page contains general information about your profile that you can edit/delete/download.
