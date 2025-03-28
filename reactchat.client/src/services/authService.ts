import Cookies from "js-cookie";
import { LoginRepository } from "../Repositories/LoginRepository";
const repo = new LoginRepository();
export const checkAuthToken = async (): Promise<boolean> => {
  try {
    const response = await repo.authenticate();
    return response.status === 200;
  } catch (error) {
    console.error("Error during authentication check:", error);
    return false;
  }
};

export const handleUnauthenticated = () => {
  Cookies.remove("token");
  localStorage.clear();
};
