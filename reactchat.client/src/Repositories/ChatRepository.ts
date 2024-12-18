import {getRequest} from '../services/apiService';
class ChatRepository {
  async getUserChats(targetUsername : string) {
    return getRequest(`/user/chatHistory?targetUsername=${targetUsername}`);
  }
}
export default ChatRepository;