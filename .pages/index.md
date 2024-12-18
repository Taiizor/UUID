---
layout: default
title: UUID - Modern .NET UUID Generator
---

# Welcome to UUID

A modern, high-performance UUID (Universally Unique Identifier) generator for the .NET ecosystem. Built with focus on efficiency, security, and ease of use.

## ✨ Features

- 🚀 High-performance UUID generation
- 🔒 Cryptographically secure by default
- 🧵 Thread-safe operations
- 💾 Zero-allocation options
- 🔄 Compatible with standard GUID/UUID formats
- 📦 Cross-platform support

## 🚀 Quick Start

### Installation

```bash
dotnet add package UUID
```

### Basic Usage

```csharp
using UUID;

// Generate a new UUID
var id = new UUID();

// Convert to string
string str = id.ToString();

// Parse from string
UUID parsed = UUID.Parse("123e4567-e89b-12d3-a456-426614174000");
```

## 📚 Documentation

- [Getting Started](docs/getting-started)
- [API Reference](docs/api)
- [Examples](docs/examples)
- [Performance Guide](docs/performance)
- [Security Guide](docs/security)
- [FAQ](docs/faq)

## 💡 Why UUID?

- **Performance**: Optimized for high-throughput scenarios
- **Security**: Cryptographically secure random number generation
- **Reliability**: Extensive testing across platforms
- **Simplicity**: Clean, intuitive API design

## 🤝 Contributing

We welcome contributions! See our [Contributing Guide](CONTRIBUTING.md) for details.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 💬 Community

- [Discord Community](https://discord.gg/nxG977byXb)
- [Stack Overflow](https://stackoverflow.com/questions/tagged/uuid)
- [GitHub Issues](https://github.com/Taiizor/UUID/issues)