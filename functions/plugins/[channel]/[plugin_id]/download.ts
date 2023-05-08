import type { PagesEnv } from '../../../types/env'

export const onRequest: PagesFunction<PagesEnv, 'channel' | 'plugin_id'> = async (context) => {
  const channel = context.params.channel as string
  const pluginId = context.params.plugin_id as string

  const redirectUrl = context.request.url.replace(
    `/plugins/${channel}/${pluginId}/download`,
    `/plugins/${channel}/${pluginId}/latest.zip`
  )
  if (!(await checkRemoteResource(redirectUrl))) {
    return new Response(null, { status: 404 })
  }

  await incrementDownloadCount(context.env.KV, pluginId)

  return Response.redirect(redirectUrl, 302)
}

const checkRemoteResource = async (url: string): Promise<boolean> => {
  try {
    const response = await fetch(url)
    return response.status >= 200 && response.status < 300
  } catch (e: unknown) {
    return false
  }
}

const getDownloadCount = async (kv: KVNamespace, pluginId: string): Promise<number> => {
  const countRaw = await kv.get(pluginId)
  if (countRaw === null) {
    return 0
  }

  const count = parseInt(countRaw, 10)
  return Number.isNaN(count) ? 0 : count
}

const incrementDownloadCount = async (kv: KVNamespace, pluginId: string): Promise<void> => {
  const count = await getDownloadCount(kv, pluginId)
  await kv.put(pluginId, (count + 1).toString(10))
}
