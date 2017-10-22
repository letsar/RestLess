// Addins
#addin "Cake.Git"

// Tools
#tool "GitReleaseManager"
#tool "GitVersion.CommandLine"
#tool "NUnit.ConsoleRunner"
#tool "vswhere"

// Arguments
var target        = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var nugetApiKey   = Argument("nugetApiKey", "");
var nugetSource   = Argument("nugetSource", "");

// Preparation
var product = "RestLess";
var local = BuildSystem.IsLocalBuild;
var artifactDirectory = "./artifacts/";
var rootDir = "../";
var isTagged = false;
var src = "../src/";
var msBuildPath = VSWhereLatest().CombineWithFilePath("./MSBuild/15.0/Bin/MSBuild.exe");

// Execution variables
var majorMinorPatch = "0.0.0";
var informationalVersion = majorMinorPatch;
var nugetVersion = majorMinorPatch;
var buildVersion = majorMinorPatch;
var assemblySemVer = majorMinorPatch;
var branchName = "";
var isMasterBranch = false;

// Setup
Setup((ctx) =>
{
    CreateDirectory(artifactDirectory);
});

// Tasks
Task("GitVersion")
    .Does(() =>
    {
        var gitVersion = GitVersion();
        majorMinorPatch = gitVersion.MajorMinorPatch;
        informationalVersion = gitVersion.InformationalVersion;
        nugetVersion = gitVersion.NuGetVersion;
        buildVersion = gitVersion.FullBuildMetaData;
        assemblySemVer = gitVersion.AssemblySemVer;
        branchName = gitVersion.BranchName ;
        isMasterBranch = branchName == "master";
        Information("Building version {0} of {1}. Nuget version {2}. Assembly version {3}", informationalVersion, product, nugetVersion, assemblySemVer);
        Information("Branch {0}", branchName);
    }); 

Task("CheckIfTagged")
    .Does(() =>
    {      
        var gitDescribe = GitDescribe(rootDir,GitDescribeStrategy.Tags);
        isTagged = !string.IsNullOrEmpty(gitDescribe) && !gitDescribe.Contains("-");        
        if(isTagged)
        {
            Information("This commit is tagged with {0}", gitDescribe);
        }
        else
        {
            Warning("This commit is not tagged!");
        }
    }); 

Task("UpdateBuildNumber")
    .IsDependentOn("GitVersion")
    .WithCriteria(() => !local)
    .Does(() =>
    {
        Information("##vso[build.updatebuildnumber]{0}", majorMinorPatch);
    });    

Task("Clean")
    .Does(() =>
    {
        var solution = src + "RestLess.sln";

        Information("Cleaning {0}", solution);

        MSBuild(solution, new MSBuildSettings() { ToolPath= msBuildPath}
            .WithTarget("clean")
            .SetConfiguration(configuration)          
            .SetVerbosity(Verbosity.Minimal)
            .SetNodeReuse(false));        
    });

Task("BuildNugetProjects")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        var projects = new[] {"RestLess.Core", "RestLess.Tasks", "RestLess", "RestLess.Core.Tests", "RestLess.Tasks.Tests"};

        foreach(var project in projects)     
        {
            var projectPath = src + project + "/" + project + ".csproj";
            Information("Building {0}", projectPath);

            MSBuild(projectPath, new MSBuildSettings() { ToolPath= msBuildPath}
                .WithTarget("restore;build")
                .WithProperty("PackageOutputPath", MakeAbsolute(Directory(artifactDirectory)).ToString())
                .WithProperty("AssemblyVersion", assemblySemVer.ToString())
                .WithProperty("FileVersion", assemblySemVer.ToString())
                .WithProperty("Version", nugetVersion.ToString())
                .SetConfiguration(configuration)          
                .SetVerbosity(Verbosity.Minimal)
                .SetNodeReuse(false));        
        }
    });   

Task("PackNugetProjects")
    .IsDependentOn("BuildNugetProjects")
    .Does(() =>
    {
        var projects = new[] {"RestLess.Core", "RestLess"};

        foreach(var project in projects)     
        {
            var projectPath = src + project + "/" + project + ".csproj";
            Information("Packing {0}", projectPath);

            MSBuild(projectPath, new MSBuildSettings() { ToolPath= msBuildPath}
                .WithTarget("pack")
                .WithProperty("PackageOutputPath", MakeAbsolute(Directory(artifactDirectory)).ToString())
                .WithProperty("AssemblyVersion", assemblySemVer.ToString())
                .WithProperty("FileVersion", assemblySemVer.ToString())
                .WithProperty("Version", nugetVersion.ToString())
                .SetConfiguration(configuration)          
                .SetVerbosity(Verbosity.Minimal)
                .SetNodeReuse(false));        
        }
    });   

Task("UpdateRestLessPackage")
    .IsDependentOn("PackNugetProjects")
    .WithCriteria(() => local)  // Does not work on VSTS.
    .Does(() =>
    {
        var projects = new[] {"RestLess.Tests", "RestLess.ConsoleSample", "RestLess.Sample"};
        var source = MakeAbsolute(Directory(artifactDirectory));

        foreach(var project in projects)     
        {
            var projectPath = src + project + "/" + project + ".csproj";

            try
            {
                // Remove old package.
                DotNetCoreTool(projectPath,"remove","package RestLess");         
            }
            catch{} 

            // Add new package.   
            var arguments = "package RestLess -v "+ nugetVersion + " -s \""+ source + "\"";
            Information("dotnet add " + projectPath + " " + arguments);
            try
            {
                DotNetCoreTool(projectPath, "add", arguments);
            }
            catch{}
            
            // Build.
            MSBuild(projectPath, new MSBuildSettings() { ToolPath= msBuildPath}
                .WithTarget("build")                
                .SetConfiguration(configuration)          
                .SetVerbosity(Verbosity.Minimal)
                .SetNodeReuse(false));        
        }
    });  

Task("Tests")
    .IsDependentOn("UpdateRestLessPackage")
    .ContinueOnError()
    .Does(() =>
    {
        var projects = new[] {"RestLess.Core.Tests", "RestLess.Tasks.Tests", "RestLess.Tests"};
        if(!local)
        {
            projects = new[] {"RestLess.Core.Tests", "RestLess.Tasks.Tests"};
        }        

        foreach(var project in projects)     
        {
            var projectPath = src + project + "/" + project + ".csproj";
            Information("Testing {0}", projectPath);

            var settings = new DotNetCoreTestSettings
            {
                Framework = "netcoreapp2.0",
                Configuration = configuration,
                NoBuild = true,
                Logger = "trx;LogFileName=TestResults.xml",
            };
            DotNetCoreTest(projectPath, settings);    
        }
    });

Task("PublishPackages")
    .IsDependentOn("CheckIfTagged")
    .IsDependentOn("Tests")
    .WithCriteria(() => !local)
    .WithCriteria(() => isTagged)
    .WithCriteria(() => !string.IsNullOrEmpty(nugetApiKey))
    .WithCriteria(() => !string.IsNullOrEmpty(nugetSource))
    .Does(()=>
    {
        var projects = new[] {"RestLess.Core", "RestLess"};
        foreach(var project in projects)
        {
            var nupkg = Directory(artifactDirectory) + File(string.Format("{0}.{1}.nupkg", project, nugetVersion));
            var settings = new NuGetPushSettings 
            {
                Source = nugetSource,
                ApiKey = nugetApiKey
            };
            NuGetPush(nupkg, settings);
        }
    });

// Task targets
Task("Default")
    .IsDependentOn("UpdateBuildNumber")
    .IsDependentOn("PublishPackages")
    .Does(() =>
    {        
    });    

// Execution
RunTarget(target);