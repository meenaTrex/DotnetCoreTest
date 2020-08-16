pipeline
{
	agent any
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
		
		stage('build')
		{
			steps
			{
				echo "build in master branch"
				//bat "dotnet restore"
				bat "dotnet build"
			}
		}
		
	}
}
