#sudo add-apt-repository -y ppa:chris-lea/redis-server

sudo apt-get update
sudo apt-get install -y git wget mono-complete redis-server

mozroots --import --sync
