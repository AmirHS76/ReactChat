import React, { useState } from 'react';
import '../css/SettingsPage.css';

const SettingsPage: React.FC = () => {
    const [isDarkMode, setIsDarkMode] = useState<boolean>(true);

    const handleDarkModeToggle = () => {
        setIsDarkMode(!isDarkMode);
    };

    return (
        <div className={`settings-page ${isDarkMode ? 'dark' : 'light'}`}>
            <div className="page-title">Settings</div>
            <div className="settings-option">
                <label>Dark Mode</label>
                <button onClick={handleDarkModeToggle} className="toggle-button">
                    {isDarkMode ? 'Disable' : 'Enable'}
                </button>
            </div>
            <div className="settings-option">
                <label>Notifications</label>
                <button className="toggle-button">Enable</button>
            </div>
        </div>
    );
};

export default SettingsPage;
