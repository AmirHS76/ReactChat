import { useState, useEffect } from 'react';
import '../css/ProfileSection.css';
import Axios from "axios";
import Cookies from 'js-cookie';

const ProfileSection = () => {
    const [username, setUsername] = useState<string>('');
    const [email, setEmail] = useState<string>('');
    const [isEditingEmail, setIsEditingEmail] = useState<boolean>(false);
    const [newEmail, setNewEmail] = useState<string>('');

    const GetUserData = async () => {
        const token = Cookies.get('token');
        const response = await Axios.get("https://localhost:7240/GetUserData", {
            headers: { Authorization: `Bearer ${token}` }
        });
        setUsername(response.data.username);
        setEmail(response.data.email);
    };

    const updateEmail = async () => {
        const token = Cookies.get('token');
        await Axios.get("https://localhost:7240/user/updateEmail/"+newEmail,
            { headers: { Authorization: `Bearer ${token}` } }
        );
        setEmail(newEmail);
        setIsEditingEmail(false);
    };

    useEffect(() => {
        GetUserData();
    }, []);

    return (
        <div className="profile-section">
            <h2>Your Profile</h2>
            <p><strong>Name:</strong> {username}</p>
            <p><strong>Email:</strong> {email || 'Not provided'}</p>
            {!email && !isEditingEmail && (
                <button onClick={() => setIsEditingEmail(true)}>Enter your email</button>
            )}
            {isEditingEmail && (
                <div>
                    <input
                        type="email"
                        value={newEmail}
                        onChange={(e) => setNewEmail(e.target.value)}
                        placeholder="Enter your email"
                    />
                    <button onClick={updateEmail}>Save Email</button>
                </div>
            )}
        </div>
    );
};

export default ProfileSection;
