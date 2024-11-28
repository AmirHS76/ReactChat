import axios, { AxiosRequestConfig, AxiosResponse, InternalAxiosRequestConfig } from 'axios';
import Cookies from 'js-cookie';

// Create an instance of Axios with a base configuration
const apiClient = axios.create({
    baseURL: 'https://localhost:7240', // Base URL for your API
    timeout: 5000, // Optional: Set a timeout for requests
    headers: {
        'Content-Type': 'application/json',
    },
});

// Add a request interceptor to attach the token to every request if available
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

// Add a response interceptor for global error handling
apiClient.interceptors.response.use(
    (response: AxiosResponse) => response,
    (error) => {
        console.error('API request error:', error);
        // Handle specific error types if needed (e.g., redirect on 401, etc.)
        if (error.response?.status === 401) {
            // Example: Redirect to login if not authenticated
            console.log('Unauthorized access - redirecting to login.');
            Cookies.remove('token'); // Clear token if unauthenticated
        }
        return Promise.reject(error);
    }
);

// Generic function to make GET requests
export const getRequest = async (url: string, config?: AxiosRequestConfig) => {
    try {
        const response = await apiClient.get(url, config);
        return response.data;
    } catch (error) {
        console.error('GET request error:', error);
        throw error;
    }
};

// Generic function to make POST requests
export const postRequest = async (url: string, data: any, config?: AxiosRequestConfig) => {
    try {
        const response = await apiClient.post(url, data, config);
        return response.data;
    } catch (error) {
        console.error('POST request error:', error);
        throw error;
    }
};

// Generic function to make PUT requests
export const putRequest = async (url: string, data: any, config?: AxiosRequestConfig) => {
    try {
        const response = await apiClient.put(url, data, config);
        return response.data;
    } catch (error) {
        console.error('PUT request error:', error);
        throw error;
    }
};

// Generic function to make DELETE requests
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
