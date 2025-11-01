@echo off
powershell -NoProfile -ExecutionPolicy Bypass -Command ^
"Write-Host 'Starting Camden Car Park applications...' -ForegroundColor Green; ^
 $webApi = Start-Process 'dotnet' -ArgumentList 'run','--launch-profile','https' -WorkingDirectory './Camden-Car-Park.WebApi' -PassThru -NoNewWindow; ^
 Write-Host 'Waiting for WebApi to start...' -ForegroundColor Yellow; ^
 do { ^
    Start-Sleep -Seconds 2; ^
    try { ^
$response = Invoke-WebRequest -Uri 'https://localhost:5000/swagger/index.html' -UseBasicParsing -ErrorAction Stop; ^
      if ($response.StatusCode -eq 200) { break; } ^
    } catch { ^
        Write-Host 'Waiting for WebApi...' -ForegroundColor Yellow; ^
    } ^
 } while ($true); ^
 Write-Host 'WebApi is ready! Starting Blazor App...' -ForegroundColor Green; ^
 $blazor = Start-Process 'dotnet' -ArgumentList 'run','--launch-profile','https' -WorkingDirectory './Camden-Car-Park.BlazorApp' -PassThru -NoNewWindow; ^
 Write-Host 'Waiting for Blazor App to start...' -ForegroundColor Yellow; ^
 do { ^
    Start-Sleep -Seconds 2; ^
    try { ^
   $response = Invoke-WebRequest -Uri 'https://localhost:2000' -UseBasicParsing -ErrorAction Stop; ^
     if ($response.StatusCode -eq 200) { break; } ^
    } catch { ^
        Write-Host 'Waiting for Blazor App...' -ForegroundColor Yellow; ^
    } ^
 } while ($true); ^
 Start-Process 'https://localhost:2000'; ^
 Write-Host "`nApplications started!" -ForegroundColor Green; ^
 Write-Host 'WebApi running on https://localhost:5000' -ForegroundColor Cyan; ^
 Write-Host 'WebApi Swagger accessible on https://localhost:5000/swagger' -ForegroundColor Cyan; ^
 Write-Host 'BlazorApp running on https://localhost:2000' -ForegroundColor Cyan; ^
 Write-Host "`nPress Ctrl+C to stop both applications" -ForegroundColor Yellow; ^
 try { ^
    Wait-Process -Id $webApi.Id -ErrorAction SilentlyContinue; ^
 } catch { ^
    Stop-Process -Id $webApi.Id -ErrorAction SilentlyContinue; ^
    Stop-Process -Id $blazor.Id -ErrorAction SilentlyContinue; ^
 }"