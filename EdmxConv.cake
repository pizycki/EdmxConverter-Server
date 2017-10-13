#tool "nuget:?package=xunit.runner.console"
#addin "Cake.WebDeploy"
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
    CleanDirectories("./src/**/published_website");
    CleanDirectory("./artifacts", f => !f.Hidden && !f.Path.FullPath.Contains("keep") );
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

Task("Publish-Local")
  .IsDependentOn("Clean")
  .IsDependentOn("RestoreNugets")
  .Does(() => {
    MSBuild(solutionPath, settings =>
      settings.SetConfiguration(configuration)
          .WithProperty("DeployOnBuild", "true")
          .WithProperty("PublishProfile", "Local"));

    Zip("./src/EdmxConv.WebAPI/published", "./artifacts/published.zip");
  });

//Task("Deploy-ToAzure")
//  //.IsDependentOn("Publish-Local")
//  .Does(() => {
//      DeployWebsite(new DeploySettings()
//      {
//          SourcePath = "./artifacts/published.zip",
//          PublishUrl = "https://edmxconvapp.scm.azurewebsites.net:443/",
//
//          SiteName = "EdmxConv",
//          Username = EnvironmentVariable("EDMX_AZURE_USER"),
//          Password = EnvironmentVariable("EDMX_AZURE_PASS")
//      });
//  });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("UnitTest");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);