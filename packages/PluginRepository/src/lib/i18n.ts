import i18n from 'i18next'
import languageDetector from 'i18next-browser-languagedetector'
import { initReactI18next } from 'react-i18next'

import { english, japanese } from './translations.ts'

// eslint-disable-next-line import/no-named-as-default-member
i18n
  .use(initReactI18next)
  .use(languageDetector)
  .init({
    resources: {
      en: {
        translation: english,
      },
      ja: {
        translation: japanese,
      },
    },
    fallbackLng: 'en',
    interpolation: {
      escapeValue: false,
    },
  })
  .catch(console.error)

export { i18n }
