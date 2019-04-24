///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

//NOTE: Executed BEFORE the first task.
Setup(context =>
{
    Information("Determine build environment");
    
    Information("Determine build version");
    GitVersion gitVersionResults = GitVersion(new GitVersionSettings());

    branch = gitVersionResults.BranchName;
    version = gitVersionResults.AssemblySemVer;
    fileVersion = gitVersionResults.AssemblySemFileVer;
    informalVersion = gitVersionResults.InformationalVersion;
    nugetVersion = gitVersionResults.NuGetVersion;
    buildNumber = gitVersionResults.PreReleaseNumber;
    fullSemVer = gitVersionResults.FullSemVer;

    //NOTE: load product settings for current CI
    Information("Determine product settings for current build environment");
    string settingsFile = string.Empty;
    if(isAppVeyor)
    {
        settingsFile = scriptsDir + "appveyor.settings.json";
        Information("Remote build on AppVeyor. Using " + settingsFile + " to load settings.");
    }
	else if(isTravisCI)
	{
		settingsFile = scriptsDir + "travisci.settings.json";
        Information("Remote build on TravisCI. Using " + settingsFile + " to load settings.");
	}
	else //assume it's local
	{
		settingsFile = scriptsDir + ".local.settings.json";
        Information("Local build. Using " + settingsFile + " to load settings.");
	};
    if(!FileExists(settingsFile))
    {
        throw new Exception("Settings file does not exist!");
    }
    productSettings = DeserializeJsonFromFile<ProductSettings>(settingsFile);

    Information("Building version {0} of {1}.", informalVersion, product);
    Information("Target: {0}.", target);
});

// NOTE: Executed AFTER the last task.
Teardown(context =>
{
    Information("Finished building version {0} of {1}.", informalVersion, product);
});