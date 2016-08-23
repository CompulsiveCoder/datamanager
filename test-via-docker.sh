#TEST_CATEGORY=$1

# TODO: Is this needed? Currently all tests are run, and the category is ignored. Remove or reimplement.
#if [ -z "$TEST_CATEGORY" ]; then
#    TEST_CATEGORY="Unit"
#fi

#echo "Tests: $TEST_CATEGORY"

sh build.sh

# The 'ubuntu-mono' image is being used instead of 'ubuntu-mono-redis' because the redis client is mocked during testing so the server isn't required
docker run -i -v /var/run/docker.sock:/var/run/docker.sock -v $PWD:/datamanager compulsivecoder/ubuntu-mono /bin/bash -c "cd /datamanager && sh test-all.sh" #$TEST_CATEGORY"
