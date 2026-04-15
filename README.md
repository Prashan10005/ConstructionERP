Construction ERP System
     A modular and scalable Enterprise Resource Planning (ERP) system built using **ASP.NET Core Web API** and **MVC architecture**, designed to manage construction operations efficiently with secure authentication and role-based access control.

Features
  Secure authentication using BCrypt password hashing
  Role-Based Access Control (Admin, ProjectManager and FieldStaff)
  Modular architecture (Controller → Service → Repository)
  User Management for Admin, Project Management and FieldStaff assigning for Project Manager, Material Management for FieldStaff
  Soft User delete implementation (IsActive flag)
  Audit tracking for system activities (CreatedAt and LastLoginAt)
  AJAX-based dynamic UI with real-time validation
  Entity Framework Core (Code-First) with migrations
  RESTful API design for scalability

Tech Stack
  Front End - HTML5, CSS3, JavaScript, jQuery, Bootstrap
  Back End - C#, ASP.NET Core Web API, Entity Framework Core, LINQ, Async
  Database - SQL Server
  Tools - Visual Studio, GitHub ( for version controlling), Postman (API testing)

Project Architecture
  Presentation Layer (MVC / API Controllers)
  Service Layer (Business Logic)
  Repository Layer (Data Access)
  Database (SQL Server)

Authentication & Authorization
  Passwords are securely hashed using **BCrypt**
  Role-based access implemented for : Admin, ProjectManager, FieldStaff
  Session-based authentication (non-JWT)

Key Functional Modules
  Admin 
      User Creation and User profile updation (Password has to be updated if only the user forgotten the old password)
  ProjectManager - Future module
      Creation of Construction Project, Project Status Change (Open, Completed and Cancelled), Assign FieldStaff for Projects (Only for the Projects with Open Status)
  Field Staff - Future Module
      Updating Cost Materials for Projects (Only for the projects with open status)



  Future Enhancement of System - Onmind
      Project Budget Creation
      Cost tracking
      Invoice tracking 
      Invoice Approval process
      
