/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./*.html",           // все HTML файлы в корне
    "./js/*.js",          // JS файлы
  ],
  theme: {
    extend: {
      // Можете добавить кастомные цвета соответствующие вашей теме
      colors: {
        'coffee-brown': '#3E2723',
        'coffee-light': '#8D6E63',
        'coffee-cream': '#F9F5F0',
      }
    },
  },
  plugins: [],
}