# IPReader

IPReader is a small utility that prints the machine's local IPv4 address and the public IP address (as seen by services on the internet). This functionality has been implemented both as a .NET console application and as PowerShell scripts to offer flexibility across environments.

## Features

- Prints your local IPv4 address (from DNS host entries).
- Fetches your public IP address using https://api.ipify.org.
- Available as a simple, single-file console app in .NET 8 and as reusable PowerShell scripts.

## Languages and Paths

| Language        | Path                      | Description                                        |
|-----------------|---------------------------|---------------------------------------------------|
| .NET 8 (C#)     | `/dotnet/IPReader`        | Console app fetching and displaying local and public IPs. Includes JSON parsing and error handling. |
| PowerShell      | `/powershell/IPReader.ps1`| PowerShell script performing the same IP fetches using native cmdlets (`Invoke-WebRequest`) and parsing JSON responses. |

## Requirements

- .NET 8 SDK (for the .NET app)
- PowerShell 7+ (for the PowerShell scripts)
- Internet access to fetch the public IP

## Quick Start

### .NET Application

Build and run with the .NET CLI (build steps not detailed here, refer to project docs).

### PowerShell Script

Run the script directly with PowerShell 7+:

pwsh ./powershell/IPReader.ps1

## What the scripts do

Both the .NET application and the PowerShell script implement the core functionality of reading IP addresses:

- They enumerate local IPv4 addresses by querying the machine's network configuration.
- They fetch the public IP address by making an HTTP request to a public IP API (such as `https://api.ipify.org`), which returns a JSON response.
- The JSON response is parsed to extract the IP address value.
- Both handle basic error scenarios, such as network unavailability or unexpected API responses, and output results or error messages to the console.

The difference lies mainly in their platform and scripting style: the .NET app uses standard .NET network libraries and JSON parsing packages, while the PowerShell script uses built-in cmdlets like `Invoke-WebRequest` and `ConvertFrom-Json`.

## Example Output

Fetching IP addresses…
Host name is: example-host.local
Results:
Local IP Address: 192.168.1.10
Public IP Address: 203.0.113.45


Exact output varies depending on your network and host name.

## How it works (brief)

- Both implement the same logic: enumerate local IPv4 addresses and fetch public IP via an HTTP JSON API.
- The code differs in platform style (.NET CLI app vs PowerShell script) but delivers equivalent results.

## Contract

- Inputs: none (reads local network info + makes one HTTP request).
- Outputs: console with host name, local IPv4, and public IPv4 (or error messages).

## Edge Cases and Error Modes

- No network or internet: error message on public IP fetch, public IP returns null.
- No IPv4 addresses: local IP reported as `Not found`.
- Unexpected JSON from API: parsing error message shown.

## Dependencies

- .NET app: Newtonsoft.Json NuGet package.
- PowerShell: No external dependencies, uses built-in cmdlets.

## Contributing

PRs and issues welcome. Suggestions:

- Add CLI options (IPv6 support, alternate IP providers).
- Add tests for JSON parsing and error handling.
- Enhance PowerShell script with advanced parameterization.

## License

Unlicensed

---