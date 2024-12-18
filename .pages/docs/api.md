---
layout: default
title: API Reference
---

# API Reference

## UUID Struct

The core type that represents a UUID.

### Constructors

```csharp
// Creates a new UUID using cryptographically secure random numbers
public UUID()

// Creates a UUID from bytes
public UUID(byte[] bytes)

// Creates a UUID from a span of bytes
public UUID(ReadOnlySpan<byte> bytes)
```

### Properties

```csharp
// Gets the bytes that make up the UUID
public ReadOnlySpan<byte> Bytes { get; }

// Gets whether this UUID is empty (all zeros)
public bool IsEmpty { get; }

// Gets whether this UUID is valid
public bool IsValid { get; }
```

### Methods

```csharp
// Converts the UUID to its string representation
public override string ToString()

// Converts to Base32 string format
public string ToBase32String()

// Converts to Base64 string format
public string ToBase64String()

// Attempts to write the string representation to a buffer
public bool TryFormat(Span<char> destination, out int charsWritten)

// Parses a UUID from its string representation
public static UUID Parse(string input)

// Attempts to parse a UUID from its string representation
public static bool TryParse(string input, out UUID result)

// Creates a new UUID using the specified random number generator
public static UUID NewUUID(Random random)
```

### Operators

```csharp
// Equality operators
public static bool operator ==(UUID left, UUID right)
public static bool operator !=(UUID left, UUID right)

// Comparison operators
public static bool operator <(UUID left, UUID right)
public static bool operator >(UUID left, UUID right)
public static bool operator <=(UUID left, UUID right)
public static bool operator >=(UUID left, UUID right)
```

## Extension Methods

### String Extensions

```csharp
// Converts a string to UUID
public static UUID ToUUID(this string str)

// Tries to convert a string to UUID
public static bool TryToUUID(this string str, out UUID result)
```

### Byte Array Extensions

```csharp
// Converts byte array to UUID
public static UUID ToUUID(this byte[] bytes)

// Tries to convert byte array to UUID
public static bool TryToUUID(this byte[] bytes, out UUID result)
```

## Interfaces

### IUUIDGenerator

Interface for custom UUID generation strategies.

```csharp
public interface IUUIDGenerator
{
    UUID GenerateUUID();
    Task<UUID> GenerateUUIDAsync();
}
```

## See Also

- [Getting Started](getting-started)
- [Examples](examples)
- [Performance Guide](performance)
- [Security Guide](security)
