Function Main {
    Write-Host "`nFetching IP Addresses"

    $localIP = Get-LocalIPAddress
    $publicIP = Get-PublicIPAddress("https://api.ipify.org?format=json");

    Write-Host "`nResults"
    Write-Host ($null -eq $localIP ? "Not found" : "$localIP`n")
    Write-Host ($null -eq $publicIP ? "Not found" : "$publicIP`n")
}

function Get-LocalIPAddress {
    $HostName = [System.Net.Dns]::GetHostName()
    Write-Host "Host name is $HostName"

    $HostEntry = [System.Net.Dns]::GetHostEntry($HostName)
    $LocalIPAddress = $HostEntry.AddressList | Where-Object { $_.AddressFamily -eq [System.Net.Sockets.AddressFamily]::InterNetwork } | Select-Object -First 1

    if ($null -ne $LocalIPAddress) {
        return $LocalIPAddress
    }
    else {
        Write-Error "Could not find the IP address."
        return $null;
    }
}

function Get-PublicIPAddress {
    param (
        [string] $IPProviderUrl
    )

    try {
        if ($null -ne $IPProviderUrl) {
            $webJob = Start-Job -ScriptBlock { Invoke-WebRequest -Uri $using:IPProviderUrl }
            Wait-Job $webJob

            $webResponse = Receive-Job $webJob
            Remove-Job $webJob # Remove the job after receiving the output

            $responseObject = $webResponse | Where-Object { $_.PSObject.Properties.Name -ccontains 'Content' }
            if ($null -ne $responseObject) {
                $publicIPAddress = ($responseObject.Content | ConvertFrom-Json).ip
                return $publicIPAddress
            }
            else {
                Write-Error "No response received, check with provider: $($using:IPProviderUrl)"
                return $null
            }
        }
        else {
            Write-Error "No IP Provider URL found, exiting."
            return $null
        }
    }
    catch {
        <#Do this if a terminating exception happens#>
        Write-Host "Error occured: $($_.Exception.Message)"
        return $null
    }
}

Main