echo "Preparing damanager project"
echo "  Dir: $PWD"

sudo apt-get update
sudo apt-get install -y git wget mono-complete # redis-server
# redis-server install disabled. It's not needed for testing because the redis client is mocked. It's up the the end user to decide whether or not to install/use it
