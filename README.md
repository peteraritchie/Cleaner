# Pri.Cleaner

A CLI utility to recursively find all .csproj files within a directory and remove any `bin` or `obj` subdirectories (and contents), _cleaning_ the directory.

## Usage
```text
Description:
  clean

Usage:
  Pri.Cleaner [<--path>] [options]

Arguments:
  <path>  The path to the directory to recurse and clean. [default: .]

Options:
  --dry-run       Only display what will be done if true.
  --version       Show version information
  -?, -h, --help  Show help and usage information
```

## Examples

Perform dry-run on currently directory, detailing what will be removed and how much space will be freed:
```powershell
Pri.Cleaner --dry-run
```



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
dotnet new gitignore
git init .
git add . ; git commit -m "initial commit"
gh repo create Cleaner --public --push --source .
```