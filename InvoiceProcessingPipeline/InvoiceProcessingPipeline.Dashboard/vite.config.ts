import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from "@tailwindcss/vite"
//"http://localhost:7147"
// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  server: {
    open: "/home",
    proxy: {
      "/api": {
        target: "https://e776e412-4b7e-46ec-a7f1-86f9e08dc223.mock.pstmn.io",
        changeOrigin: true,
      }
    }
  },
})
