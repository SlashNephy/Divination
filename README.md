# Dalamud.Divination.Template

[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/horoscope-dev/Dalamud.Divination.Template/CI?style=flat-square)](https://github.com/horoscope-dev/Dalamud.Divination.Template/actions/workflows/ci.yml)

A template for developing Divination projects.

## How to use

1. Click "Use this template" on GitHub.
2. Open the solution, and rename `Divination.Template` to your preferred name.
3. Replace all `Template` strings with your project name.
4. Edit `.github/workflows/ci.yml`.
5. Issue GitHub PAT (Private Access Token), and add it to the repository secrets
   with the name `GH_PAT`. Please note that you must issue a PAT with write access to the plugin repository as described
   below.
6. Start coding!

## Distribution

This repository is automatically built by GitHub Actions and published to the plugin repository (default:
[Dalamud.DivinationPluginRepo](https://github.com/horoscope-dev/Dalamud.DivinationPluginRepo)).

When a release tag is created, it is published to the stable repository, and when a push to `master` branch is made, it is
published to the testing repository.
