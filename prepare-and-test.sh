echo "Preparing to test damanager project"
echo "  Dir: $PWD"


TEST_CATEGORY=$1

if [ -z "$TEST_CATEGORY" ]; then
    TEST_CATEGORY="Unit"
fi

echo "Tests: $TEST_CATEGORY"

sh prepare.sh && \
sh init.sh && \
sh build.sh && \
nohup sudo redis-server & sh test.sh $TEST_CATEGORY
