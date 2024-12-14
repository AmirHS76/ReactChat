import { deleteRequest, getRequest, postRequest } from '../services/apiService';
import user from '../types/users';
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

    addUser = async (userModel : user) => {
        try {
            const response = await postRequest(`user`,userModel);
            return response;
        } catch (error) {
            console.error('Error Adding user: ',error);
            throw error;
        }
    };
    

    getUsers = async () => {
        try {
            const response = await getRequest('user/getAll');
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

    deleteUser = async (userId: number) => {
        try {
            const response = await deleteRequest(`user/${userId}`)
            return response;
        }
        catch (error){
            console.error("Error when deleting user",error);
            throw error;
        }
        }
}
