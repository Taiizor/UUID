---
layout: default
title: UUID Examples
---

# UUID Examples

## Basic Usage

### Generation

```csharp
// Create a new UUID
UUID id = new UUID();

// Create from string
UUID fromString = UUID.Parse("123e4567-e89b-12d3-a456-426614174000");

// Create from bytes
byte[] bytes = new byte[16];
new Random().NextBytes(bytes);
UUID fromBytes = new UUID(bytes);
```

### String Conversion

```csharp
UUID id = new UUID();

// Standard format
string standard = id.ToString();  // "123e4567-e89b-12d3-a456-426614174000"

// Base32 format (URL-safe)
string base32 = id.ToBase32String();  // "6QR8H1RZWM8VDPJ38WSKDD5GR0"

// Base64 format
string base64 = id.ToBase64String();  // "Ej5FZ+ibEtOkVkJmFBdAAA=="
```

## Advanced Usage

### Bulk Generation

```csharp
// Generate multiple UUIDs in parallel
var uuids = Enumerable.Range(0, 1000)
    .AsParallel()
    .Select(_ => new UUID())
    .ToArray();

// Using array pooling
var pool = ArrayPool<UUID>.Shared;
var array = pool.Rent(1000);
try
{
    Parallel.For(0, 1000, i => array[i] = new UUID());
    // Use array...
}
finally
{
    pool.Return(array);
}
```

### Custom Generation

```csharp
public class CustomGenerator : IUUIDGenerator
{
    private readonly Random _random;

    public CustomGenerator(Random random)
    {
        _random = random;
    }

    public UUID GenerateUUID()
    {
        return UUID.NewUUID(_random);
    }

    public Task<UUID> GenerateUUIDAsync()
    {
        return Task.FromResult(GenerateUUID());
    }
}
```

### Database Integration

```csharp
public class User
{
    public UUID Id { get; set; }
    public string Name { get; set; }
}

public class UserRepository
{
    private readonly DbContext _context;

    public async Task<User> CreateUser(string name)
    {
        var user = new User
        {
            Id = new UUID(),
            Name = name
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
```

### Thread-Safe Operations

```csharp
public class ThreadSafeGenerator
{
    private static readonly ThreadLocal<Random> Random =
        new(() => new Random());

    public UUID Generate()
    {
        return UUID.NewUUID(Random.Value);
    }
}
```

## See Also

- [API Reference](api)
- [Performance Guide](performance)
- [Security Guide](security)
