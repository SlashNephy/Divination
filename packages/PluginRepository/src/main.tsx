import { NextUIProvider } from '@nextui-org/react'
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { I18nextProvider } from 'react-i18next'

import { App } from './App.tsx'
import { i18n } from './lib/i18n.ts'

const container = document.getElementById('root')
if (container !== null) {
  const root = createRoot(container)
  root.render(
    <StrictMode>
      <I18nextProvider i18n={i18n}>
        <NextUIProvider>
          <App />
        </NextUIProvider>
      </I18nextProvider>
    </StrictMode>
  )
}
