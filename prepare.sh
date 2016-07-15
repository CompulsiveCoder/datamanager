echo "Preparing damanager project"
echo "  Dir: $PWD"

sudo apt-get update
sudo apt-get install -y git wget mono-complete redis-server

# Disabled because it breaks travis CI process # TODO Remove if not needed
#mozroots --import --sync
