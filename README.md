**Note: This project is currently incomplete and under development.**
# Quran

## Description

`Quran` is an F# class library designed to provide strongly-typed models and utility functions for handling Quranic data, such as verses and chapters. This library aims to offer a solid foundation for building Quranic applications with enhanced type safety and functionality.

## Features

- Strongly-typed models for Quranic Chapters and Verses.
- Utility functions to validate chapter and verse numbers.

## Installation

### Prerequisites

- .NET SDK 5.0 or higher

### Steps

1. **Clone the repository**:

    ```bash
    git clone https://github.com/abnewstein/Quran.git
    ```

2. **Navigate to the project folder**:

    ```bash
    cd Quran
    ```

3. **Restore dependencies**:

    ```bash
    dotnet restore
    ```

## Usage

To use the library in your F# project, add it as a reference:

```bash
dotnet add reference /path/to/Quran/Quran.fsproj
```

Then, in your F# files, you can use the library like so:

```fsharp
open Quran

// Your code here
```

## Running Tests

We follow Test-Driven Development (TDD). To run the tests:

```bash
dotnet test
```

