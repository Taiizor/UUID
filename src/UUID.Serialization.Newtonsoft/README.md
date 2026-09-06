# UUID.Serialization.Newtonsoft

A Newtonsoft.Json serialization provider for the UUID library.

## Installation

```bash
dotnet add package UUID.Serialization.Newtonsoft
```

## Usage

### Basic Usage

```csharp
using Newtonsoft.Json;

// Register the converter globally
JsonSerializerSettings settings = new()
{
    Converters = { new UUIDConverter() }
};

// Serialize
var uuid = new UUID();
string json = JsonConvert.SerializeObject(uuid, settings);

// Deserialize
UUID deserializedUuid = JsonConvert.DeserializeObject<UUID>(json, settings);
```

### With Models

```csharp
public class UserModel
{
    public UUID Id { get; set; }
    public string Name { get; set; }
}

// Register converter
JsonSerializerSettings settings = new()
{
    Converters = { new UUIDConverter() }
};

// Serialize model
var user = new UserModel 
{ 
    Id = new UUID(),
    Name = "John Doe"
};
string json = JsonConvert.SerializeObject(user, settings);

// Deserialize model
UserModel deserializedUser = JsonConvert.DeserializeObject<UserModel>(json, settings);
```

### Using Attributes

```csharp
public class UserModel
{
    [JsonConverter(typeof(UUIDConverter))]
    public UUID Id { get; set; }
    public string Name { get; set; }
}

// No need to register converter globally
var user = new UserModel 
{ 
    Id = new UUID(),
    Name = "John Doe"
};
string json = JsonConvert.SerializeObject(user);
```

## Features

- Support for null handling
- Thread-safe implementation
- Efficient string conversion
- Comprehensive error handling
- Support for JsonConverter attribute
- Full documentation with XML comments
- Seamless integration with Newtonsoft.Json
- Custom error messages for better debugging

## Error Handling

The converter provides detailed error messages for common scenarios:

- Null values: "Cannot convert null value to UUID"
- Incorrect JSON types: Shows expected vs actual type
- Empty strings: "Cannot convert empty string to UUID"
- Invalid string formats: Includes the attempted value in error message

## Requirements

- UUID library
- Newtonsoft.Json 13.0.5-beta1 or later
- Supports .NET 6.0+, .NET Standard 2.0+, and .NET Framework 4.8+

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the [MIT License](https://github.com/Taiizor/UUID/blob/develop/LICENSE). See the LICENSE file in the root directory for more details.