import React, { useState } from "react";
import "../../styles/PrivateChat.css"; // Correctly importing plain CSS

interface ChatInputProps {
  onSendMessage: (message: string) => void;
}

const ChatInput: React.FC<ChatInputProps> = ({ onSendMessage }) => {
  const [input, setInput] = useState<string>("");

  const handleSend = () => {
    if (input.trim()) {
      onSendMessage(input);
      setInput("");
    }
  };

  return (
    <div className="input-container">
      <input
        type="text"
        value={input}
        onChange={(e) => setInput(e.target.value)}
        onKeyDown={(e) => e.key === "Enter" && handleSend()}
        placeholder="Type a message..."
        className="message-input"
      />
      <button onClick={handleSend} className="send-button">
        Send
      </button>
    </div>
  );
};

export default ChatInput;
