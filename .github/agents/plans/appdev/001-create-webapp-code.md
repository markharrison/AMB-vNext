## Plan: Create ASP.NET Razor Pages Web App (Modern UI)

Develop a new ASP.NET Razor Pages web application in C# that replicates the functionality shown in the legacy screenshots, with a clean, modern UI. The app should target .NET 10 (LTS) and provide intuitive navigation and controls for all core database operations (add, read, filter data).

### Steps

- Review all screenshots in the `/Legacy-Screenshots/` folder to identify required features, workflows, and data entities.
- Design a database schema in the `/Database-Schema/database_schema.sql` file to support all identified features.
- Scaffold a new ASP.NET Razor Pages project targeting .NET 10 (LTS) with a clean, modular folder structure.
- Implement Razor Pages for each major feature, ensuring the index page provides clear navigation (buttons/links) for all CRUD and filter operations.
- Apply a modern UI theme focusing on usability, clarity, and responsive design.
- Use files in `/Style-Guide/*` to influence the design, look & feel.
- Integrate database access using APIs.
- Test all user flows to ensure feature parity with legacy screenshots and smooth, modern UX.
- Build application into `/src`