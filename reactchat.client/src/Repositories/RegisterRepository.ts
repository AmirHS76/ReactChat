import { postRequest } from "../services/apiService";

export class LoginRepository {
  register = async (username: string, password: string, email: string) => {
    try {
      const response = await postRequest(`register`, {
        username,
        password,
        email,
      });
      return response;
    } catch (error) {
      console.log(error);
      throw error;
    }
  };
}
export default LoginRepository;
