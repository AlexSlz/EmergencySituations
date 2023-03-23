/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        'purple': '#cdb4db',
        'pink': '#ffc8dd',
        'moPink': '#ffafcc',
        'blue': '#bde0fe',
        'moBlue': '#a2d2ff',
      }
    },
  },
  plugins: [
    require('@tailwindcss/line-clamp'),
  ],
}
