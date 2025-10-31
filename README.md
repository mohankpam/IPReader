# IPReader

IPReader is a small .NET console application that prints the machine's local IPv4 address and the public IP address (as seen by services on the internet).

## Features

- Prints your local IPv4 address (from DNS host entries).
- Fetches your public IP address using https://api.ipify.org.
- Simple, single-file console app written for .NET 8.

## Requirements

- .NET 8 SDK (dotnet 8.0+)
- Internet access to fetch the public IP (for the public IP feature)

## Quick start

Clone or open this repository and build/run with the .NET CLI.

Build:

```bash
# change into the project folder (example: if you cloned the repo to ~/projects)
cd IPReader
dotnet build -c Debug
```

Run (from the project root):

```bash
dotnet run --project .
```

Or run the produced binary directly:

```bash
./bin/Debug/net8.0/IPReader
```

## Example output


```

Fetching IP addresses...

Host name is: example-host.local

Results:
Local IP Address: 192.168.1.10
Public IP Address: 203.0.113.45

```

Exact output will vary depending on your network and host name.

## How it works (brief)

- The app uses `Dns.GetHostEntry` to enumerate local addresses and selects the first IPv4 address.
- For the public IP it calls `https://api.ipify.org?format=json`, parses the JSON response, and prints the `ip` field.

## Contract

- Inputs: none (the app reads local network configuration and makes one outbound HTTP request).
- Outputs: console text showing host name, local IPv4 (if any), and public IPv4 (if available).
- Success criteria: program prints at least one of the local or public IP addresses or prints informative error messages.

## Edge cases and error modes

- No network / no internet: public IP fetch will fail and the app prints an error message and returns null for public IP.
- No IPv4 addresses bound: local IP will be reported as `Not found`.
- API returns unexpected JSON: public IP parsing will fail and the app prints a parsing error message.

## Dependencies

- Newtonsoft.Json (used for minimal JSON parsing). Dependencies are managed by the project file; `dotnet build` will fetch them.

## Contributing

Feel free to open issues or PRs. Small improvements that are useful:

- Add better CLI options (select IPv4/IPv6, choose alternate public IP provider).
- Add tests for parsing logic.

## License

Unlicensed

---