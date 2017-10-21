// Addins
#addin "Cake.Git"

// Tools
#tool "GitReleaseManager"
#tool "GitVersion.CommandLine"

// Arguments
var target        = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Preparation
var local = BuildSystem.IsLocalBuild;
var artifactDirectory = "./artifacts/";

// GitVersion
var gitVersion = GitVersion();
var majorMinorPatch = gitVersion.MajorMinorPatch;
var informationalVersion = gitVersion.InformationalVersion;
var nugetVersion = gitVersion.NuGetVersion;
var buildVersion = gitVersion.FullBuildMetaData;

// Setup
Setup((ctx) =>
{
    Information("Building version {0} of RestLess. Nuget version {1}", informationalVersion, nugetVersion);
    Information("IsTagged: {0}", "");
    CreateDirectory(artifactDirectory);
});

// Tasks

Task("UpdateBuildNumber")
    .WithCriteria(() => !local)
    .Does(() =>
    {
        Information("##vso[build.updatebuildnumber]{0}", majorMinorPatch);
    });    

Task("Git")
    .Does(() =>
    {
        var tags = GitTags("../");
        if(tags.Count > 0)
        {
            var lastTag = tags.LastOrDefault();
            Information("Tag {0}", lastTag.FriendlyName);
        }
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