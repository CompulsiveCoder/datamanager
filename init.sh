echo "Initializing datamanager project"
echo "  Current directory:"
echo "  $PWD"


DIR=$PWD

cd lib
sh get-libs.sh
cd $DIR
