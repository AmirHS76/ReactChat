import Cookies from "js-cookie";
import { LoginRepository } from "../Repositories/LoginRepository";
import { disconnectChatHub } from "../hooks/useChatConnection";
import { disconnectHub } from "../components/ChatHub/js/useChatHub";
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

export const handleUnauthenticated = async () => {
  await disconnectChatHub();
  await disconnectHub();
  Cookies.remove("token");
  Cookies.remove("refreshToken");
  localStorage.clear();
};
