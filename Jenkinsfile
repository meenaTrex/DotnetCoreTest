pipeline
{
	agent any
	environment
	{
		scannerHome = tool name: 'sonar_scanner', type: 'hudson.plugins.sonar.MsBuildSQRunnerInstallation'
	}
	options
	{
		//skipDefaultCheckout()
		buildDiscarder(logRotator(daysToKeepStr: '10',numToKeepStr : '5'))
		disableConcurrentBuilds()
	}
	stages
	{
		stage('checkout')
		{
			steps
			{
				echo "checkout in master branch"
				checkout scm
			}
		}
		stage('restore')
		{
			steps
			{
				echo "restore in master branch"
				bat "dotnet restore"
			}
		}
		
		stage('Start Sonar Analysis')
		{
			steps
			{
				withSonarQubeEnv('test-sonar')
				{
					bat "dotnet C:\Program%Files%(x86)\Jenkins\tools\hudson.plugins.sonar.MsBuildSQRunnerInstallation\sonar_scanner/SonarScanner.MSBuild.dll begin /k:pipeline-dotnet-test /n:pipeline-dotnet-test /v:1.0"
				}
			}
		}
		stage('build')
		{
			steps
			{
				echo "build in master branch"
				bat "dotnet build"
			}
		}
		stage('End Sonar Analysis')
		{
			steps
			{
				withSonarQubeEnv('test-sonar')
				{
					bat "dotnet ${scannerHome}/SonarScanner.MSBuild.dll end"
				}
			}
		}
	}
}
