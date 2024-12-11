import axios, { AxiosRequestConfig, AxiosResponse, InternalAxiosRequestConfig } from 'axios';
import Cookies from 'js-cookie';

const apiClient = axios.create({
    baseURL: process.env.REACT_APP_API_BASE_URL,
    timeout: 5000,
    headers: {
        'Content-Type': 'application/json',
    },
});

apiClient.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const token = Cookies.get('token');
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

apiClient.interceptors.response.use(
    (response: AxiosResponse) => response,
    (error) => {
        console.error('API request error:', error);
        if (error.response?.status === 401) {
            console.log('Unauthorized access - redirecting to login.');
            Cookies.remove('token');
        }
        return Promise.reject(error);
    }
);

export const getRequest = async (url: string, config?: AxiosRequestConfig) => {
    try {
        const response = await apiClient.get(url, config);
        return response.data;
    } catch (error) {
        console.error('GET request error:', error);
        throw error;
    }
};

export const postRequest = async (url: string, data: any, config?: AxiosRequestConfig) => {
    try {
        const response = await apiClient.post(url, data, config);
        return response.data;
    } catch (error) {
        console.error('POST request error:', error);
        throw error;
    }
};

export const putRequest = async (url: string, data: any, config?: AxiosRequestConfig) => {
    try {
        const response = await apiClient.put(url, data, config);
        return response.data;
    } catch (error) {
        console.error('PUT request error:', error);
        throw error;
    }
};

export const deleteRequest = async (url: string, config?: AxiosRequestConfig) => {
    try {
        const response = await apiClient.delete(url, config);
        return response.data;
    } catch (error) {
        console.error('DELETE request error:', error);
        throw error;
    }
};

export default apiClient;
