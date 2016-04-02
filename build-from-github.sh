BRANCH=$1

DIR=$PWD

if [ -z "$BRANCH" ]; then
    BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
fi

if [ -z "$BRANCH" ]; then
    BRANCH="master"
fi

echo "Branch: $BRANCH"

# If the .tmp/datamanager directory exists then remove it
if [ -d ".tmp/datamanager" ]; then
    rm .tmp/datamanager -rf
fi

git clone https://github.com/CompulsiveCoder/datamanager.git .tmp/datamanager --branch $BRANCH
cd .tmp/datamanager && \
sh init-build.sh && \
cd $DIR && \
rm .tmp/datamanager -rf
