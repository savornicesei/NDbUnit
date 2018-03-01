<# 
.SYNOPSIS
     Prepares env. (win or linux or mac) for building NDbUnit2
.DESCRIPTION
     Prepares the build environment by:
     - downloading NUnit console runner
     - downloading OpenCover
     - downloading coveralls.net
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
import-module PackageManagement

function Build-Setup{
	param
    (
		[Parameter(Mandatory = $true, HelpMessage = "Some dummy parameter.")]
        [String]
		$Name
	)

	


}