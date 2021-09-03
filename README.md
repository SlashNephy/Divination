# Dalamud.Divination.Template

📝 Template repository for Dalamud.Divination Plugins

## How to use

0. Click "Use this template" on GitHub.
1. Open the solution, then rename "Divination.Template" project in JetBrains Rider.
2. Replace all "Template" with the project name.
3. Update the Common lib as desired.
    ```shell
    $ git submodule foreach git pull origin master
    ```
4. Edit `.github/workflows/build.yml` and put `GH_PAT` (with "repo:public_repo" scope) in repository secrets setting.
5. Start coding!

## Plugin Repository

This repository will be automatically built and published to the [plugin repository](https://github.com/SlashNephy/Dalamud.DivinationPluginRepo).

Plugins are published to the stable repository when a release tag is created, and to the testing repository when a commit is pushed to the `master` branch.
