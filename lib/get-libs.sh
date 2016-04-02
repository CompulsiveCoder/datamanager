echo "Getting library files"
echo "  Dir: $PWD"

PARENT_DIR=../../..
PARENT_LIB_DIR=$PARENT_DIR/lib

echo "Parent libs directory: $PARENT_LIB_DIR"

if [ -f $PARENT_LIB_DIR/nuget.exe ]; then
   echo "Copying libs from parent projects"
   cp $PARENT_LIB_DIR/nuget.exe . -v
   cp $PARENT_LIB_DIR/**/* . -v
fi

NUGET_FILE="nuget.exe"

if [ ! -f "$NUGET_FILE" ];
then
    wget http://nuget.org/nuget.exe
fi

mono nuget.exe install newtonsoft.json -version 8.0.3
mono nuget.exe install nunit -version 2.6.4
mono nuget.exe install nunit.runners -version 2.6.4
mono nuget.exe install sider -version 0.10.0
