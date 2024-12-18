![Logo](.images/Logo.png)

![Dot-Net-Framework-Version](https://img.shields.io/badge/.NET%20Framework-%3E%3D4.8-blue)
![Dot-Net-Standard-Version](https://img.shields.io/badge/.NET%20Standard-%3E%3D2.0-blue)
![Dot-Net-Version](https://img.shields.io/badge/.NET-%3E%3D6.0-blue)
![C-Sharp-Version](https://img.shields.io/badge/C%23-Preview-blue.svg)
[![IDE-Version](https://img.shields.io/badge/IDE-VS2022-blue.svg)](https://visualstudio.microsoft.com/downloads)
[![NuGet-Version](https://img.shields.io/nuget/v/UUID.svg?label=NuGet)](https://www.nuget.org/packages/UUID)
[![NuGet-Download](https://img.shields.io/nuget/dt/UUID?label=Download)](https://www.nuget.org/api/v2/package/UUID)
[![Stack Overflow](https://img.shields.io/badge/Stack%20Overflow-UUID-orange.svg)](https://stackoverflow.com/questions/tagged/uuid)

[![.NET](https://github.com/Taiizor/UUID/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Taiizor/UUID/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/Taiizor/UUID/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/Taiizor/UUID/actions/workflows/codeql-analysis.yml)
[![.NET Desktop](https://github.com/Taiizor/UUID/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/Taiizor/UUID/actions/workflows/dotnet-desktop.yml)

[![Discord-Server](https://img.shields.io/discord/932386235538878534?label=Discord)](https://discord.gg/nxG977byXb)

# Welcome to UUID
UUID is a modern and efficient unique identifier generator for .NET ecosystem. This high-performance library is designed for modern distributed systems, providing thread-safe operations and time-ordered identifiers with enhanced security features.

## Contributors

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<table>
  <tr>
    <td align="center">
		<a href="https://github.com/Taiizor">
			<img src="https://avatars3.githubusercontent.com/u/41683699?s=460&v=4" width="80px;" alt="Taiizor"/>
			<br/>
			<sub>
				<b>Taiizor</b>
			</sub>
		</a>
		<br/>
		<a href="https://github.com/Taiizor/UUID/commits?author=Taiizor" title="Code">💻</a>
		<a href="https://www.vegalya.com" title="Ideas & Planning, Feedback">🤔</a>
	</td>
  </tr>
</table>

This project follows the [all contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!

## Backers

<table>
  <tr>
    <td align="center">
		<a href="https://github.com/Vegalya">
			<img src="https://avatars3.githubusercontent.com/u/98421771?s=200&v=4" width="80px;" alt="Vegalya"/>
			<br/>
			<sub>
				<b>Vegalya</b>
			</sub>
		</a>
		<br/>
		<a href="https://github.com/Vegalya" target="_blank" title="Content">🖋</a>
	</td>
    <td align="center">
		<a href="https://github.com/Soferity">
			<img src="https://avatars3.githubusercontent.com/u/63516515?s=200&v=4" width="80px;" alt="Soferity"/>
			<br/>
			<sub>
				<b>Soferity</b>
			</sub>
		</a>
		<br/>
		<a href="https://github.com/Soferity" target="_blank" title="Content">🖋</a>
	</td>
  </tr>
</table>

## Platform support

UUID works on .NET Framework, .NET Standard, .NET Core and .NET.

<table>
   <thead>
      <tr>
         <th>.NET implementation</th>
         <th>Version support</th>
      </tr>
   </thead>
   <tbody>
      <tr>
         <td>.NET and .NET Core</td>
         <td>2.0, 2.1, 2.2, 3.0, 3.1, 5.0, 6.0, 7.0, 8.0, 9.0</td>
      </tr>
      <tr>
         <td>.NET Framework</td>
         <td>4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1</td>
      </tr>
      <tr>
         <td>Mono</td>
         <td>5.4, 6.4</td>
      </tr>
      <tr>
         <td>Xamarin.iOS</td>
         <td>10.14, 12.16</td>
      </tr>
      <tr>
         <td>Xamarin.Mac</td>
         <td>3.8, 5.16</td>
      </tr>
      <tr>
         <td>Xamarin.Android</td>
         <td>8.0, 10.0</td>
      </tr>
      <tr>
         <td>Universal Windows Platform</td>
         <td>10.0.16299, TBD</td>
      </tr>
      <tr>
         <td>Unity</td>
         <td>2018.1</td>
      </tr>
   </tbody>
</table>

Binaries for all platforms are built from a single Visual Studio Project. You will need the latset [Visual Studio](https://visualstudio.microsoft.com/downloads) to build or contribute to UUID.

## Features

- **High Performance & Thread Safety**
  - Thread-safe operations optimized for performance
  - Thread-local secure random generation
  - Efficient memory usage with 16-byte format

- **Time-Based Ordering**
  - Natural sorting based on creation time
  - Monotonic timestamps for consistent ordering
  - Perfect for distributed systems and databases

- **Security**
  - Cryptographically secure random generation
  - Enhanced protection against prediction and collision

- **Multiple Format Support**
  - Base32 encoding for URL-friendly strings
  - Base64 encoding for compact representation
  - Guid compatibility
  - Efficient string parsing and formatting

- **Rich API**
  - Implicit/explicit conversion operators
  - Comparison and equality operations
  - Comprehensive test coverage
  - Cross-platform compatibility

## Getting started

UUID is distributed via Microsofts package manager [NuGet](https://www.nuget.org). We refer to [this page](https://docs.microsoft.com/en-gb/nuget) for detailed descriptions on how to get started/use NuGet. Here is a link to the [UUID NuGet package](https://www.nuget.org/packages/UUID).
You can grab a copy of the library on NuGet by running:

By Package Manager (PM): 
```sh 
Install-Package UUID
```

By .NET CLI: 
```sh 
dotnet add package UUID
```

## Quick Start

```csharp
using UUID;

// Generate a new UUID
UUID id = UUID.New();

// Convert to string formats
string str = id.ToString();        // Standard format
string base32 = id.ToBase32();     // URL-friendly
string base64 = id.ToBase64();     // Compact

// Parse from string
UUID parsed = UUID.Parse(str);
bool success = UUID.TryParse(str, out UUID result);

// Guid compatibility
Guid guid = id.ToGuid();
UUID fromGuid = UUID.FromGuid(guid);

// Implicit/Explicit conversions
UUID implicitFromGuid = guid;      // Implicit
Guid explicitToGuid = (Guid)id;    // Explicit

// Time component
DateTimeOffset timestamp = id.Time;

// Comparison operations
bool equals = id == parsed;
bool lessThan = id < parsed;
```

## Documentation and Support

To learn more about UUID, check out our [documentation](https://github.com/Taiizor/UUID/wiki). You can get help via:

* [Stack Overflow](https://stackoverflow.com/questions/tagged/uuid)
* [Discord](https://discord.gg/nxG977byXb)
* [Issue Tracker](https://github.com/Taiizor/UUID/issues)

## Contributing

Would you like to help make UUID even better? We keep a list of issues that are approachable for newcomers under the [solved](https://github.com/Taiizor/UUID/issues?q=is%3Aissue+label%3Asolved) label (accessible only when logged into GitHub). Before starting work on a pull request, we suggest commenting on, or raising, an issue on the issue tracker so that we can help and coordinate efforts.

When contributing please keep in mind our [Code of Conduct](CODE_OF_CONDUCT.md).

_UUID is copyright &copy; 2024-2025 UUID Contributors - Provided under the [MIT License](LICENSE)._