#Functionality

This program is designed to be used as an ERP program for a university or a school.

There are two use roles: 

1. Administrator
2. Student

Administrator can:

	1. CRUD Students
	2. CRUD Courses
	3. Get the list of all courses
	4. Get the list of all students
	5. Grade students' selected courses

Student can:

	1. Enroll to a course
	2. Get academic progress

#Requirements

	1. Docker Desktop
	2. Kubernetes

#Configuration

Launch app only with docker compose [ NOT THIS repository ]

	1. Clone reporistory from https://github.com/WillTissot/SchoolERPMultiContainerProject.git
	2. In the same directory insert the following command lines
		2.1 docker compose build
		2.2 docker compose up -d
	3. Access https://localhost:8002/ 
	4. Access http://localhost:8001/swagger for testing api purposes regarding students
	5. Access http://localhost:8003/swagger for testing api purposes regarding courses


Launch app with kubernetes

Clone repository from https://github.com/itp21116/schoolerpmulticontainerproject

Inside the same directory insert the following commands:
	
	1.minikube start
	2.kubectl create secret generic mssql-secret --from-literal=SA_PASSWORD="2Secure*Password2"
	3.kubectl apply -f sql.yaml
	4.kubectl apply -f school.yaml
	5.kubectl apply -f students.yaml
	6.kubectl apply -f courses.yaml
	7.kubectl port-forward schoolerp-mvc-ui-6f99c9849-89zhf 8002:8002

#Testing accounts

amdin@gmail.com
Admin!1!

marvin@gmail.com
Student!1!

nick@gmail.com
Student!2!
