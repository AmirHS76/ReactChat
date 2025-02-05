import { useState, useEffect } from "react";
import "../css/ProfileSection.css";
import UserRepository from "../../../Repositories/UserRepository";
const ProfileSection = () => {
  const [username, setUsername] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [isEditingEmail, setIsEditingEmail] = useState<boolean>(false);
  const [newEmail, setNewEmail] = useState<string>("");
  const userRepo = new UserRepository();

  const updateEmail = async () => {
    await userRepo.updateEmail(newEmail);
    setEmail(newEmail);
    setIsEditingEmail(false);
  };

  useEffect(() => {
    const GetUserData = async () => {
      const response = await userRepo.getUserData();
      const data = response.data as { username: string; email: string };
      setUsername(data.username);
      setEmail(data.email);
    };
    GetUserData();
  }, []);

  return (
    <div className="profile-section">
      <h2>Your Profile</h2>
      <p>
        <strong>Name:</strong> {username}
      </p>
      <p>
        <strong>Email:</strong> {email || "Not provided"}
      </p>
      {!email && !isEditingEmail && (
        <button onClick={() => setIsEditingEmail(true)}>
          Enter your email
        </button>
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
