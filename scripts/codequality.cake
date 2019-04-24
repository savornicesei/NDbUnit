Task("sonar-begin")
	.Does(() => {
		SonarBegin(new SonarBeginSettings{
			//Supported parameters
			Key = "ndbunit2",
			Url = "sonarcube.contoso.local",
			Login = "admin",
			Password = "admin",
			Verbose = true,
			//Custom parameters
			ArgumentCustomization = args => args
				.Append("/d:sonar.gitlab.project_id=XXXX")
				.Append("/d:sonar.gitlab.xxx=XXXX")
		});
	});

Task("sonar-end")
	.Does(() => {
		SonarEnd(new SonarEndSettings{
			Login = "admin",
			Password = "admin"
		});
	});

Task("analyze-codequality")
	//.Criteria()
	.IsDependentOn("sonar-begin")
	.IsDependentOn("build")
	.IsDependentOn("test")
	.IsDependentOn("sonar-end")
	.Does(() =>
	{
		//check dependency
		DependencyCheck(new DependencyCheckSettings
        {
            Project = product,
            Scan = sourceDir + "*",
            Format = "HTML"
        });

		//do a sonarqube analysis


		//push to Coveralls
		CoverallsNet("coverage.xml", CoverallsNetReportType.OpenCover, new CoverallsNetSettings()
		{
			RepoToken = "abcdef"
		});

		//push to Codecov
		var buildVersion = string.Format("{0}.build.{1}", fullSemVer, BuildSystem.AppVeyor.Environment.Build.Version);
		Codecov(new CodecovSettings {
			Files = new[] { "coverage1.xml", "coverage2.xml" },
			Token = "00000000-0000-0000-0000-000000000000",
			Flags = "ut", //unittests, integration or ut,chrome for UI tests
			EnvironmentVariables = new Dictionary<string,string> { { "APPVEYOR_BUILD_VERSION", buildVersion } },
			Verbose = true
		});
	});