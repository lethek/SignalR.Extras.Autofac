@echo off
if "%1"=="" goto rebuildCurrent

nuget pack %~dp0Source\AutofacExtensions.Integration.SignalR\AutofacExtensions.Integration.SignalR.csproj -Symbols -Build -OutputDirectory %~dp0Build -Properties Configuration=%1
goto end

:rebuildCurrent
nuget pack %~dp0Source\AutofacExtensions.Integration.SignalR\AutofacExtensions.Integration.SignalR.csproj -Symbols -Build -OutputDirectory %~dp0Build
goto end

:end
