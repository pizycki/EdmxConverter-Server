#tool "nuget:?package=xunit.runner.console"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var solutionPath = "./EdmxConv.sln";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories("./src/**/bin");
});

Task("RestoreNugets")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionPath);
});

Task("Build")
    .IsDependentOn("RestoreNugets")
    .Does(() =>
{
    // Use MSBuild
    MSBuild(solutionPath, settings =>
      settings.SetConfiguration(configuration));
    
});

Task("UnitTest")
    .IsDependentOn("Build")
    .Does(() =>
{
    var testAssemblies = GetFiles("./tests/**/*.Tests.dll");
    XUnit2(testAssemblies,
        new XUnit2Settings {
            Parallelism = ParallelismOption.All,
            HtmlReport = true,
            NoAppDomain = true,
            OutputDirectory = "./build"
    });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("UnitTest");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);