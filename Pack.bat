@echo off
if "%1"=="" goto rebuildCurrent

nuget pack %~dp0Source\SignalR.Extras.Autofac\SignalR.Extras.Autofac.csproj -Symbols -Build -OutputDirectory %~dp0Build -Properties Configuration=%1
goto end

:rebuildCurrent
nuget pack %~dp0Source\SignalR.Extras.Autofac\SignalR.Extras.Autofac.csproj -Symbols -Build -OutputDirectory %~dp0Build
goto end

:end
