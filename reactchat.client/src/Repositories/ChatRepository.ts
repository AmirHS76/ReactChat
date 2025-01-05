import { getRequest } from "../services/apiService";
class ChatRepository {
  async getUserChats(targetUsername: string, pageNum: number) {
    return getRequest(
      `/user/chatHistory?targetUsername=${targetUsername}&pageNum=${pageNum}`
    );
  }
}
export default ChatRepository;
