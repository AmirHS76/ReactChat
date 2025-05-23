import { getRequest, postRequest } from "../services/apiService";
import Cookies from "js-cookie";
export class LoginRepository {
  login = async (
    username: string,
    password: string,
    captcha: string | null
  ) => {
    try {
      const response = await postRequest(
        `login`,
        {
          username,
          password,
          captcha,
        },
        { withCredentials: true }
      );
      return response;
    } catch (error) {
      console.log(error);
      throw error;
    }
  };
  refreshToken = async (refreshToken: string | undefined) => {
    try {
      const response = await getRequest(`login?refreshToken=${refreshToken}`);
      Cookies.set("token", response.data as string, {
        expires: 7,
        secure: true,
        sameSite: "Strict",
      });
      return response;
    } catch (error) {
      console.log(error);
      throw error;
    }
  };
  authenticate = async () => {
    try {
      const response = await getRequest(`api/v1/authenticate`);
      return response;
    } catch (error) {
      console.log(error);
      throw error;
    }
  };

  authenticateWithGoogle = async () => {
    try {
      const response = await getRequest(`api/ExternalAuth/google-login`);
      return response;
    } catch (error) {
      console.log(error);
      throw error;
    }
  };
  getCaptchaImage = async () => {
    try {
      const response = await getRequest(`api/v1/captcha`, {
        withCredentials: true,
      });
      return response;
    } catch (error) {
      console.log(error);
      throw error;
    }
  };
}
