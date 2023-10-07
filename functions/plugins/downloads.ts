import { getDownloadCount } from '../lib/downloadCount.ts'

import type { Env } from '../types/env.ts'

export const onRequest: PagesFunction<Env> = async (context) => {
  const { keys } = await context.env.KV.list()

  const promises = Array.from(keys.values()).map(async ({ name }) =>
    getDownloadCount(context.env.KV, name).then((count) => [name, count] as [string, number])
  )
  const result = await Promise.all(promises)
  const payload: Record<string, number> = result.reduce((acc, [name, count]) => ({ ...acc, [name]: count }), {})

  return Response.json(payload)
}
