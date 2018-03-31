<# 
.SYNOPSIS
     Prepares env. (win or linux or mac) for building NDbUnit2
.DESCRIPTION
     Prepares the build environment by:
     - downloading NUnit console runner
     - downloading OpenCover
     - downloading coveralls.net
     - downloading SonarQube MSBuild scanner
     The script is agnostic to the platform it is run on (windows, linux or mac)
.INPUTTYPE
   Documentary text, eg:
   Input type  [Universal.SolarSystem.Planetary.CommonSense]
   Appears in -full
.RETURNVALUE
   Documentary Text, eg:
   Output type  [Universal.SolarSystem.Planetary.Wisdom]
   Appears in -full
#>
Import-Module -Name OneGet
import-module PackageManagement

function Build-Setup{
	param
    (
		[Parameter(Mandatory = $true, HelpMessage = "Operating.")]
        [String]
		$Os
	)
	
}

function Check-Machine{
	$checkOk = true;
	
	if($PSVersionTable -eq $null )
	{
		$checkOk = false;
		Write-Host -ForegroundColor Red "Powershell version should be 6 or greater. Please upgrade!"
	}
	
	return $checkOk;
}

function Install-Choco{
	param
	{
		[Parameter(Mandatory = $true, HelpMessage = "Operating system (can be Unix, Windows or .")]
		[String]
		$Os
	}
}