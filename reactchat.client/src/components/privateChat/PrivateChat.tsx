import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import Cookies from "js-cookie";
import "../../styles/PrivateChat.css";
import { useChatConnection } from "../../hooks/useChatConnection";
import MessageList from "./MessageList";
import ChatInput from "./ChatInput";
import ChatRepository from "../../Repositories/ChatRepository";
interface Message {
  sender: string;
  content: string;
  type: "sender" | "recipient";
}

const PrivateChat: React.FC = () => {
  const { username } = useParams<{ username: string }>();
  const currentUser = Cookies.get("username");
  const [messages, setMessages] = useState<Message[]>([]);
  const [connection, connectToHub] = useChatConnection();
  const chatRepo = new ChatRepository();
  useEffect(() => {
    setMessages([]);
    if (username)
      fetchChatHistory(username).then((data) => {
        (data as { data: Message[] }).data.forEach((message: Message) => {
          setMessages((prevMessages) => [
            ...prevMessages,
            {
              sender: message.sender === currentUser ? "You" : message.sender,
              content: message.content,
              type: message.sender === currentUser ? "sender" : "recipient",
            },
          ]);
        });
      });
  }, []);
  useEffect(() => {
    if (!connection && username) {
      connectToHub(username, setMessages, currentUser);
    }
  }, [connection, connectToHub, username, currentUser]);

  const fetchChatHistory = async (username: string) => {
    return await chatRepo.getUserChats(username);
  };
  const handleSendMessage = async (message: string) => {
    if (connection && message.trim()) {
      await connection.invoke("SendPrivateMessage", username, message);
    }
  };

  return (
    <div className="private-chat-container">
      <div className="private-chat-header">
        <h2>{username}</h2>
      </div>
      <div>
        <MessageList messages={messages} />
      </div>
      <div className="private-chat-input">
        <ChatInput onSendMessage={handleSendMessage} />
      </div>
    </div>
  );
};

export default PrivateChat;
