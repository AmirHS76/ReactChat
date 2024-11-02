import React from 'react';
import '../css/MainPage.css';
import WelcomeSection from './WelcomeSection';
import ProfileSection from './ProfileSection';
import NewChatSection from './NewChatSection';

const MainPage = () => {
    const handleStartNewChat = () => {
        // Logic to start a new chat
        console.log('Starting a new chat...');
        // Here, you can add functionality to open a chat window or navigate to a chat page
    };

    return (
        <div className="main-page">
            <header className="header">
                <h1>Chat Application</h1>
            </header>
            <div className="container">
                <WelcomeSection />
                <NewChatSection onStartNewChat={handleStartNewChat} />
                <ProfileSection />
            </div>
        </div>
    );
};

export default MainPage;
