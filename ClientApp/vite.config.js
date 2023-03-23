import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import mkcert from 'vite-plugin-mkcert'
import path from 'path'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue(), mkcert()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src')
    }
  },
  server: {
    port: 3000,
    https: true,
    strictPort: true,
    proxy: {
      '/api': {
        target: 'https://localhost:5000',
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path.replace(/^\/api/, '/api'),
      },
    },
  },
})