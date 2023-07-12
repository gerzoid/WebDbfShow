import axios from 'axios';

export const axio = axios.create({
    baseURL : import.meta.env.VITE_URL
})