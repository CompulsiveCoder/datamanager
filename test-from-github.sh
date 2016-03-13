BRANCH=$1

if [ -z "$BRANCH" ]; then
    BRANCH="master"
fi

echo "Branch: $BRANCH"

git clone https://github.com/CompulsiveCoder/datamanager.git --branch $BRANCH
cd datamanager
sh init-build-test.sh
