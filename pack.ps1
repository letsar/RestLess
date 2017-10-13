$nugetCoreProject = ".\src\DoLess.Rest.Core\DoLess.Rest.Core.csproj"
$nugetTaskProject = ".\src\DoLess.Rest.Tasks\DoLess.Rest.Tasks.csproj"
$nugetRestProject = ".\src\DoLess.Rest\DoLess.Rest.csproj"
$nugetJsonProject = ".\src\DoLess.Rest.Newtonsoft.Json\DoLess.Rest.Newtonsoft.Json.csproj"
$buildNumber = "0.1.0-build$([System.DateTime]::Now.ToString('yyyyMMdd-HHmm'))"

dotnet clean $nugetCoreProject -c Debug
dotnet clean $nugetTaskProject -c Debug
dotnet clean $nugetRestProject -c Debug
dotnet clean $nugetJsonProject -c Debug

dotnet build $nugetCoreProject -c Debug
dotnet build $nugetTaskProject -c Debug
dotnet build $nugetRestProject -c Debug
dotnet build $nugetJsonProject -c Debug

dotnet pack $nugetCoreProject /p:PackageVersion=$buildNumber -o "..\..\..\..\Nuget"
dotnet pack $nugetRestProject /p:PackageVersion=$buildNumber -o "..\..\..\..\Nuget"
dotnet pack $nugetJsonProject /p:PackageVersion=$buildNumber -o "..\..\..\..\Nuget"