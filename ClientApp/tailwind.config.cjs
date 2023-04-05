/** @type {import('tailwindcss').Config} */

module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        'myMain': '#f5f3ff',
        'myActive': '#0ea5e9',
        'myRed': '#f43f5e',

        'myText': '#fafafa',
        'mySecondText': '#a3a3a3',

        'myBG': '#262626',
        'mySecond': '#404040',
        'mySecondActive': '#525252'
      }
    },
  },
  plugins: [
    require('@tailwindcss/line-clamp'),
  ],
}
