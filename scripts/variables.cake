//////////////////////////////////////////////////////////////////////
// VARIABLES
//////////////////////////////////////////////////////////////////////
// Project details
var solution = "./NDbUnit.sln";

//Build details
var local = BuildSystem.IsLocalBuild;

// Get version.
var buildNumber = "";
var version = releaseNotes.Version.ToString();
var semVersion = local ? version : (version + string.Concat("-build-", buildNumber));