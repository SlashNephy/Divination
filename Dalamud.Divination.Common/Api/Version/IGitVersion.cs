namespace Dalamud.Divination.Common.Api.Version
{
    public interface IGitVersion
    {
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public string PreReleaseTag { get; }
        public string PreReleaseTagWithDash { get; }
        public string PreReleaseLabel { get; }
        public string PreReleaseNumber { get; }
        public int WeightedPreReleaseNumber { get; }
        public int BuildMetaData { get; }
        public string BuildMetaDataPadded { get; }
        public string FullBuildMetaData { get; }
        public string MajorMinorPatch { get; }
        public string SemVer { get; }
        public string LegacySemVer { get; }
        public string LegacySemVerPadded { get; }
        public string AssemblySemVer { get; }
        public string AssemblySemFileVer { get; }
        public string FullSemVer { get; }
        public string InformationalVersion { get; }
        public string BranchName { get; }
        public string EscapedBranchName { get; }
        public string Sha { get; }
        public string ShortSha { get; }
        public string NuGetVersionV2 { get; }
        public string NuGetVersion { get; }
        public string NuGetPreReleaseTagV2 { get; }
        public string NuGetPreReleaseTag { get; }
        public string VersionSourceSha { get; }
        public int CommitsSinceVersionSource { get; }
        public string CommitsSinceVersionSourcePadded { get; }
        public string CommitDate { get; }
        public int UncommittedChanges { get; }

        /// <summary>
        /// リフレクションを用いて, すべてのバージョン情報の文字列を返します。
        /// </summary>
        public string ToString();
    }
}
