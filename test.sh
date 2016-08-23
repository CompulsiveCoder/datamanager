CATEGORY=$1

if [ -z "$CATEGORY" ]; then
    CATEGORY="Unit"
fi

echo "Testing datamanager project"
echo "  Dir: $PWD"
echo "  Category: $CATEGORY"

# TODO: Remove if not needed. Should redis be started here? Or within the test setup?
#if [ "Integration" = "$CATEGORY" ]; then
#  nohup sudo redis-server & # This ampersand is causing problems; redis runs in the background and hangs the terminal window
#fi

mono lib/NUnit.Runners.2.6.4/tools/nunit-console.exe bin/Release/*.dll --include=$CATEGORY
