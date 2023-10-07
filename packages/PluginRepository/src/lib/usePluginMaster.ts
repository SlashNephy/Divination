import useSWR from 'swr'

export type PluginManifest = {
  AssemblyVersion: string
  Author: string
  CategoryTags: string[]
  DalamudApiLevel: number
  Description: string
  DownloadCount: number
  DownloadLinkInstall: string
  DownloadLinkTesting: string
  InternalName: string
  Name: string
  Punchline: string
  RepoUrl: string
  Tags: string[]
  TestingAssemblyVersion: string
}

export function usePluginMaster(): PluginManifest[] {
  const { data } = useSWR<PluginManifest[]>(
    '/plugins/master.json',
    async (url) => fetch(url).then(async (res) => res.json()),
    { suspense: true, refreshInterval: 60000 }
  )

  return data ?? []
}
