pipeline
{
	agent any
	environment
	{
		scannerHome = tool name: 'sonar_scanner', type: 'hudson.plugins.sonar.MsBuildSQRunnerInstallation' 
		kubeConfigPath = "C:/Users/meenakshithukral/.kube/config"
		helmConfigPath = "D:/setups/helm-v3.3.1-windows-amd64/windows-amd64"
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
				withSonarQubeEnv('Test_Sonar')
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
				withDockerRegistry([ credentialsId: "382f4956-57d3-425e-a2e0-b287402182db", url: "" ]) {
					bat "docker push docker.io/meenakshi23/dotnetcoretest:${BUILD_NUMBER}"
				}
				
			}
		}
		stage('Remove container')
		{
			steps
			{
				powershell label: '', script: '''$containerId = docker container ls -aq --filter=name=c_meenakshithukral_master
				if($containerId){
				docker stop $ContainerId
				docker rm $ContainerId
				}'''
			}
		}
		stage('Docker deployment')
		{
			steps
			{
				bat "docker run --name c_meenakshithukral_master -d -p 2342:8080 meenakshi23/dotnetcoretest:${BUILD_NUMBER}"
			}
		}
		stage('helm charts deployment')
		{
			steps
			{
				withEnv(["KUBECONFIG=${kubeConfigPath}", "helm=${helmConfigPath}"]){
					bat "helm install test-deployment test-chart-1 --set nodePort=30157 --set replicaCount=2 --set image.repository=meenakshi23/dotnetcoretest:${BUILD_NUMBER} --set image.pullPolicy=Always"
				}
			}
		}
	}
}
