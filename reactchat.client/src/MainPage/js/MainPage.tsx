import '../css/MainPage.css';
import WelcomeSection from './WelcomeSection';
import ProfileSection from './ProfileSection';
import NewChatSection from './NewChatSection';

const MainPage = () => {
    const handleStartNewChat = () => {
        console.log('Starting a new chat...');
    };

    return (
        <div className="main-page">
            <header className="header">
                <h1>Chat Application</h1>
            </header>
            <div className="container">
                <WelcomeSection />
                <NewChatSection />
                <ProfileSection />
            </div>
        </div>
    );
};

export default MainPage;
