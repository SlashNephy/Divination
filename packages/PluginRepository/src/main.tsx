import React from 'react'
import ReactDOM from 'react-dom/client'
import { I18nextProvider } from 'react-i18next'

import { App } from './App.tsx'
import { i18n } from './lib/i18n.ts'

const container = document.getElementById('root')
if (container !== null) {
  const root = ReactDOM.createRoot(container)
  root.render(
    <React.StrictMode>
      <I18nextProvider i18n={i18n}>
        <App />
      </I18nextProvider>
    </React.StrictMode>
  )
}
