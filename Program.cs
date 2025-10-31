using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;

public class Program
{

    static async Task Main()
    {
        Console.WriteLine("\nFetching IP addresses...");

        // Run both tasks in parallel
        var localIPTask = Task.Run(() => GetLocalIPAddress());
        var publicIPTask = GetPublicIPAddress();

        await Task.WhenAll(localIPTask, publicIPTask);

        var localIP = localIPTask.Result;
        var publicIP = publicIPTask.Result;

        Console.WriteLine("\nResults:");
        Console.WriteLine($"Local IP Address: {localIP?.ToString() ?? "Not found"}");
        Console.WriteLine($"Public IP Address: {publicIP?.ToString() ?? "Not found"}\n");
    }

    public static IPAddress? GetLocalIPAddress()
    {
        string hostName = Dns.GetHostName();
        Console.WriteLine($"Host name is: {hostName}");

        IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

        // Find the first IPV4 address
        //Console.WriteLine($"All IP addresses: {string.Join<IPAddress>(", ", hostEntry.AddressList)}");
        IPAddress? localIpAddress = hostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

        if (localIpAddress != null)
        {
            //Console.WriteLine($"Local IPv4 address: {localIpAddress}");
            return localIpAddress;
        }
        else
        {
            Console.WriteLine("Could not find the IP address.");
            return null;
        }
    }

    public static async Task<IPAddress?> GetPublicIPAddress()
    {
        using HttpClient client = new();
        try
        {
            HttpResponseMessage response = await client.GetAsync("https://api.ipify.org?format=json");
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            JObject jsonObject = JObject.Parse(jsonResponse);

            string? ipString = jsonObject["ip"]?.ToString();
            if (string.IsNullOrEmpty(ipString))
            {
                Console.WriteLine("Could not parse public IP address.");
                return null;
            }

            return IPAddress.TryParse(ipString, out IPAddress? publicIPAddress)
                ? publicIPAddress
                : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }
}