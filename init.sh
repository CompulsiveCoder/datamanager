echo "Initializing datamanager project"
echo "  Dir: $PWD"

PARENT_DIR=../..
PARENT_LIB_DIR=$PARENT_DIR/lib

if [ -f $PARENT_LIB_DIR/nuget.exe ]; then
   echo "Copying libs from parent projects"
   cp $PARENT_LIB_DIR/* lib -v
fi

DIR=$PWD

cd lib
sh get-libs.sh
cd $DIR
