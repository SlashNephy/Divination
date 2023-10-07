export async function getDownloadCount(kv: KVNamespace, pluginId: string): Promise<number> {
  // TODO: migrate to Cloudflare D1
  const countRaw = await kv.get(pluginId)
  if (countRaw === null) {
    return 0
  }

  const count = parseInt(countRaw, 10)

  return Number.isNaN(count) ? 0 : count
}

export async function incrementDownloadCount(kv: KVNamespace, pluginId: string): Promise<void> {
  const count = await getDownloadCount(kv, pluginId)
  await kv.put(pluginId, (count + 1).toString(10))
}
