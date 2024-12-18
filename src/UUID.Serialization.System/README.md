# UUID.Serialization.System

A System.Text.Json serialization provider for the UUID library.

## Installation

```bash
dotnet add package UUID.Serialization.System
```

## Usage

### Basic Usage

```csharp
using System.Text.Json;

// Register the converter globally
JsonSerializerOptions options = new()
{
    Converters = { new UUIDConverter() }
};

// Serialize
var uuid = new UUID();
string json = JsonSerializer.Serialize(uuid, options);

// Deserialize
UUID deserializedUuid = JsonSerializer.Deserialize<UUID>(json, options);
```

### With Models

```csharp
public class UserModel
{
    public UUID Id { get; set; }
    public string Name { get; set; }
}

// Register converter
JsonSerializerOptions options = new()
{
    Converters = { new UUIDConverter() }
};

// Serialize model
var user = new UserModel 
{ 
    Id = new UUID(),
    Name = "John Doe"
};
string json = JsonSerializer.Serialize(user, options);

// Deserialize model
UserModel deserializedUser = JsonSerializer.Deserialize<UserModel>(json, options);
```

## Features

- Seamless integration with System.Text.Json
- Efficient string conversion
- Comprehensive error handling
- Thread-safe implementation
- Full documentation with XML comments
- Support for null handling
- Custom error messages for better debugging

## Error Handling

The converter provides detailed error messages for common scenarios:

- Null values: "Cannot convert null value to UUID"
- Invalid string formats: Includes the attempted value in error message
- Empty strings: "Cannot convert empty string to UUID"
- Incorrect JSON types: Shows expected vs actual type

## Requirements

- .NET 9.0 or later
- UUID library
- System.Text.Json 9.0.0 or later

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the terms of the LICENSE file included in the root directory.