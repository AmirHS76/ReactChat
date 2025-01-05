import React, { useEffect, useState, useRef } from "react";
import { useParams } from "react-router-dom";
import Cookies from "js-cookie";
import { useChatConnection } from "../../hooks/useChatConnection";
import ChatRepository from "../../Repositories/ChatRepository";
import MessageList from "./MessageList";
import ChatInput from "./ChatInput";
import "../../styles/PrivateChat.css";

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
  const messageListRef = useRef<HTMLDivElement | null>(null);
  const chatRepo = new ChatRepository();

  // Pagination states
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);

  useEffect(() => {
    setMessages([]);
    if (username) {
      fetchChatHistory(username, 1).then((data) => {
        data.forEach((message: Message) => {
          setMessages((prev) => [
            ...prev,
            {
              sender: message.sender === currentUser ? "You" : message.sender,
              content: message.content,
              type: message.sender === currentUser ? "sender" : "recipient",
            },
          ]);
        });
      });
    }
  }, []);

  useEffect(() => {
    if (!connection && username) {
      connectToHub(username, setMessages, currentUser);
    }
  }, [connection, connectToHub, username, currentUser]);

  const fetchChatHistory = async (username: string, pageNum = 1) => {
    const result = await chatRepo.getUserChats(username, pageNum);
    setHasMore(result.data.hasMore);
    return result.data.messages;
  };

  const loadMoreMessages = async () => {
    const nextPage = page + 1;
    setPage(nextPage);
    const data = await fetchChatHistory(username!, nextPage);
    data.forEach((message: Message) => {
      setMessages((prev) => [
        ...prev,
        {
          sender: message.sender === currentUser ? "You" : message.sender,
          content: message.content,
          type: message.sender === currentUser ? "sender" : "recipient",
        },
      ]);
    });
  };

  const handleSendMessage = async (message: string) => {
    if (connection && message.trim()) {
      await connection.invoke("SendPrivateMessage", username, message);
      const container = messageListRef.current;
      if (container) container.scrollTop = container.scrollHeight;
    }
  };

  useEffect(() => {
    const container = messageListRef.current;
    if (!container) return;
    container.scrollTop = container.scrollTop + 20; // Scroll to the bottom
    const handleScroll = () => {
      if (container.scrollTop === 0 && hasMore) {
        loadMoreMessages();
      }
    };
    container.addEventListener("scroll", handleScroll);
    return () => container.removeEventListener("scroll", handleScroll);
  }, [hasMore, messages]);

  return (
    <div className="private-chat-container">
      <div className="private-chat-header">
        <h2>{username}</h2>
      </div>
      <div
        id="111"
        ref={messageListRef}
        style={{ flex: "1", overflowY: "auto", maxHeight: "400px" }}
      >
        <MessageList messages={messages} />
      </div>
      <div className="private-chat-input">
        <ChatInput onSendMessage={handleSendMessage} />
      </div>
    </div>
  );
};

export default PrivateChat;
