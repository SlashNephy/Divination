using System;
using System.Reflection;

namespace Dalamud.Divination.Common
{
    public class GitVersion
    {
        private readonly Type? gitVersionInfo;

        public GitVersion()
        {
            try
            {
                gitVersionInfo = Assembly.GetExecutingAssembly().GetType("GitVersionInformation");
            }
            catch
            {
                gitVersionInfo = null;
            }
        }

        public int Major => GetIntField("Major");
        public int Minor => GetIntField("Minor");
        public int Patch => GetIntField("Patch");
        public string PreReleaseTag => GetStringField("PreReleaseTag");
        public string PreReleaseTagWithDash => GetStringField("PreReleaseTagWithDash");
        public string PreReleaseLabel => GetStringField("PreReleaseLabel");
        public string PreReleaseNumber => GetStringField("PreReleaseNumber");
        public int WeightedPreReleaseNumber => GetIntField("WeightedPreReleaseNumber");
        public int BuildMetaData => GetIntField("BuildMetaData");
        public string BuildMetaDataPadded => GetStringField("BuildMetaDataPadded");
        public string FullBuildMetaData => GetStringField("FullBuildMetaData");
        public string MajorMinorPatch => GetStringField("MajorMinorPatch");
        public string SemVer => GetStringField("SemVer");
        public string LegacySemVer => GetStringField("LegacySemVer");
        public string LegacySemVerPadded => GetStringField("LegacySemVerPadded");
        public string AssemblySemVer => GetStringField("AssemblySemVer");
        public string AssemblySemFileVer => GetStringField("AssemblySemFileVer");
        public string FullSemVer => GetStringField("FullSemVer");
        public string InformationalVersion => GetStringField("InformationalVersion");
        public string BranchName => GetStringField("BranchName");
        public string EscapedBranchName => GetStringField("EscapedBranchName");
        public string Sha => GetStringField("Sha");
        public string ShortSha => GetStringField("ShortSha");
        public string NuGetVersionV2 => GetStringField("NuGetVersionV2");
        public string NuGetVersion => GetStringField("NuGetVersion");
        public string NuGetPreReleaseTagV2 => GetStringField("NuGetPreReleaseTagV2");
        public string NuGetPreReleaseTag => GetStringField("NuGetPreReleaseTag");
        public string VersionSourceSha => GetStringField("VersionSourceSha");
        public int CommitsSinceVersionSource => GetIntField("CommitsSinceVersionSource");
        public string CommitsSinceVersionSourcePadded => GetStringField("CommitsSinceVersionSourcePadded");
        public string CommitDate => GetStringField("CommitDate");
        public int UncommittedChanges => GetIntField("UncommittedChanges");

        private int GetIntField(string key)
        {
            try
            {
                return (int) gitVersionInfo!.GetField(key).GetValue(null);
            }
            catch
            {
                return default;
            }
        }

        private string GetStringField(string key)
        {
            try
            {
                return (string) gitVersionInfo!.GetField(key).GetValue(null);
            }
            catch
            {
                return string.Empty;
            }
        }

        public void Dump()
        {
            if (gitVersionInfo == null)
            {
                return;
            }

            foreach (var field in gitVersionInfo.GetFields())
            {
                Console.WriteLine($"{field.Name}: {field.GetValue(null)}");
            }
        }
    }
}
