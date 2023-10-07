import type { PagesEnv } from '../types/env'

export const onRequest: PagesFunction<PagesEnv> = async (context) => {
  const { keys } = await context.env.KV.list()

  const promises = Array.from(keys.values()).map(async ({ name }) =>
    getDownloadCount(context.env.KV, name).then((count) => [name, count] as [string, number])
  )
  const result = await Promise.all(promises)
  const payload: Record<string, number> = result.reduce((acc, [name, count]) => ({ ...acc, [name]: count }), {})

  return Response.json(payload)
}

const getDownloadCount = async (kv: KVNamespace, pluginId: string): Promise<number> => {
  const countRaw = await kv.get(pluginId)
  if (countRaw === null) {
    return 0
  }

  const count = parseInt(countRaw, 10)

  return Number.isNaN(count) ? 0 : count
}
