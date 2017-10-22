$nugetCoreProject = ".\src\RestLess.Core\RestLess.Core.csproj"
$nugetTaskProject = ".\src\RestLess.Tasks\RestLess.Tasks.csproj"
$nugetRestProject = ".\src\RestLess\RestLess.csproj"
$nugetJsonProject = ".\src\RestLess.Newtonsoft.Json\RestLess.Newtonsoft.Json.csproj"
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