import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import Cookies from 'js-cookie';
import '../../styles/PrivateChat.css'; // Correctly importing plain CSS
import { useChatConnection } from '../../hooks/useChatConnection';
import MessageList from './MessageList';
import ChatInput from './ChatInput';

interface Message {
    sender: string;
    content: string;
    type: 'sender' | 'recipient';
}

const PrivateChat: React.FC = () => {
    const { username } = useParams<{ username: string }>();
    const currentUser = Cookies.get('username');
    const [messages, setMessages] = useState<Message[]>([]);
    const [connection, connectToHub] = useChatConnection();

    useEffect(() => {
        if (!connection) {
            connectToHub(username, setMessages, currentUser);
        }
    }, [connection, connectToHub, username, currentUser]);

    const handleSendMessage = async (message: string) => {
        if (connection && message.trim()) {
            await connection.invoke('SendPrivateMessage', username, message);
        }
    };

    return (
        <div className="private-chat-container">
            <h2 className="chat-header">Private chat with {username}</h2>
            <MessageList messages={messages} />
            <ChatInput onSendMessage={handleSendMessage} />
        </div>
    );
};

export default PrivateChat;
