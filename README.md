
# FATMS (FA Training Management System) - Web API

## This application is used for Fresher Academy to support creation and management classes easily.
Features as follow:
- Create and manage the syllabus's content.
- Create and manage training programs.
- Plan and manage classes.
- Report classes by week, quarter.

## Technology Stack
The FATMS (FA Training Management System) - Web API is built using the following technologies:

- Programming Language: C#
- Framework: ASP.NET Core
- Database: SQL Server
- ORM: Entity Framework Core
- Authentication: JWT (JSON Web Token)
- Documentation: Swagger UI
- Testing: xUnit, Postman

## To run this project locally from Git, you can follow these steps:

- Install Git on your local machine if you haven't already done so.
- Open a command prompt or terminal window and navigate to the directory where you want to clone the repository.
- Run the following command to clone the repository to your local machine:
```
git clone https://github.com/trinity-fptu/FATMS.git
```
- Open the cloned project in Visual Studio.
- Update the connection string in the appsettings.json file to match your SQL Server configuration.
- Open the Package Manager Console and run the following commands:
```
Update-Database
```
- Set WebAPI as startup project.
- Build and run the application using Visual Studio.
