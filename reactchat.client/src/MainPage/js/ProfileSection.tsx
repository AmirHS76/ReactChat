import React from 'react';
import '../css/ProfileSection.css';

const ProfileSection = () => {
    const userProfile = {
        name: 'John Doe',
        email: 'john.doe@example.com',
    };

    return (
        <div className="profile-section">
            <h2>Your Profile</h2>
            <p><strong>Name:</strong> {userProfile.name}</p>
            <p><strong>Email:</strong> {userProfile.email}</p>
        </div>
    );
};

export default ProfileSection;
