import React from "react";
import "../../styles/PrivateChat.css"; // Correctly importing plain CSS

interface Message {
  sender: string;
  content: string;
  type: "sender" | "recipient";
}

interface MessageItemProps {
  message: Message;
}

const MessageItem: React.FC<MessageItemProps> = ({ message }) => {
  return (
    <div className={`message-item ${message.type}`}>
      <strong>{message.sender}:</strong> {message.content}
    </div>
  );
};

export default MessageItem;
