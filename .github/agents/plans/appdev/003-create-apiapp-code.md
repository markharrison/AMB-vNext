## Plan: API App Code Creation & Integration

Develop a comprehensive API layer to support all app functionality shown in the legacy screenshots, ensuring all data access is via APIs. Provide Swagger documentation for easy API exploration and testing. Support both development (mocked in-memory data) and production (real database) modes.

### Steps

- Review all screenshots in the `/Legacy-Screenshots/` folder to identify all required features and data flows.
- Define API endpoints for each feature, specifying request/response schemas and error handling.
- Implement data-access APIs:
  - Use in-memory data stores for development mode.
  - Integrate with the production database as defined in the `/Database-Schema/database_schema.sql` file for production mode.
- Set up environment/configuration switching between development and production modes.
- Generate and integrate Swagger/OpenAPI documentation, ensuring all endpoints are documented and testable via Swagger UI.
- Ensure app code consume the data-access APIs, removing any direct database access.
- Test APIs
- Build application into `/src`
