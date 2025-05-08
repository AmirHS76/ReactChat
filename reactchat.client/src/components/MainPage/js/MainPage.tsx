import { useEffect, useState } from "react";
import "../css/MainPage.css";
import WelcomeSection from "../../../components/Sections/RegularUserSections/js/WelcomeSection";
import AdminSection from "../../../components/Sections/AdminSections/js/AdminSection";
import ProfileSection from "../../../components/Sections/PublicSections/js/ProfileSection";
import NewChatSection from "../../../components/Sections/PublicSections/js/NewChatSection";
import UserRepository from "../../../Repositories/UserRepository";
const MainPage = () => {
  const [userRole, setUserRole] = useState<string | null>(null);
  useEffect(() => {
    const userRepo = new UserRepository();
    const fetchUserRole = async () => {
      const role = localStorage.getItem("role");
      if (role !== null) {
        setUserRole(role);
        return;
      }
      const response = await userRepo.fetchUserRole();
      const data: string = response;
      localStorage.setItem("role", data);
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
