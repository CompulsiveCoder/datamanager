BRANCH=$1

if [ -z "$BRANCH" ]; then
    BRANCH="master"
fi

curl https://raw.githubusercontent.com/CompulsiveCoder/datamanager/$BRANCH/prepare.sh | sh
curl https://raw.githubusercontent.com/CompulsiveCoder/datamanager/$BRANCH/test-from-github.sh | sh -s $BRANCH
