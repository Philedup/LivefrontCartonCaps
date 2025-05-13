# Carton Caps Referral Feature – Mock API

This project is a mock REST API built in .NET Core to support a new referral feature for the **Carton Caps** app. The API simulates backend functionality for generating and sharing referral links, tracking referral history, and onboarding new users. It uses realistic mock data and clean architecture to demonstrate best practices in service design, testing, and documentation.

> Built for Livefront as part of a design and engineering evaluation.

---

## Project Goals

- Allow users to generate referral links with shareable message templates
- Display a user’s referral history and status
- Ensure the API can be tested and integrated by front-end developers
- Simulate realistic user and referral behavior using mock data
- Provide full Swagger/OpenAPI documentation for consumption
- Demonstrate async patterns and modular architecture

---

## Tech Stack

- [.NET 8](https://dotnet.microsoft.com/)
- ASP.NET Core Web API
- Swagger / Swashbuckle for OpenAPI documentation
- xUnit + Moq for unit testing
- Fully async service/repository pattern
- macOS-compatible; build, run, and test via .NET CLI

---

## API Summary

- `GET /api/referrals/page` – Get referral code, shareable link, and message templates
- `GET /api/referrals/history` – View referral history (resolved user names)
- `POST /api/referrals/track` – [Reserved] Track referral link clicks (future)
- `POST /api/registration` – Register new user and redeem referral

---

## Deliverable 1: REST API Specification

| Method | Endpoint                  | Description                                          |
|--------|---------------------------|------------------------------------------------------|
| GET    | `/api/referrals/page`     | Returns referral code, link, and share content       |
| GET    | `/api/referrals/history`  | Lists users referred by the current user             |
| POST   | `/api/referrals/track`    | Reserved for click tracking                          |
| POST   | `/api/registration`       | Registers a new user and optionally redeems referral |

- Includes XML summaries for models and endpoints
- Status code examples: `200`, `400`, `404`, `409`, `500`
- Reusable response wrapper: `ApiResponse<T>`
- Custom examples via `SwaggerResponseExample`

---

## Deliverable 2: Mock API Service

- Fully async mock services (no real database needed)
- Config-driven referral link base (from `appsettings.json`)
- Realistic test data for users and referrals
- Unit-tested endpoints with meaningful assertions
- Designed to swap in real repositories later if needed
- ✅ Over 90% unit test coverage across services and controllers

---

## Extra Work

Although not required, additional endpoints and tooling were implemented to demonstrate integration of the referral logic:

- **`POST /api/registration`**  
  - Simulates user creation and referral redemption  
  - Registers with a random referral code
  - Supports optional referral attribution and linking

- **Swagger Examples**  
  - Preconfigured Swagger examples for each response type, including failures

- **Deep Linking Simulation**  
  - The API embeds referral codes as query parameters in generated links (e.g., `?referral_code=PLP1013&method=sms`)
  - Assumes deferred deep link behavior is handled externally by a service like Firebase Dynamic Links or Branch.io
  - The app would extract and redeem the referral code during onboarding

---

## Running the App

```bash
dotnet run --project LivefrontCartonCaps
```

Then open:

```
https://localhost:7111/swagger
```

Swagger UI includes:
- Full documentation for all endpoints
- Example requests and responses
- Typed error messages

---

## Running Unit Tests

```bash
dotnet test LivefrontCartonCaps.Test
```

Tests cover:

- Referral link generation
- Validation and error scenarios
- Referral history with missing users
- Registration logic with and without referral codes

---

## Project Structure

```
LivefrontCartonCaps/
├── Controllers/                # API endpoints
├── Entities/                  # Core entities
├── Models/                    # DTOs and response wrappers
│   ├── ReferralsPage/
│   ├── RegistrationPage/
│   └── Shared/
├── MockData/                  # Mock in-memory repositories
├── Services/                  # Core business logic
├── SwaggerExamples/           # Swagger example generators
│   ├── ReferralsPage/
│   ├── RegistrationPage/
├── appsettings.json           # Config for referral link base
└── LivefrontCartonCaps.Test/  # Unit tests with xUnit + Moq
```

---

## Notes

- This is a mock API only. No database or authentication is implemented.
- Referral link tracking is simulated. Deferred deep link support is assumed to be external.
- All data resets on each restart.
- `POST /api/referrals/track` is reserved for future analytics or fraud prevention expansion.

---

## Contact

Created by [Phil Phan](mailto:me@philphan.com)  
Livefront Project Candidate
