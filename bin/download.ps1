Function Get-Webfile ($url, $out)
{
    $dest=(Join-Path $pwd.Path $out)
    Write-Host "Downloading $out" -ForegroundColor DarkGreen;
    Write-Host "from $url`n" -ForegroundColor DarkGreen;
    $uri=New-Object "System.Uri" "$url"
    $request=[System.Net.HttpWebRequest]::Create($uri)
    $request.set_Timeout(5000)
    $response=$request.GetResponse()
    $totalLength=[System.Math]::Floor($response.get_ContentLength()/1024)
    $length=$response.get_ContentLength()
    $responseStream=$response.GetResponseStream()
    $destStream=New-Object -TypeName System.IO.FileStream -ArgumentList $dest, Create

    Try
    {
        $buffer=New-Object byte[] 10KB
        $count=$responseStream.Read($buffer,0,$buffer.length)
        $downloadedBytes=$count
        $nextShow=$downloadedBytes
        while ($count -gt 0)
            {
            if ($downloadedBytes -gt $nextShow)
            {
            	[System.Console]::Write("Downloaded {0}K of {1}K ({2}%)`n", [System.Math]::Floor($downloadedBytes/1024), $totalLength, [System.Math]::Round(($downloadedBytes / $length) * 100,0))
            	$nextShow=$downloadedBytes + 1000000
            }
            $destStream.Write($buffer, 0, $count)
            $count=$responseStream.Read($buffer,0,$buffer.length)
            $downloadedBytes+=$count
            }
        Write-Host ""
        Write-Host "`nDownload of `"$dest`" finished." -ForegroundColor DarkGreen;
        $destStream.Flush()
    }
    Finally
    {
        $destStream.Close()
        $destStream.Dispose()
        $responseStream.Dispose()
    }
}

$completed = $false
Try 
{
    Get-Webfile $args[0] $args[1]
    $completed = $true
}
Finally
{
    if (-Not $completed)
    {
        Write-Host "Download failed!`n" -ForegroundColor Red;
        If (Test-Path $args[1])
        {
            Remove-Item $args[1]
        }
        exit 1
    }
}
