import Cookies from "js-cookie";
import { LoginRepository } from "../Repositories/LoginRepository";
import { disconnectChatHub } from "../hooks/useChatConnection";
import { disconnectHub } from "../components/ChatHub/js/useChatHub";
export const checkAuthToken = async (): Promise<boolean> => {
  try {
    const repo = new LoginRepository();
    const response = await repo.authenticate();
    return response.status === 200;
  } catch (error) {
    console.error("Error during authentication check:", error);
    return false;
  }
};

export const handleUnauthenticated = async () => {
  Cookies.remove("token");
  Cookies.remove("refreshToken");
  localStorage.clear();
  await disconnectChatHub();
  await disconnectHub();
  window.location.href = "/login";
};
