
//////////////////////////////////////////////////////////////////////
// IMPORTS
//////////////////////////////////////////////////////////////////////
#addin "nuget:?package=Cake.Incubator&version=5.0.1"

#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"

#tool "nuget:?package=NUnit.ConsoleRunner&version=3.10.0"
#addin nuget:?package=Cake.Json&version=3.0.1
#addin nuget:?package=Newtonsoft.Json&version=9.0.1
#addin nuget:?package=Microsoft.Management.Infrastructure&version=1.0.0
#addin "Cake.Services&version=0.3.5"
#addin "Cake.SqlTools&version=0.2.1"
#tool "nuget:?package=OpenCover&version=4.7.922"

#tool "nuget:?package=DependencyCheck.Runner.Tool&version=3.2.1&include=./**/dependency-check.sh&include=./**/dependency-check.bat"
#addin "nuget:?package=Cake.DependencyCheck&version=1.2.0"

#tool nuget:?package=MSBuild.SonarQube.Runner.Tool&version=4.6.0
#addin "nuget:?package=Cake.Sonar&version=1.1.18"

#addin nuget:?package=Cake.Coveralls&version=0.7.0
#tool nuget:?package=coveralls.net&version=0.7.0

#tool nuget:?package=Codecov&version=1.4.0
#addin nuget:?package=Cake.Codecov&version=0.5.0


//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug"); 
var verbosity = Argument("verbosity", "Normal");
var codequality = HasArgument("codequality");
var testcategory = Argument("testcategory", "");


///////////////////////////////////////////////////////////////////////////////
// LOAD
///////////////////////////////////////////////////////////////////////////////
#load "./scripts/load.cake"

Task("default")
	.Does(() =>
	{
		Information("Hello World!");
	});

Task("clean")
  .Does(() =>
  {
	Information("Cleaning project files");
    CleanDirectories(string.Format("./src/**/obj/{0}", configuration));
    CleanDirectories(string.Format("./src/**/bin/{0}", configuration));

	Information("Cleaning build files");
	CleanDirectories(reportsDir);
	CleanDirectories(artifactsDir);

	Information("Cleaning nuspec temp files");
	DeleteFiles("./nuspec/*.temp.nuspec");
  });

Task("restore-nuget-packages")
	.Does(() => NuGetRestore(solution));

Task("update-solution-info")
	.Does(() =>
	{
		var file = sourceDir + "SolutionInfo.cs";

		CreateAssemblyInfo(file, new AssemblyInfoSettings {
			Version = version,
			FileVersion = fileVersion,
			InformationalVersion = informalVersion,
			Copyright = string.Format("Copyright (c) 2008 - {0}", DateTime.Now.Year)
});
	});

Task("build")
	.IsDependentOn("clean")
	.IsDependentOn("restore-nuget-packages")
	.IsDependentOn("update-solution-info")
	.Does(() =>
	{
		DotNetBuild(solution);
	});

Task("rebuild")
	.IsDependentOn("clean")
	.IsDependentOn("build");

Task("test")
	.IsDependentOn("run-unit-tests")
	.IsDependentOn("run-integration-tests");


Task("pack")
	.Does(() =>
	{
		
	});

Task("deploy")
	.Does(() =>
	{
		
	});

Task("view-gitversion")
	.Does(() =>
	{
		GitVersion gitVersionResults = GitVersion(new GitVersionSettings());
    	Verbose(gitVersionResults.Dump());
	});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////
RunTarget(target);