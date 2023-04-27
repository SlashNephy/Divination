#!/usr/bin/env zsh

set -eux

REPOSITORY_OWNER="SlashNephy"
MIGRATE_TO="packages/plugins"
REPOSITORIES=(
  "xxx"
)

for repo in "${REPOSITORIES[@]}"
do
  old_repo_dir="/tmp/${repo}"
  if [ ! -d "${old_repo_dir}" ]
  then
    git clone "git@github.com:${REPOSITORY_OWNER}/${repo}.git" "${old_repo_dir}"
  fi

  mkdir -p "${MIGRATE_TO}/${repo}"

  git remote add "migrate-${repo}" "${old_repo_dir}"
  git fetch "migrate-${repo}"

  git merge -s ours --no-commit --allow-unrelated-histories "migrate-${repo}/master"
  git read-tree --prefix="${MIGRATE_TO}/${repo}/" -u "migrate-${repo}/master"
  git commit -m "refactor(${repo}): migrate to monorepo"

  git remote remove "migrate-${repo}"
  rm -rf "${old_repo_dir}"
done
