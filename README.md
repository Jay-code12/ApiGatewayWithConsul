This project is a microservices-based authentication and user management system built with ASP.NET Core.  
It demonstrates how to implement authentication, service discovery, and inter-service communication in a distributed architecture.

## Features

- User Registration
- JWT Authentication
- ASP.NET Core Identity Integration
- Microservices Architecture
- Service Discovery using Consul
- API Communication using HttpClient
- Role-based Authorization

## Architecture

The system is divided into multiple services:

- **ApiGateway**
  - Handles authentication
  - Manage service routing
  - Ratelimiting
  - Handle services instance loadbalancing
  - use consul service discovery to get services address
 
- **AuthService**
  - Generates JWT tokens
  - Manages Identity users

- **UserService**
  - Manages user profiles
  - Stores additional user data

- **Service Discovery**
  - Uses Consul to dynamically locate services

## Technologies Used

- ASP.NET Core Web API
- ASP.NET Core Identity
- Entity Framework Core
- JWT Authentication
- Consul Service Discovery
- Mapster (Object Mapping)
- SQL Server

## Project Structure

ApiGatewayWithConsul
│
├── AuthService
├── UserService
├── Consul Configuration


