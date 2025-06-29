# 🎮 GAMESTORE

_Empowering gamers with seamless, secure experiences._

![last-commit](https://img.shields.io/github/last-commit/weldonkipchirchir/GameStore?style=flat&logo=git&logoColor=white&color=0080ff)
![repo-top-language](https://img.shields.io/github/languages/top/weldonkipchirchir/GameStore?style=flat&color=0080ff)
![repo-language-count](https://img.shields.io/github/languages/count/weldonkipchirchir/GameStore?style=flat&color=0080ff)

## 📚 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Usage](#usage)
  - [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)

---

## 🧩 Overview

**GameStore** is a powerful backend tool built for managing game-related data with a secure and scalable API. It's designed for developers building game apps or digital marketplaces.

---

## 🚀 Features

- **Robust API Framework:** Built on .NET 9.0 using RESTful principles.
- **JWT Authentication:** Secure endpoints with token-based auth.
- **PostgreSQL Integration:** Reliable data storage and management.
- **DTO Support:** Clean separation between data models and API contracts.
- **CORS Configuration:** Seamless integration with frontends from any origin.
- **Rate Limiting Middleware:** Protects the API from abuse.
- **Centralized Error Handling:** Enhanced debugging and logging.

---

## 🛠 Tech Stack

- **Language:** C#
- **Framework:** .NET 9.0
- **Database:** PostgreSQL
- **Authentication:** JWT
- **Package Manager:** NuGet

---

## ⚙ Getting Started

### Prerequisites

- [.NET SDK 9.0+](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository**

```bash
git clone https://github.com/weldonkipchirchir/GameStore
cd GameStore

    Restore NuGet packages

dotnet restore

    Apply database migrations

dotnet ef database update

Usage

Run the application with:

dotnet run

Access the API at: http://localhost:5000
Testing

Replace {test_framework} below with your actual test library (e.g., xUnit, NUnit):

dotnet test

🤝 Contributing

Contributions are welcome! Feel free to fork this repo and submit a pull request.
📄 License

This project is licensed under the MIT License.


```
