import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import { ChakraProvider } from "@chakra-ui/react";
import ChakraTheme from "../chakra-theme.ts"
import App from './App.tsx'
import './index.css';

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <ChakraProvider theme={ChakraTheme}>
            <App/>
        </ChakraProvider>
    </StrictMode>,
)
