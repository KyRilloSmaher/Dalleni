# Dalleni Q&A API Integration Guide

This document provides a comprehensive overview of the available API endpoints for the Dalleni Q&A platform. 

**Base URL**: `https://localhost/api`  
**Authentication**: All protected endpoints require a Bearer Token in the `Authorization` header.

---

## 🔐 Authentication Module

| Method | Endpoint | Request Body / Parameters | Description | Auth |
| :--- | :--- | :--- | :--- | :--- |
| `POST` | `/auth/sign-up` | **FormBody**: `FirstName`, `LastName`, `UserName`, `Email`, `Password`, `Bio`, `ProfileImage` (file) | Register a new user | ❌ |
| `POST` | `/auth/login` | **JSON**: `{ "userNameOrEmail": "", "password": "" }` | Login and get tokens | ❌ |
| `POST` | `/auth/logout` | None | Revoke current tokens | ✅ |
| `POST` | `/auth/refresh-token` | **JSON**: `{ "token": "", "refreshToken": "" }` | Refresh access token | ✅ |
| `POST` | `/auth/change-password` | **JSON**: `{ "currentPassword": "", "newPassword": "", "confirmPassword": "" }` | Change password | ✅ |
| `POST` | `/auth/send-reset-code` | **JSON**: `{ "email": "" }` | Request reset code | ❌ |
| `POST` | `/auth/resend-reset-code` | **JSON**: `{ "email": "" }` | Resend reset code | ❌ |
| `POST` | `/auth/confirm-reset-password-code` | **JSON**: `{ "email": "", "code": "" }` | Verify reset code | ❌ |
| `POST` | `/auth/reset-password` | **JSON**: `{ "email": "", "code": "", "newPassword": "" }` | Set new password | ❌ |
| `GET` | `/auth/confirm-email` | **Query**: `userId`, `code` | Verify email via registration link | ❌ |
| `GET` | `/auth/resend-confirmation-email` | **Query**: `email` | Resend confirmation email | ❌ |
| `GET` | `/auth/google-signin` | None | Google Sign-in (Redirects) | ❌ |
| `GET` | `/auth/google-login` | None | Google Login (Redirects) | ❌ |

---

## 🙋‍♂️ Users Module

| Method | Endpoint | Request Body / Parameters | Description | Auth |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/users/me` | None | Get current profile | ✅ |
| `GET` | `/users/{id}` | `id` (Guid) | Get user profile by ID | ❌ |
| `GET` | `/users/by-email/{email}` | `email` (string) | Get user profile by Email | ❌ |
| `GET` | `/users/search` | **Query**: `query` (string) | Search users by name/username | ❌ |
| `PUT` | `/users/update-profile` | **JSON**: `{ "firstName": "", "lastName": "", "bio": "", "phoneNumber": "" }` | Update profile info | ✅ |
| `PUT` | `/users/update-profile-image` | **FormBody**: `ProfileImage` (file) | Upload new image | ✅ |
| `POST` | `/users/restore` | **JSON**: `{ "email": "", "password": "" }` | Restore a soft-deleted account | ❌ |
| `DELETE` | `/users/delete/{id}` | `id` (Guid) | Soft-delete a user account | ✅ |
| `GET` | `/users/top-users` | **Query**: `count` (int) | Get top users by reputation | ❌ |
| `GET` | `/users/top-contributors` | **Query**: `count` (int) | Get top users by answers | ❌ |
| `GET` | `/users/stats/{id}` | `id` (Guid) | Get detailed user stats | ❌ |

---

## ❓ Questions Module

| Method | Endpoint | Request Body / Parameters | Description | Auth |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/questions` | **Query**: `pageNumber`, `pageSize` | Get paged questions | ❌ |
| `GET` | `/questions/{id}` | `id` (Guid) | Get detailed question view | ❌ |
| `POST` | `/questions` | **JSON**: `{ "title": "", "content": "", "categoryId": "guid", "tags": ["tag1", "tag2"] }` | Create question | ✅ |
| `PUT` | `/questions/{id}` | **JSON**: `{ "title": "", "content": "", "categoryId": "guid" }` | Update question | ✅ |
| `DELETE` | `/questions/{id}` | `id` (Guid) | Delete a question | ✅ |
| `GET` | `/questions/tag/{id}` | `id` (TagId), **Query**: `pageNumber`, `pageSize` | Get questions by tag | ❌ |
| `GET` | `/questions/category/{id}` | `id` (CategoryId) | Get questions by category | ❌ |
| `GET` | `/questions/search` | **Query**: `query`, `pageNumber`, `pageSize` | Search questions | ❌ |
| `GET` | `/questions/related` | **Query**: `id` (Guid), `count` (int) | Get related questions | ❌ |
| `GET` | `/questions/similars` | **Query**: `Question` (string) | Get similar questions (AI-based) | ❌ |
| `POST` | `/questions/close/{id}` | `id` (Guid) | Close a question | ✅ |
| `POST` | `/questions/reopen/{id}` | `id` (Guid) | Reopen a question | ✅ |
| `POST` | `/questions/accept-answer/{id}` | `id` (AnswerId), **Query**: `questionId` | Mark answer as accepted | ✅ |

---

## 💬 Answers Module

| Method | Endpoint | Request Body / Parameters | Description | Auth |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/answers/question/{id}` | `id` (Guid) | Get answers for a question | ❌ |
| `POST` | `/answers` | **JSON**: `{ "content": "", "questionId": "guid" }` | Submit an answer | ✅ |
| `PUT` | `/answers/{id}` | **JSON**: `{ "content": "" }` | Edit an answer | ✅ |
| `DELETE` | `/answers/{id}` | `id` (Guid) | Delete an answer | ✅ |

---

## 👍 Voting Module

| Method | Endpoint | Request Body / Parameters | Description | Auth |
| :--- | :--- | :--- | :--- | :--- |
| `POST` | `/votes/question/{id}` | `id` (Guid), **Query**: `type` (0=Up, 1=Down) | Vote on a question | ✅ |
| `POST` | `/votes/answer/{id}` | `id` (Guid), **Query**: `type` (0=Up, 1=Down) | Vote on an answer | ✅ |

---

## 🔖 Saved Questions Module

| Method | Endpoint | Request Body / Parameters | Description | Auth |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/user/saved-questions/by-user-Id` | None | Get current user's saved questions | ✅ |
| `POST` | `/user/saved-questions/add` | **JSON**: `{ "questionId": "guid", "userId": "guid" }` | Save a question | ✅ |
| `POST` | `/user/saved-questions/remove/{id}` | `id` (Guid) | Unsave a question | ✅ |

---

## 🏷️ Tags & Categories

| Method | Endpoint | Request Body / Parameters | Description | Auth |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/tags` | **Query**: `pageNumber`, `pageSize` | Get top/popular tags | ❌ |
| `GET` | `/categories` | None | Get all question categories | ❌ |

---

## 💡 Integration Tips for Mobile Developers

> [!IMPORTANT]
> **Pagination**: List endpoints (like `/questions` or `/tags`) support `pageNumber` and `pageSize` as query parameters.

> [!NOTE]
> **Soft Deletion**: Most delete operations (Users, Questions, Answers) are "Soft Deletes". Data remains in the database but is hidden from regular queries.

> [!WARNING]
> **Token Expiry**: Access tokens are short-lived. Implement logic to use the `refresh-token` endpoint if a `401 Unauthorized` response is received.

> [!TIP]
> **Route Consistency**: All routes are prefixed with `/api`. Ensure your base client configuration includes this prefix.
