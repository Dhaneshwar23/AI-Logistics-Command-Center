# AI Logistics Command Center

## Overview

AI Logistics Command Center is a production-ready backend system built using ASP.NET Core 8 following Clean Architecture principles.

The project simulates a real-world logistics platform capable of managing customers, shipments and shipment tracking while implementing enterprise authentication, authorization, cloud deployment and DevOps practices.

The primary goal of this project is to demonstrate backend engineering skills expected from a mid-level .NET Backend Developer.

---

# Technology Stack

## Backend

- ASP.NET Core 8 Web API
- C#
- Entity Framework Core
- SQL Server
- Azure SQL Database

## Architecture

- Clean Architecture
- Repository Pattern
- Service Layer
- Dependency Injection

## Security

- JWT Authentication
- Refresh Token Authentication
- Role-Based Authorization
- BCrypt Password Hashing

## Cloud

- Azure App Service
- Azure SQL Database

## DevOps

- GitHub Actions CI
- GitHub Actions CD

## Logging

- Serilog
- Correlation ID Middleware
- Request / Response Logging
- Global Exception Middleware

---

# Project Structure

AILogistics.Api

- Controllers
- Middleware
- Configuration
- Dependency Injection
- Swagger

AILogistics.Application

- Services
- DTOs
- Interfaces
- Business Logic

AILogistics.Domain

- Entities
- Enums
- Domain Models

AILogistics.Infrastructure

- EF Core
- DbContext
- Repositories
- Authentication
- Database Configuration

---

# Implemented Modules

## Customer Management

- Create Customer
- Update Customer
- Delete Customer
- Get Customer
- List Customers

---

## Shipment Management

- Shipment CRUD
- Shipment Status
- Customer Association

---

## Tracking Events

- Shipment Tracking
- Tracking History
- Shipment Status Updates

---

# Authentication Flow

User Login

↓

Validate Credentials

↓

Verify Password (BCrypt)

↓

Generate JWT Access Token

↓

Generate Secure Refresh Token

↓

Persist Refresh Token in Database

↓

Return Access Token + Refresh Token

---

# Refresh Token Flow

Client sends Refresh Token

↓

Lookup Refresh Token

↓

Validate

- Exists
- Not Expired
- Not Revoked

↓

Generate New JWT

↓

Generate New Refresh Token

↓

Revoke Old Refresh Token

↓

Persist New Refresh Token

↓

Return New Tokens

---

# Logout Flow

Client sends Refresh Token

↓

Locate Refresh Token

↓

Mark Token as Revoked

↓

Save Changes

↓

Return Success

---

# Authorization

Supported Roles

- Admin
- Manager
- Customer

Role-Based Authorization

Example

- Admin endpoints
- Manager endpoints
- Customer endpoints

Unauthorized users receive

403 Forbidden

---

# Database

Main Entities

- Users
- Customers
- Shipments
- TrackingEvents
- RefreshTokens

Relationships

User

↓

Many Refresh Tokens

Customer

↓

Many Shipments

Shipment

↓

Many Tracking Events

---

# Logging

Implemented

- Global Exception Handling
- Request Logging
- Response Logging
- Correlation IDs
- Serilog File Logging

---

# Cloud Deployment

Application

Azure App Service

Database

Azure SQL Database

Configuration

- Environment Variables
- User Secrets (Development)

Deployment

GitHub Actions

↓

Automatic Build

↓

Automatic Deployment

↓

Azure App Service

---

# Security

Implemented

- Password Hashing
- JWT Authentication
- Refresh Token Rotation
- Refresh Token Revocation
- Role-Based Authorization
- Secure Configuration
- Environment Variables

---

# Testing

Unit Tests

Authentication

- Login
- Invalid Login
- Refresh Token
- Logout
- Token Rotation
- Expired Token
- Revoked Token

---

# Current Release

v1.3.0

Completed

- Backend Foundation
- Authentication
- Azure Deployment
- CI/CD
- Authorization
- Refresh Tokens

---

# Planned Features

Phase 6

- API Versioning
- Rate Limiting
- Response Caching
- Health Checks
- Security Headers
- CORS Improvements

Phase 7

- Shipment Pricing Engine
- Billing
- Invoice Generation
- Payment Status

Phase 8

- Azure Blob Storage
- Email Service
- Background Jobs
- Redis
- Docker

Phase 9

- AI Shipment Assistant
- AI Search
- Shipment Summary Generation
- Document Processing