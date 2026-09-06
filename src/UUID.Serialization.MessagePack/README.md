# UUID.Serialization.MessagePack

A MessagePack serialization provider for the UUID library.

## Installation

```bash
dotnet add package UUID.Serialization.MessagePack
```

## Usage

### Basic Usage

```csharp
using MessagePack;
using MessagePack.Resolvers;

// Register the resolver globally
var options = MessagePackSerializerOptions.Standard
    .WithResolver(CompositeResolver.Create(
        UUIDResolver.Instance,
        StandardResolver.Instance
    ));

// Serialize
var uuid = new UUID();
byte[] bytes = MessagePackSerializer.Serialize(uuid, options);

// Deserialize
UUID deserializedUuid = MessagePackSerializer.Deserialize<UUID>(bytes, options);
```

### With Models

```csharp
public class UserModel
{
    [Key(0)]
    public UUID Id { get; set; }
    
    [Key(1)]
    public string Name { get; set; }
}

// Register resolver
var options = MessagePackSerializerOptions.Standard
    .WithResolver(CompositeResolver.Create(
        UUIDResolver.Instance,
        StandardResolver.Instance
    ));

// Serialize model
var user = new UserModel 
{ 
    Id = new UUID(),
    Name = "John Doe"
};
byte[] bytes = MessagePackSerializer.Serialize(user, options);

// Deserialize model
UserModel deserializedUser = MessagePackSerializer.Deserialize<UserModel>(bytes, options);
```

## Features

- Support for null handling
- Thread-safe implementation
- Comprehensive error handling
- Efficient binary serialization
- Full documentation with XML comments
- Seamless integration with MessagePack
- Custom error messages for better debugging

## Error Handling

The formatter provides detailed error messages for common scenarios:

- Null values: "Cannot convert null value to UUID"
- Invalid byte length: "UUID must be exactly 16 bytes long"
- Incorrect MessagePack types: Shows expected vs actual type
- Invalid binary format: Includes details about the attempted deserialization

## Requirements

- UUID library
- MessagePack 3.1.8 or later
- Supports .NET 6.0+, .NET Standard 2.0+, and .NET Framework 4.8+

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the [MIT License](https://github.com/Taiizor/UUID/blob/develop/LICENSE). See the LICENSE file in the root directory for more details.