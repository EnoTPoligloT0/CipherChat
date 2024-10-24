/** @type {import('tailwindcss').Config} */
export default {
  content: [
    './index.html',
    './src/**/*.{js,ts,jsx,tsx}',
  ],
  theme: {
    extend: {
      colors: {
        'text-primary': '#2D3748', // Darker gray
        'background-gray': '#4A5568',
        'teal-dark': '#2C7A7B',
        'cool-white': '#F4FDFF',
        'warm-gray': '#8E9394',
        'soft-teal': '#85BCC7',
      },
      fontFamily: {
        'mono': ['Roboto Mono', 'monospace'],
      },
    },
  },
  plugins: [],
}

