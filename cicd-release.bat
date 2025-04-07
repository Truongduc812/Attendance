git fetch --all
git stash --include-untracked
git reset --hard
git clean -fd
git checkout main
git pull origin main
git checkout release
git merge main
git push origin release
