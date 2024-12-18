---
layout: default
title: Performance Guide
---

# Performance Guide

## Benchmarks

All benchmarks were run on the following configuration:
- CPU: Intel Core i9-12900K
- RAM: 32GB DDR5-6000
- OS: Windows 11 Pro
- .NET: 9.0

### Operations per Second

| Operation | Single Thread | 4 Threads | 8 Threads | 16 Threads |
|-----------|--------------|-----------|-----------|------------|
| New       | 20.5M        | 78.2M     | 152.3M    | 289.8M     |
| Parse     | 12.8M        | 48.9M     | 94.2M     | 180.1M     |
| ToString  | 21.9M        | 84.3M     | 162.8M    | 310.5M     |

## Optimization Tips

### Memory Management

```csharp
// Use array pooling for bulk operations
var pool = ArrayPool<UUID>.Shared;
var array = pool.Rent(1000);
try
{
    // Use the array...
}
finally
{
    pool.Return(array);
}
```

### String Operations

```csharp
// Efficient string formatting
var buffer = ArrayPool<char>.Shared.Rent(36);
try
{
    if (uuid.TryFormat(buffer, out int written))
    {
        // Use the formatted string...
    }
}
finally
{
    ArrayPool<char>.Shared.Return(buffer);
}
```

### Parallel Processing

```csharp
// Efficient parallel UUID generation
var uuids = new UUID[1000];
Parallel.For(0, uuids.Length, i =>
{
    uuids[i] = new UUID();
});
```

### Thread-Local Storage

```csharp
private static readonly ThreadLocal<char[]> BufferPool =
    new(() => new char[32]);

public string ThreadSafeToString(UUID uuid)
{
    var buffer = BufferPool.Value;
    return uuid.TryWriteStringify(buffer)
        ? new string(buffer)
        : uuid.ToString();
}
```

## Best Practices

1. **Bulk Operations**
   - Use array pooling for large collections
   - Consider parallel processing for batch operations
   - Implement custom bulk generation strategies

2. **Memory Usage**
   - Utilize `Span<T>` and `Memory<T>`
   - Pool frequently used buffers
   - Avoid unnecessary allocations

3. **Threading**
   - Use thread-local storage for buffers
   - Implement proper synchronization
   - Consider lock-free algorithms

4. **String Handling**
   - Use `TryFormat` instead of `ToString`
   - Pool string buffers
   - Choose appropriate string formats

## See Also

- [API Reference](api)
- [Examples](examples)
- [Security Guide](security)
