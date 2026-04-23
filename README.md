# 🌍 Dalleni Community - Q&A Platform

[![Backend](https://img.shields.io/badge/Backend-.NET%2010-blue.svg)](https://dotnet.microsoft.com/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20%2F%20DDD-orange.svg)](#architecture)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-red.svg)](https://www.microsoft.com/en-us/sql-server)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

Dalleni (دلّني)  is a scalable, high-performance platform that helps users navigate real-world services through community knowledge + verified information + intelligent search. it is built with a focus on clean architecture, domain-driven design (DDD), and modern engineering best practices. It provides a robust backend ecosystem for knowledge sharing, community engagement, and expert discussions.

---

# 🌍 Vision
>“Ask less. Find faster. Trust more.”

Dalleni bridges the gap between:
   * Real user experiences
   * Official service information
   * Intelligent discovery
# 🎯 Problem
Users struggle with:
   * ❌ Finding accurate service information
   * ❌ Conflicting sources
   * ❌ Repeating trial-and-error processes
# 💡 Solution
Dalleni provides a unified platform combining:
   * ✔ Community-driven Q&A
   * ✔ Verified official data
   * ✔ Smart ranking & trust system
   * ✔ AI-powered search & assistance


## 🏗️ Architecture

The project follows the **Clean Architecture** (Onion Architecture) pattern, ensuring that the core business logic remains independent of external frameworks, databases, and UI concerns.

### 🧱 Project Layers

- **`Dalleni.Domin`**: The core domain layer. Features structured entities, domain events, and pure business logic.
- **`Dalleni.Application`**: Orchestrates use cases using the **CQRS** pattern. Leverages **MediatR** for decoupling and **FluentValidation** for strict request integrity.
- **`Dalleni.Infrastructure`**: Implements data persistence, external integrations, and cross-cutting concerns like logging and security.
- **`Dalleni.API`**: The public-facing REST API providing a clean interface for mobile and web clients.

### 🛠️ Advanced Engineering Patterns

- **Soft Delete Mechanism**: Implemented globally via EF Core Query Filters, ensuring data safety while maintaining high performance.
- **Automated Auditing**: Built-in tracking for `CreatedAt` and `UpdatedAt` timestamps across all domain entities.
- **Domain-Driven Events**: Internal event dispatcher for horizontal decoupling between modules (e.g., updating reputation when a vote occurs).
- **Unit of Work & Repository Pattern**: Clean abstraction over the database layer for easier testing and maintenance.

---

## 🚀 Tech Stack

| Category | Technology |
| :--- | :--- |
| **Framework** | .NET 10 / ASP.NET Core |
| **Data Access** | Entity Framework Core (SQL Server) |
| **Design Patterns** | CQRS, Repository Pattern, Unit of Work, MediatR, Domain Events |
| **Authentication** | ASP.NET Core Identity, JWT Bearer, Google OAuth |
| **Validation** | FluentValidation (Auto-Validation) |
| **Mapping** | AutoMapper |
| **Search** | Azure Cognitive Search |
| **Media** | Cloudinary & Azure Blob Storage |
| **Logging** | Serilog (File & Console) |
| **API Documentation**| Swagger / OpenAPI |
| **Rate Limiting** | Fixed Window Limiter |

---

## 🛠️ Key Features

### 🔐 Multi-Channel Authentication
- Robust Auth system with Support for **JWT Tokens** & **Refresh Tokens**.
- Social Login integration (Google).
- OTP-based password recovery and email confirmation.
- Security stamp-based token revocation for enhanced security.

### 🙋‍♂️ User Management
- Detailed profiles with reputation tracking.
- Contributor statistics and achievements.
- Flexible profile updates with cloud-based image uploading.

### ❓ Advanced Q&A Engine
- Complex question/answer relationships with threaded comments.
- **Tagging System**: Organize content with hierarchical tags.
- **Categorization**: Multi-level categories for better discoverability.
- **Voting System**: Upvote/Downvote logic for quality control.
- **Accepted Answers**: Mark the best solutions.

### 🔍 Intelligence & Search
- Full-text search powered by **Azure Search**.
- AI-driven "Similar Questions" suggestions (Roadmap).
- Related questions discovery.

---

## 📦 Project Structure

```text
e:/Dalleni/
├── Dalleni.API/             # Entry point, Controllers, Middlewares
├── Dalleni.Application/     # CQRS Handlers, DTOs, Mappers, Validators
├── Dalleni.Domin/           # Domain Entities, Interfaces, Enums
├── Dalleni.Infrasstructure/ # EF Core, External Services, Migrations
├── api_documentation.md      # Detailed API Endpoints Guide
└── seed_initial_data.sql     # SQL script for initial database setup
```

---

## ⚙️ Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (or VS Code / Rider)

### Installation & Setup

1. **Clone the Repository**
   ```bash
   git clone  https://github.com/KyRilloSmaher/Dalleni.git
   cd Dalleni
   ```

2. **Configure App Settings**
   Update the connection strings and external service keys in `Dalleni.API/appsettings.json`:
   - `ConnectionStrings:Local`
   - `JwtSettings`
   - `Cloudinary` (if using)
   - `EmailSettings`

3. **Apply Migrations**
   ```bash
   dotnet ef database update --project Dalleni.Infrasstructure --startup-project Dalleni.API
   ```

4. **Seed Initial Data**
   Execute the `seed_initial_data.sql` script against your SQL Server database to populate categories, tags, and sample users.

5. **Run the Application**
   ```bash
   dotnet run --project Dalleni.API
   ```

---

## 📖 API Documentation

The API comes with built-in Swagger documentation. Once the application is running, you can access it at:
`https://localhost:5001/swagger` (or your configured port).

For a quick reference of all endpoints, refer to [api_documentation.md](file:///e:/Dalleni/api_documentation.md).

---

## 🔮 Roadmap (Future Features)

### 🤖 AI Chat Assistant
- **Conversational Expert**: AI assistant powered by platform knowledge to provide instant answers.
- **Linguistic Support**: Full support for Arabic language and regional dialects.
- **Service Guidance**: Step-by-step interactive flows for organizational procedures.

### 🏢 Official Verified Accounts
- **Institutional Profiles**: Dedicated spaces for Government and Organizations.
- **Badge System**: High-trust verified status icons for official entities.
- **Trusted Requirements**: Centralized hub for official documentation and procedure requirements.

### 🗺️ Location-Based Services (LBS)
- **Local Discovery**: Find the nearest service providers and official offices.
- **Geospatial Processing**: Map integration and geo-filtered Q&A for local communities.

### 📱 Performance & Accessibility
- **Offline Mode**: Robust caching for access in low-connectivity areas.
- **Smart Notifications**: Real-time, personalized alerts for service changes and mentions.

---

## 👨‍💻 Contributing

1. Fork the project.
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`).
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`).
4. Push to the Branch (`git push origin feature/AmazingFeature`).
5. Open a Pull Request.

---

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.

---

**Developed with ❤️ by the Dalleni Engineering Team(it Just Me Kyrillos Maher)**
