# Pri.Cleaner

## scaffolding

```powershell
md Pri.Cleaner
cd .\Pri.Cleaner\
dotnet new consoleapp -o Pri.Cleaner
ren .\Pri.Cleaner\ Cleaner
cd .\Cleaner\
dotnet new xunit -o Tests
dotnet reference add .\Cleaner\ --project Tests
dotnet new sln
dotnet sln add .\Cleaner\
dotnet sln add .\Tests\
dotnet new classlib --framework "net8.0" -o Pri.Cleaner.Core
ren Pri.Cleaner.Core Core
del .\Core\Class1.cs
dotnet sln add .\Core\
dotnet reference add .\Core\ --project .\Cleaner\
```