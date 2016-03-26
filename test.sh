CATEGORY=$1

if [ -z "$CATEGORY" ]; then
    CATEGORY="Unit"
fi

echo "Testing ipfs-cs project"
echo "  Dir: $PWD"
echo "  Category: $CATEGORY"

nohup sudo redis-server & mono lib/NUnit.Runners.2.6.4/tools/nunit-console.exe bin/Release/*.dll --include=$CATEGORY
