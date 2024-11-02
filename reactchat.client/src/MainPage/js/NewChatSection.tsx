import React from 'react';
import '../css/NewChatSection.css';

const NewChatSection = ({ onStartNewChat }) => {
    return (
        <div className="new-chat-section">
            <label className="chat-label">Click this button to start a new chat:</label>
            <button className="start-chat-button" onClick={onStartNewChat}>
                Start a new chat
            </button>
        </div>
    );
};

export default NewChatSection;
