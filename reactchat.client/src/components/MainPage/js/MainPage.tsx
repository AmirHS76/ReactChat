import { useEffect, useState } from "react";
import "../css/MainPage.css";
import WelcomeSection from "../../../Sections/RegularUserSections/js/WelcomeSection";
import AdminSection from "../../../Sections/AdminSections/js/AdminSection";
import ProfileSection from "../../../Sections/PublicSections/js/ProfileSection";
import NewChatSection from "../../../Sections/PublicSections/js/NewChatSection";
import UserRepository from "../../../Repositories/UserRepository";
const MainPage = () => {
  const [userRole, setUserRole] = useState<string | null>(null);
  const userRepo = new UserRepository();
  useEffect(() => {
    const fetchUserRole = async () => {
      const response = await userRepo.fetchUserRole();
      const data: string = response;
      setUserRole(data);
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
