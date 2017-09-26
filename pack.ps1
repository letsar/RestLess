$nugetProject = ".\src\DoLess.Rest.Standard\DoLess.Rest.Standard.20.csproj"
$taskProject = ".\src\DoLess.Rest.Tasks\DoLess.Rest.Tasks.csproj"
$buildNumber = Get-Date -Format 0.MM.ddHHmm

dotnet clean $taskProject -c Debug
dotnet clean $nugetProject -c Debug
dotnet build $taskProject -c Debug
dotnet build $nugetProject -c Debug
dotnet pack $nugetProject /p:PackageVersion=$buildNumber