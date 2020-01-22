# DotNetCoreAuthIntro
This solution is an intro project exploring how .NET Core 2.2 handles role and claim-based authentication with policies. It was scaffolded with the .NET Core 2.2 MVC web app with Individual authentication. There are two MVC web projects in the solution: one for the roles and one for the claims. Both point to the same local DB on the machine, so whatever users Claims creates will exist in the Roles demo.

Both projects have added password options in the Startup.cs as well as policy declarations. Program.cs seeds an admin user for both projects if it does not exist. Claims includes a DateStarted when it seeds the data.

The Roles project displays straight-forward use of the Authorize token to protect functions. Three roles are defined: Admin, Manager, and User.

The Claims project has a custom attribute for years worked by a user, which becomes a claim describing the user. If there is a user created without a DateStarted, the program will currently crash. If their DateStarted does not satisfy a policy enforced on a particular resource, they will be notified that their access is denied.
