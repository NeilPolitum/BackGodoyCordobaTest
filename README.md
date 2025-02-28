Para correr el programa correctamente, se necesita lo siguiente:

instalar .NET trabajando desde ubuntu 22.04
	sudo add-apt-repository ppa:dotnet/backports
	sudo apt-get update
	sudo apt-get install -y dotnet-sdk-9.0

instalar mongo trabajando desde ubuntu 22.04:
	wget -qO - https://www.mongodb.org/static/pgp/server-6.0.asc | sudo apt-key add -
	echo "deb [ arch=amd64,arm64 ] https://repo.mongodb.org/apt/ubuntu jammy/mongodb-org/6.0 multiverse" | sudo tee /etc/apt/sources.list.d/mongodb-org-6.0.list
	sudo apt-get update
	sudo apt-get install -y mongodb-org
	sudo systemctl start mongod
	sudo systemctl enable mongod
