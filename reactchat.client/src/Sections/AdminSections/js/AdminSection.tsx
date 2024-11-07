import '../css/AdminSection.css';
import { useNavigate } from "react-router-dom";

const AdminSection = () => {
    const navigate = useNavigate();
    const handleManageUsers = () => {
        navigate("/userManagement");
    };
    const handleNewUser = () => {
        navigate("/NewUser");
    };
    const handleManagesettings = () => {
        navigate("/settingManagement");
    };
    return (
        <div className="admin-section">
            <div className="admin-label">Admin Section</div>
            <button onClick={handleManageUsers}>Manage Users</button>
            <button onClick={handleNewUser}>Add new user</button>
            <button onClick={handleManagesettings}>Manage Settings</button>
        </div>
    );
};

export default AdminSection;
