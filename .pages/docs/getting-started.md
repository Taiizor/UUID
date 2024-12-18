---
layout: default
title: Getting Started with UUID
---

# Getting Started with UUID

## Installation

Install the UUID package via NuGet:

```bash
dotnet add package UUID
```

Or using the Package Manager Console:

```powershell
Install-Package UUID
```

## Basic Usage

```csharp
using UUID;

// Generate a new UUID
var id = new UUID();

// Convert to string
string str = id.ToString();

// Parse from string
UUID parsed = UUID.Parse("123e4567-e89b-12d3-a456-426614174000");
```

## Format Options

```csharp
var uuid = new UUID();

// Different string formats
string standard = uuid.ToString();           // "123e4567-e89b-12d3-a456-426614174000"
string base32 = uuid.ToBase32String();       // "6QR8H1RZWM8VDPJ38WSKDD5GR0"
string base64 = uuid.ToBase64String();       // "Ej5FZ+ibEtOkVkJmFBdAAA=="
```

## Performance Tips

1. **String Formatting**
   - Use appropriate string format methods based on your needs
   - Consider using Base32 for URL-safe strings

2. **Bulk Operations**
   - Use array pooling for bulk operations
   - Consider parallel processing for large batches

3. **Memory Usage**
   - UUID struct is optimized for minimal memory footprint
   - Use array pooling for bulk operations

## Next Steps

- Check out the [Examples](examples) for more usage scenarios
- Read the [API Reference](api) for detailed documentation
- Review [Performance Guide](performance) for optimization tips
- See [Security Guide](security) for security best practices
