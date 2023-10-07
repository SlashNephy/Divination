import { incrementDownloadCount } from '../../../lib/downloadCount.ts'

import type { Env } from '../../../types/env'

export const onRequest: PagesFunction<Env, 'channel' | 'plugin_id'> = async (context) => {
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

async function checkRemoteResource(url: string): Promise<boolean> {
  try {
    const response = await fetch(url)

    return response.status >= 200 && response.status < 300
  } catch (e: unknown) {
    return false
  }
}
