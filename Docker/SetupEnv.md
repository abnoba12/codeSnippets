# Docker
Docker will be the primary technology used for Continuous Deployment. Docker comes with much greater challenges when running on windows. Instead of Windows we are going to run a Linux virtual machine for our Docker instances.

## Setting up local Docker environment 
1.	Download and install [ViruralBox](https://www.virtualbox.org/wiki/Downloads) for your OS
1.	Install docker-machine (Two Options)
	1.	Download the exe https://github.com/docker/machine/releases and add it to your windows path manually.
	1.	Easy way, but lots of unneeded extras: https://www.docker.com/docker-toolbox
1.	Create a new Vitural Machine 

	```docker-machine create -d virtualbox --virtualbox-memory "4096" --virtualbox-cpu-count "4" DockerVM``` 

	**NOTE:** I use GITBASH for these commands. Windows command prompt can have formatting issues.
1.	SSH into the DockerVM 

	```docker-machine ssh DockerVM```
1.	Install docker-compose
	
	```shell
	sudo -s
	sudo printf "curl -L https://github.com/docker/compose/releases/download/1.11.1/docker-compose-`uname -s`-`uname -m` | sudo tee /usr/local/bin/docker-compose > /dev/null \nsudo chmod +x /usr/local/bin/docker-compose" > /var/lib/boot2docker/bootlocal.sh
	reboot
	```
	
    Make sure you use the most recent version (mine was 1.11.1) https://github.com/docker/compose/releases

## Running Docker Containers 
### Local 
1. SSH into the DockerVM ```docker-machine ssh DockerVM```
1. Run your project in docker
	1. Open your project. Folders under the Users folder are shared and accessable in the DockerVM by default. 
	
	```cd /c/Users/IBM_ADMIN/Documents/GIT/{project}```
    1.	```docker-compose up``` This will pull down everything that is needed for this project to run in a Docker container and run it.		
		1. If you are running a SQL database, then you will need to run the bp3.sql script on the MSSQL instance to create your database and tables before you can execute the code.
		1. If you are running SQL the default username is *sa* and password is *yourStrong(!)Password*
    1.	Open your ports. Any port you want to access from the host machine you will need to open manually in VituralBox. 
        1.	Open ViruralBox --> RancherOS --> Settings --> Network --> Advanced --> Port Forwarding --> Name: API, Protocol: TCP, Host Port: 9090, Guest Port: 9090
        1.	```"C:\Program Files\Oracle\VirtualBox\VBoxManage.exe" modifyvm "DockerVM" --natpf1 "API,tcp,,9090,,9090"```
1.	On your host machine, outside of the DockerVM go to the following URL in a browser: {externalIp}:9090/{microserviceName}/api/v1/swagger

## Useful docker commands
* **Display All Docker containers:** ```sudo docker ps -a```
* **Display All Docker images:** ```sudo docker images```
* **Display All Docker volumes:** ```sudo docker volume ls```	
* **Delete all Docker containers:** ```sudo docker rm $(sudo docker ps -a -q)```
* **Delete all Docker images:** ```sudo docker rmi $(sudo docker images -q)```
* **Delete all Docker volumes:** ```sudo docker volume rm $(sudo docker volume ls -qf dangling=true)```
* **Open shell inside of a container:** ```sudo docker exec -i -t CONTAINER_NAME /bin/bash```
* **Force a Fresh Build:** ```sudo docker-compose rm && sudo docker-compose pull && sudo docker-compose build --no-cache```
* **Start up Compose:** ```sudo docker-compose up -d --force-recreate```
* **Start a node build machine: **```docker run --name nodeBuild -v /c/:/c/ -t -i node:latest /bin/bash```
	**NOTE:** You must use the /c/ shared folder otherwise symlinks will not work. npm install uses symlinks.

**NOTE:** Anything you want exposed outside of a docker container must be bound to IP 0.0.0.0 otherwise it will not work.