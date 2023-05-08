import useSWR from 'swr'

export type PluginManifest = {
  /* eslint-disable @typescript-eslint/naming-convention */
  Name: string
  InternalName: string
  Punchline: string
  Description: string
  RepoUrl: string
  CategoryTags: string[]
  Tags: string[]
  DalamudApiLevel: number
  Author: string
  DownloadLinkInstall: string
  DownloadLinkTesting: string
  AssemblyVersion: string
  TestingAssemblyVersion: string
  /* eslint-enable @typescript-eslint/naming-convention */
}

export function usePluginMaster(): PluginManifest[] {
  const { data } = useSWR<PluginManifest[]>(
    '/plugins/master.json',
    async (url) => fetch(url).then(async (res) => res.json()),
    { suspense: true, refreshInterval: 60000 }
  )

  return data ?? []
}
