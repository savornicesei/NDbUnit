//////////////////////////////////////////////////////////////////////
// VARIABLES
//////////////////////////////////////////////////////////////////////
// Project details
var product = "NDbUnit2";
var solution = "./NDbUnit.sln";

//Build details
var isLocal = BuildSystem.IsLocalBuild;
var isAppVeyor = BuildSystem.IsRunningOnAppVeyor;
var isTravisCI = BuildSystem.IsRunningOnTravisCI;

// Get version.
var branch = "";
var version = "";
var fileVersion = "";
var informalVersion = "";
var nugetVersion = "";
int? buildNumber = null;
var fullSemVer = "";

//Define directories.
var sourceDir = "./src/";
var testsDir = "./test/";
var scriptsDir = "./scripts/";
var reportsDir = "./reports/";
var artifactsDir = "./artifacts/";

//Global variables
ProductSettings productSettings; //project settings for current CI server
Dictionary<string, string> dbProviders = new Dictionary<string, string>
{
    {"sqlserver", "MsSql"},
    {"mysql", "MySql"},
    {"postgresql", "Npgsql"},
    {"mariadb", "MySql"},
    {"oracle", ""}
};
Dictionary<string, string> connectionStringTemplates = new Dictionary<string, string>
{
    // 0 - server, 1 - user, 2 - passwd, 3 - port, 4 - database
    {"sqlserver", "Server={0};User Id={1};Password={2};{4}"},
    {"mysql", "Server={0};Port={3};Uid={1};Pwd={2};{4}"},
    {"postgresql", "User ID={1};Password={2};Host={0};Port={3};Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;{4}"}
};