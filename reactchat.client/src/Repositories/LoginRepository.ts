import { getRequest, postRequest } from "../services/apiService";
import Cookies from "js-cookie";
export class LoginRepository {
    login = async (username : string , password : string) => {
            try{
                const response = await postRequest(`login`,{username,password});
                return response;
            }
            catch(error) {
                console.log(error);
                throw error;
            }
        };
    refreshToken = async (refreshToken : string | undefined) =>{
        try{
            const response = await getRequest(`login?refreshToken=${refreshToken}`);
            Cookies.set('token', response.data, { expires: 7, secure: true, sameSite: 'Strict' });
            return response;
        }
        catch(error) {
            console.log(error);
            throw error;
        }
    }
    authenticate = async () => {
        try{
            const response = await getRequest(`authenticate`);
            return response;
        }
        catch(error) {
            console.log(error);
            throw error;
        }
    }
}