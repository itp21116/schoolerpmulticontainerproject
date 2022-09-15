pipeline {
    agent any
    environment {
            DOCKER_TOKEN = credentials('docker-push-secret')
            DOCKER_USER = 'marvolo95'
            DOCKER_SERVER = 'ghcr.io'
            DOCKER_PREFIX = 'ghcr.io/marvolo95/SchoolERPMultiContainerProject'
        }
        


    stages {
        
        stage('test') {
            steps {
                sh '''
                    docker-compose kill -s SIGINT
                    docker-compose up -d --build
                    while ! docker-compose exec schoolerp.mvc.ui wget -S --spider https://localhost:8003/api/Course ; do sleep 1; done
                    docker-compose exec curl -X 'GET' 'https://localhost:8001/api/Student' -H 'accept: application/json'
                    docker-compose down --volumes
                '''
            }
        }        
        
        
        
        
         stage('docker build and push') {
           

            steps {
                sh '''
                    HEAD_COMMIT=$(git rev-parse --short HEAD)
                    TAG=$HEAD_COMMIT-$BUILD_ID
                    docker build --rm -t $DOCKER_PREFIX:$TAG -t $DOCKER_PREFIX:latest  -f SchoolERP.MVC.UI/Dockerfile .
                    docker build --rm -t $DOCKER_PREFIX:$TAG -t $DOCKER_PREFIX:latest  -f Courses.Services/Dockerfile .
                    docker build --rm -t $DOCKER_PREFIX:$TAG -t $DOCKER_PREFIX:latest  -f Students.Services/Dockerfile .
                '''

                
                sh '''
                    echo $DOCKER_TOKEN | docker login $DOCKER_SERVER -u $DOCKER_USER --password-stdin
                    docker push $DOCKER_PREFIX --all-tags
                '''
            }
         }

            stage('deploy to k8s') {
            steps {
                sh '''
                    HEAD_COMMIT=$(git rev-parse --short HEAD)
                    TAG=$HEAD_COMMIT-$BUILD_ID
                    kubectl config use-context microk8s
                    kubectl set image deployment/schoolerp-mvc-ui-deployment schoolerp-mvc-ui=$DOCKER_PREFIX:$TAG
                    RUNNING_TAG=$(kubectl get pods  -o=jsonpath="{.items[*].spec.containers[*].image}" -l component=schoolerp-mvc-ui | grep $TAG)
                    FOUND=$(echo $RUNNING_TAG | wc -l)
                    timeout --preserve-status 3m bash -c  -- "while [ $FOUND -eq  0 ] ; do echo \"waiting\"; sleep 1; done"
                '''
            }
        }
        
    }
    
    post{
	always{
		mail to:'itp21116@hua.gr', 
			subject: "Status of Pipeline: ${currentBuild.fullDisplayName}",
			body: "${env.BUILD_URL} has result ${surrentBuild.result}"
		}
	}

}