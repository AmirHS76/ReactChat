import { getRequest, postRequest } from '../services/apiService';

export class UserRepository {
    getUser = async (userID: number) => {
        try {
            const response = await getRequest(`Users/id=${userID}`);
            return response;
        } catch (error) {
            console.error('Error fetching user:', error);
            throw error;
        }
    };

    getUsers = async () => {
        try {
            const response = await getRequest('Users');
            return response;
        } catch (error) {
            console.error('Error fetching users:', error);
            throw error;
        }
    };

    updateUser = async (data: object) => {
        try {
            const response = await postRequest('Users', data);
            return response;
        } catch (error) {
            console.error('Error updating user:', error);
            throw error;
        }
    };
}
