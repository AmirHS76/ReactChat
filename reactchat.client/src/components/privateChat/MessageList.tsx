import React from "react";
import MessageItem from "./MessageItem";

interface Message {
  sender: string;
  content: string;
  type: "sender" | "recipient";
}

interface MessageListProps {
  messages: Message[];
}

const MessageList: React.FC<MessageListProps> = ({ messages }) => {
  return (
    <div className="private-message-list">
      {messages.map((message, index) => (
        <MessageItem key={index} message={message} />
      ))}
    </div>
  );
};

export default MessageList;
