import react from '@vitejs/plugin-react-swc'
import { defineConfig } from 'vite'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/plugins/master.json': {
        target: 'https://xiv.starry.blue',
        changeOrigin: true,
      },
    },
  },
})
