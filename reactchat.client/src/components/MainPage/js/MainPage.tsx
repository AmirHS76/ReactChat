import { useEffect, useState } from 'react';
import axios from 'axios';
import '../css/MainPage.css';
import WelcomeSection from '../../../Sections/RegularUserSections/js/WelcomeSection';
import AdminSection from '../../../Sections/AdminSections/js/AdminSection';
import ProfileSection from '../../../Sections/PublicSections/js/ProfileSection';
import NewChatSection from '../../../Sections/PublicSections/js/NewChatSection';
import Cookies from 'js-cookie';
const MainPage = () => {
    const [userRole, setUserRole] = useState<string | null>(null);

    useEffect(() => {
        const fetchUserRole = async () => {
            const token = Cookies.get('token');
            const response = await axios.get("https://localhost:7240/user/GetRole", {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            const data = await response.data;
            setUserRole(data.role);
        };

        fetchUserRole();
    }, []);

    if (userRole === null) {
        return <div>Loading...</div>;
    }

    return (
        <div className="main-page">
            <header className="header">
                <h1>Chat Application</h1>
            </header>
            <div className="container">
                {userRole == "Admin" ? <AdminSection /> : <WelcomeSection />}
                <NewChatSection />
                <ProfileSection />
            </div>
        </div>
    );
};

export default MainPage;
