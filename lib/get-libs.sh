NUGET_FILE="nuget.exe"
 
if [ ! -f "$NUGET_FILE" ];
then
    wget http://nuget.org/nuget.exe
fi

mono nuget.exe install sider
mono nuget.exe install newtonsoft.json
mono nuget.exe install nunit -version 2.6.4
mono nuget.exe install nunit.runners -version 2.6.4
