import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { useChatHub } from "../../../components/ChatHub/js/useChatHub";
import "../css/GroupChat.css";
import Cookies from "js-cookie";
import MessageList from "../../../components/privateChat/MessageList";
import { Message } from "../../../contexts/ChatContext";

const GroupChat = () => {
  const { groupName } = useParams<{ groupName: string }>();
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState("");
  const connection = useChatHub();
  const currentUser: string = Cookies.get("username") ?? "";

  useEffect(() => {
    if (connection) {
      connection.on("ReceiveMessage", (sender: string, content: string) => {
        setMessages((prevMessages) => [
          ...prevMessages,
          {
            sender: sender === currentUser ? "You" : sender,
            content: content,
            type: sender === currentUser ? "sender" : "recipient",
          },
        ]);
      });
    }
    return () => {
      if (connection) {
        connection.off("ReceiveMessage");
      }
    };
  }, [connection, currentUser]);

  const handleSendMessage = async () => {
    if (connection && newMessage && groupName) {
      try {
        await connection.invoke("SendGroupMessage", groupName, newMessage);
        setNewMessage("");
      } catch (error) {
        console.error("Error sending message:", error);
      }
    }
  };

  return (
    <div className="group-chat-container">
      <div className="messages">
        <MessageList messages={messages} />
      </div>
      <div className="message-input">
        <input
          type="text"
          value={newMessage}
          onChange={(e) => setNewMessage(e.target.value)}
          placeholder="Type your message..."
        />
        <button className="send-button" onClick={handleSendMessage}>
          Send
        </button>
      </div>
    </div>
  );
};

export default GroupChat;
