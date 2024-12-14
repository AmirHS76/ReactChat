import axios from 'axios';
import Cookies from 'js-cookie';

export const checkAuthToken = async (token: string): Promise<boolean> => {
    try {
        const refreshToken = Cookies.get('refreshToken');
        const response = await axios.get('https://localhost:7240/authenticate', {
            headers: { Authorization: `Bearer ${token}`,'refreshToken': refreshToken }
        });
        return response.status === 200;
    } catch (error) {
        console.error('Error during authentication check:', error);
        return false;
    }
};

export const handleUnauthenticated = () => {
    Cookies.remove('token'); // Clear the token cookie if not authenticated
};
