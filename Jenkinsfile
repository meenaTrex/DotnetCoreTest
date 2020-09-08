pipeline
{
	agent any
	environment
	{
		scannerHome = tool name: 'sonar_scanner_dotnet', type: 'hudson.plugins.sonar.MsBuildSQRunnerInstallation'
		dockerUsername 
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
				echo env.docker_username
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
				withSonarQubeEnv('Test_Sonar')
				{
					bat 'dotnet "C:/Program Files (x86)/Jenkins/tools/hudson.plugins.sonar.MsBuildSQRunnerInstallation/sonar_scanner/SonarScanner.MSBuild.dll" begin /k:pipeline-dotnet-test /n:pipeline-dotnet-test /v:1.0'
					//bat "dotnet ${scannerHome}/SonarScanner"
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
					bat 'dotnet "C:/Program Files (x86)/Jenkins/tools/hudson.plugins.sonar.MsBuildSQRunnerInstallation/sonar_scanner/SonarScanner.MSBuild.dll" end'
				}
			}
		}
		stage('Docker Image')
		{
			steps
			{
				bat "docker build -t meenakshi23/dotnetcoretest:${BUILD_NUMBER} --no-cache -f ./dotnet/Dockerfile ./dotnet"
				
			}
		}
		stage('Push Docker Image')
		{
			steps
			{
				//bat "docker tag dotnetcoretest:${BUILD_NUMBER} meenakshi23/dotnetcoretest:${BUILD_NUMBER}"
				//bat "docker push docker.io/meenakshi23/dotnetcoretest:${BUILD_NUMBER}"
				withDockerRegistry([ credentialsId: "49cb0b01-e5b6-4d94-9fb1-987521ee9465", url: "" ]) {
					bat "docker push docker.io/meenakshi23/dotnetcoretest:${BUILD_NUMBER}"
				}
				
			}
		}
		stage('Remove container')
		{
			steps
			{
				//bat "powershell.exe $containerId = docker container ls -aq --filter=name=test${BUILD_NUMBER}; docker stop $ContainerId ;docker rm $ContainerId"
				//bat "FOR /F %a IN ('docker ps -a^| findstr 2341') DO docker rm -f  %a"
				powershell label: '', script: '''$containerId = docker container ls -aq --filter=name=test${BUILD_NUMBER}
				docker stop $ContainerId
				docker rm $ContainerId'''
			}
		}
		stage('Docker deployment')
		{
			steps
			{
				bat "docker run --name test${BUILD_NUMBER} -d -p 2341:8080 meenakshi23/dotnetcoretest:${BUILD_NUMBER}"
			}
		}
		
	}
}
