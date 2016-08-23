BRANCH=$1

if [ -z "$BRANCH" ]; then
    BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
fi

if [ -z "$BRANCH" ]; then
    BRANCH="master"
fi

# The 'ubuntu-mono' image is being used instead of 'ubuntu-mono-redis' because the redis client is mocked during testing so the server isn't required
docker run -it -v $PWD:/datamanager-src compulsivecoder/ubuntu-mono /bin/bash -c "git clone /datamanager-src /datamanager-dest/ && cd /datamanager-dest/ && sh init.sh && sh build-and-test-all.sh $BRANCH"
