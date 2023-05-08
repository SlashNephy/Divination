export const onRequest = async (context) => {
  const { channel } = context.params
  const pluginId = context.params.plugin_id
  const redirectUrl = `https://xiv.starry.blue/plugins/${channel}/${pluginId}/latest.zip`
  if (!(await checkRemoteResource(redirectUrl))) {
    return new Response(null, { status: 404 })
  }
  await incrementDownloadCount(context.env.KV, pluginId)
  return Response.redirect(redirectUrl, 302)
}
const checkRemoteResource = async (url) => {
  try {
    const response = await fetch(url)
    return response.status >= 200 && response.status < 300
  } catch (e) {
    return false
  }
}
const getDownloadCount = async (kv, pluginId) => {
  const countRaw = await kv.get(pluginId, 'text')
  if (countRaw === null) {
    return 0
  }
  const count = parseInt(countRaw, 10)
  return Number.isNaN(count) ? 0 : count
}
const incrementDownloadCount = async (kv, pluginId) => {
  const count = await getDownloadCount(kv, pluginId)
  await kv.put(pluginId, (count + 1).toString(10))
}
