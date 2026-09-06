# UUID.Serialization.Dapper

A Dapper type handler provider for the UUID library.

## Installation

```bash
dotnet add package UUID.Serialization.Dapper
```

## Usage

### Basic Usage

```csharp
using Dapper;

// Register the binary handler (recommended for storage efficiency)
SqlMapper.AddTypeHandler(new BinaryUUIDHandler());

// Or register the string handler (if you need human-readable values)
SqlMapper.AddTypeHandler(new StringUUIDHandler());

// Or register the Base64 handler (for compact string representation)
SqlMapper.AddTypeHandler(new Base64UUIDHandler());

// Example query
var user = connection.QuerySingle<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = UUID.New() });
```

### With Models

```csharp
public class UserModel
{
    public UUID Id { get; set; }
    public string Name { get; set; }
}

// Example insert
var user = new UserModel 
{ 
    Id = UUID.New(),
    Name = "John Doe"
};

connection.Execute("INSERT INTO Users (Id, Name) VALUES (@Id, @Name)", user);

// Example select
var users = connection.Query<UserModel>("SELECT * FROM Users");
```

## Features

- Thread-safe implementation
- Comprehensive error handling
- Seamless integration with Dapper
- Full documentation with XML comments
- Custom error messages for better debugging
- Base64 UUID Handler: Store UUIDs as 24-character strings (compact)
- Binary UUID Handler: Store UUIDs as 16-byte binary data (most efficient)
- String UUID Handler: Store UUIDs as 32-character fixed-length strings (human-readable)

## Storage Options

### Binary Handler (Recommended)
- Ideal for database indexing
- Most efficient storage (16 bytes)
- Perfect for high-performance scenarios

### String Handler
- Human-readable format
- Useful for debugging and logging
- 32-character hexadecimal representation

### Base64 Handler
- URL-safe encoding
- Balance between readability and size
- Compact string representation (24 characters)

## Requirements

- UUID library
- Dapper 2.1.79 or later
- Supports .NET 6.0+, .NET Standard 2.0+, and .NET Framework 4.8+

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the [MIT License](https://github.com/Taiizor/UUID/blob/develop/LICENSE). See the LICENSE file in the root directory for more details.