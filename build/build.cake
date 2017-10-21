// Addins
#addin "Cake.Git"

// Tools
#tool "GitReleaseManager"
#tool "GitVersion.CommandLine"

// Arguments
var target        = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Preparation
var product = "RestLess";
var local = BuildSystem.IsLocalBuild;
var artifactDirectory = "./artifacts/";
var rootDir = "../";

// Execution variables
var majorMinorPatch = "0.0.0";
var informationalVersion = majorMinorPatch;
var nugetVersion = majorMinorPatch;
var buildVersion = majorMinorPatch;

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
        Information("Building version {0} of {1}. Nuget version {2}", informationalVersion, product, nugetVersion);
        Information("{0}", gitVersion.CommitsSinceVersionSource);
        Information("{0}", gitVersion.PreReleaseNumber);
        Information("{0}", gitVersion.BuildMetaData);
        Information("{0}", gitVersion.FullSemVer);
    }); 

Task("Git")
    .Does(() =>
    {
        var commit = GitLogTip(rootDir);
        Information("{0}", commit);
        var tags = GitTags(rootDir);
        if(tags.Count > 0)
        {
            var lastTag = tags.LastOrDefault();
            Information("{0}", lastTag);
        }

        Information("{0}", GitDescribe(rootDir,false,GitDescribeStrategy.Tags));
        Information("{0}", GitDescribe(rootDir,true,GitDescribeStrategy.Tags));

    }); 

Task("UpdateBuildNumber")
    .IsDependentOn("GitVersion")
    .WithCriteria(() => !local)
    .Does(() =>
    {
        Information("##vso[build.updatebuildnumber]{0}", majorMinorPatch);
    });    



// Task targets
Task("Default")
    .IsDependentOn("UpdateBuildNumber")
    .IsDependentOn("Git")
    .Does(() =>
    {
        Information("Git Version: {0}", nugetVersion);
    });    

// Execution
RunTarget(target);