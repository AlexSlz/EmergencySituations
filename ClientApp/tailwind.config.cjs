/** @type {import('tailwindcss').Config} */

module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        'myMain': '#262626',
        'mySecond': '#404040',


        'myElement': '#363f7e',
        'myActive': '#788DF9',
        'mySecondActive': '#0a0a0a',
        'myRed': '#f43f5e',


        'myText': '#fafafa',
        'mySecondText': '#a3a3a3',

      }
    },
  },
  plugins: [
    require('@tailwindcss/line-clamp'),
  ],
}
