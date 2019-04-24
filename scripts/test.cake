
Task("run-unit-tests")
	.Does(() =>
	{
		var resultsFile = reportsDir + "NDbUnit.UnitTests" + ".nunit.xml";
		var coverageFile = reportsDir + "NDbUnit.UnitTests" + ".coverage.xml";

		NUnit3(testsDir + "NDbUnit.UnitTests/bin/" + configuration + "/NDbUnit.UnitTests.dll", 
				new NUnit3Settings {
					Configuration = configuration,
					NoResults = false,
					StopOnError = isLocal, //break the build if tests fail locally
					Results = new[] { new NUnit3Result{ FileName =  resultsFile} }
				});

		if(AppVeyor.IsRunningOnAppVeyor)
    	{
        	AppVeyor.UploadTestResults(resultsFile, AppVeyorTestResultsType.NUnit3);
    	}

		//calculate coverage
		OpenCover(tool => {
			tool.NUnit3(testsDir + "NDbUnit.UnitTests/bin/" + configuration + "/NDbUnit.UnitTests.dll", 
				new NUnit3Settings {
				ShadowCopy = false
				});
			},
			new FilePath(coverageFile),
			new OpenCoverSettings()
				.WithFilter("+[NDbUnit]*")
				.WithFilter("-[NDbUnit.UnitTests]*"));
	});

Task("setup-integration-tests")
	.Does(() =>
{
	if(productSettings == null)
	{
		throw new Exception("No settings available. Integration tests cannot be executed.");
	}
    var itSettings = productSettings.IntegrationTests;
    
	if(itSettings.Databases != null && itSettings.Databases.Count > 0)
	{
		var testsConfigFile = File(testsDir + @"NDbUnit.Test/app.config");

		foreach (var database in itSettings.Databases.Where(d => d.Enabled))
		{
			//skip if database platform does not match the current platform on which the build executes
			if(database.OS == OSType.Win && !IsRunningOnWindows())
			{
				continue;
			}

			//if on windows, start the service
			if(database.OS == OSType.Win && IsServiceStopped(database.ServiceName)) 
			{
				StartService(database.ServiceName);
			}

			//setup connection and provider
			SqlQuerySettings sqs = new SqlQuerySettings
			{
				Provider = dbProviders[database.DbType],
				//NOTE: for connection string: 0 - server, 1 - user, 2 - passwd, 3 - port, 4 - database (Database="master";)
				ConnectionString = string.Format(connectionStringTemplates[database.DbType],
												database.Server,
												database.Username,
												database.Password,
												database.Port,
												"")
			};

			//run script
			if(database.DbType == "postgresql")
			{
				//NOTE: postgres won't support a CREATE DATABASE call inside a larger script, so CREATE has to be its own invocation
				ExecuteSqlQuery("CREATE DATABASE testdb;", sqs);
				sqs.ConnectionString += "Database=testdb;";
			}

			ExecuteSqlFile(testsDir + @"NDbUnit.Test\Scripts\" + database.DbType + "-testdb-create.sql", sqs);

			//if on windows stop service
			if(database.OS == OSType.Win && IsServiceRunning(database.ServiceName))
			{
				StopService(database.ServiceName);
			}

			//configure connection strings in integration tests config file
			if(database.DbType == "sqlserver")
			{
				XmlPoke(testsConfigFile, "/configuration/connectionStrings/add[@key = 'SqlConnectionString']/@value", sqs.ConnectionString + "Database=testdb;");
				XmlPoke(testsConfigFile, "/configuration/connectionStrings/add[@key = 'SqlScriptTestsConnectionString']/@value", sqs.ConnectionString + "Database=master;");
				XmlPoke(testsConfigFile, "/configuration/connectionStrings/add[@key = 'OleDbConnectionString']/@value", sqs.ConnectionString + "Database=testdb;Provider=SQLOLEDB;");
			}
			else if(database.DbType == "mysql")
			{
				XmlPoke(testsConfigFile, "/configuration/connectionStrings/add[@key = 'MysqlConnectionString']/@value", sqs.ConnectionString + "Database=testdb;");
			}
			else if(database.DbType == "postgresql")
			{
				XmlPoke(testsConfigFile, "/configuration/connectionStrings/add[@key = 'MysqlConnectionString']/@value", sqs.ConnectionString);
			}
		}
	}
});

Task("run-integration-tests")
	.IsDependentOn("setup-integration-tests")
	.Does(() =>
	{
		var resultsFile = reportsDir + "NDbUnit.Test" + ".nunit.xml";
		var coverageFile = reportsDir + "NDbUnit.Test" + ".coverage.xml";

		NUnit3(testsDir + "NDbUnit.Test/bin/" + configuration + "/NDbUnit.Test.dll", 
				new NUnit3Settings {
					Configuration = configuration,
					NoResults = false,
					StopOnError = isLocal, //break the build if tests fail locally
					Results = new[] { new NUnit3Result{ FileName =  resultsFile} }
				});

		if(AppVeyor.IsRunningOnAppVeyor)
    	{
        	AppVeyor.UploadTestResults(resultsFile, AppVeyorTestResultsType.NUnit3);
    	}

		//calculate coverage
		OpenCover(tool => {
			tool.NUnit3(testsDir + "NDbUnit.Test/bin/" + configuration + "/NDbUnit.Test.dll",
				new NUnit3Settings {
				ShadowCopy = false
				});
			},
			new FilePath(coverageFile),
			new OpenCoverSettings()
				.WithFilter("+[NDbUnit]*")
				.WithFilter("-[NDbUnit.Test]*"));
    });