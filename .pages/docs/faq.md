---
layout: default
title: Frequently Asked Questions
---

# Frequently Asked Questions

## General Questions

### What is UUID?
UUID (Universally Unique Identifier) is a 128-bit identifier that can be used to uniquely identify information in distributed systems.

### Why use this UUID library?
Our library offers:
- High performance
- Thread safety
- Cryptographic security
- Modern .NET features
- Extensive documentation

### Is it compatible with standard UUIDs/GUIDs?
Yes, our UUIDs are fully compatible with standard UUID/GUID formats and can be used interchangeably.

## Technical Questions

### How is it different from System.Guid?
- Better performance
- More modern API design
- Additional format options
- Enhanced security features

### Is it thread-safe?
Yes, all operations are thread-safe by design:
```csharp
// Safe for parallel operations
Parallel.For(0, 1000, _ => {
    UUID id = new UUID();
    // Thread-safe operations
});
```

### What string formats are supported?
```csharp
UUID id = new UUID();

// Standard format
string standard = id.ToString();  // "123e4567-e89b-12d3-a456-426614174000"

// Base32 (URL-safe)
string base32 = id.ToBase32String();  // "6QR8H1RZWM8VDPJ38WSKDD5GR0"

// Base64
string base64 = id.ToBase64String();  // "Ej5FZ+ibEtOkVkJmFBdAAA=="
```

## Performance Questions

### How fast is it?
Our benchmarks show excellent performance:
- 20.5M UUIDs/second on single thread
- 289.8M UUIDs/second on 16 threads

### How can I optimize bulk operations?
Use array pooling and parallel processing:
```csharp
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

## Security Questions

### Is it cryptographically secure?
Yes, we use cryptographically secure random number generation by default.

### How should I store UUIDs securely?
- Store as binary in databases
- Use parameterized queries
- Validate all inputs

### What if I find a security issue?
1. Don't open a public issue
2. Email taiizor@vegalya.com
3. Include detailed information
4. Wait for confirmation

## See Also

- [Getting Started](getting-started)
- [API Reference](api)
- [Security Guide](security)
- [Performance Guide](performance)
