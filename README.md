# ZerdaLiveApi 🎬

🚀 **ZerdaLiveApi** is a full-featured **ASP.NET Core Web API** backend designed to provide movie, series, and live channel data to mobile and web applications. It supports categorized media content and includes a simple admin management system.

---

## 📌 Features

- 🔗 Provides ready-to-use links for movies, TV shows, and live channels.
- 🎞️ Supports multiple content categories (Action, Drama, Comedy, etc.).
- 📱 Built to integrate smoothly with mobile and web frontends.
- 🔐 Includes a basic system for managing application administrators.
- 📩 Sends **push notifications** to mobile/web apps via **Firebase Cloud Messaging (FCM)**.
- ⚡ Built with modern technologies for high performance and reliability.

---

## 🔔 Firebase Notifications

The API is integrated with **Firebase Cloud Messaging (FCM)** to send push notifications to connected client apps.

> ⚠️ **Important:** To enable FCM notifications, you must provide your own `PrivateKey.json` file from your Firebase project and **DO NOT upload it to the repository**.

- Add your Firebase service account key (typically named `PrivateKey.json`) to the root of the project or a secure path.
- Keep this file secure and exclude it using `.gitignore`.

---

## 🧰 Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- Firebase Admin SDK (.NET)
- Swagger / OpenAPI
- LINQ
- JSON Serialization
- Git for version control

---

## 🚀 Getting Started

```bash
# Clone the project
git clone https://github.com/MuhammdShYaseen/ZerdaLiveApi.git
cd ZerdaLiveApi

# Run the project
dotnet run
