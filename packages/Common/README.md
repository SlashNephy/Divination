# Dalamud.Divination.Common

[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/horoscope-dev/Dalamud.Divination.Common/CI?style=flat-square)](https://github.com/horoscope-dev/Dalamud.Divination.Common/actions/workflows/ci.yml)
[![nuget.org](https://img.shields.io/nuget/vpre/Dalamud.Divination.Common?style=flat-square)](https://www.nuget.org/packages/Dalamud.Divination.Common)

A shared library used by the Divination projects to provide boilerplate for Dalamud plugin and to complement Dalamud
APIs.

## Usage

```xml
<Project Sdk="Microsoft.NET.Sdk">
    <ItemGroup>
        <PackageReference Include="Dalamud.Divination.Common" Version="${Version}" />
    </ItemGroup>
</Project>

```

Please refer to the [Template](https://github.com/horoscope-dev/Dalamud.Divination.Template) repository for more
information.
