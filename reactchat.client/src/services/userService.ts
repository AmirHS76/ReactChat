import axios from 'axios';

export const fetchUserRole = async (token: string): Promise<string> => {
    try {
        const response = await axios.get('https://localhost:7240/user/GetRole', {
            headers: { Authorization: `Bearer ${token}` },
        });
        return response.data.role;
    } catch (error) {
        console.error('Error fetching user role:', error);
        return '';
    }
};
