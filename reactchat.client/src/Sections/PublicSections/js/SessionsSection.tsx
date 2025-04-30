import { useEffect, useState, useMemo } from "react";
import "../css/ProfileSection.css";
import UserRepository from "../../../Repositories/UserRepository";
import Sessions from "../../../types/Sessions";

const SessionsSection = () => {
  const [sessions, setSessions] = useState<Sessions[]>([]);
  const [loading, setLoading] = useState(true);
  const [revokingId, setRevokingId] = useState<number | null>(null);
  const userRepo = useMemo(() => new UserRepository(), []);

  useEffect(() => {
    const fetchSessions = async () => {
      const response = await userRepo.getSessions();
      setSessions(response.data || []);
      setLoading(false);
    };
    fetchSessions();
  }, [userRepo]);

  const handleRevoke = async (id: number) => {
    setRevokingId(id);
    await userRepo.revokeSession(id);
    setSessions((prev) =>
      prev.map((s) => (s.id === id ? { ...s, isRevoked: true } : s))
    );
    setRevokingId(null);
  };

  return (
    <div className="profile-section">
      <h2>Your Active Sessions</h2>
      {loading ? (
        <p>Loading...</p>
      ) : sessions.length === 0 ? (
        <p>No active sessions found.</p>
      ) : (
        <ul className="sessions-list">
          {sessions.map((session, idx) => (
            <li key={session.id || idx} className="session-item">
              <div>
                <strong>User Agent:</strong> {session.userAgent || "Unknown"}
              </div>
              <div>
                <strong>IP:</strong> {session.ipAddress || "Unknown"}
              </div>
              <div>
                <strong>Last Active:</strong>{" "}
                {session.lastActivity || session.createdAt || "Unknown"}
              </div>
              {session.isRevoked ? (
                <span className="revoked">Revoked</span>
              ) : (
                <button
                  className="session-button"
                  style={{ marginTop: "10px", width: "fit-content" }}
                  onClick={() => handleRevoke(session.id)}
                  disabled={revokingId === session.id}
                >
                  {revokingId === session.id ? "Revoking..." : "Revoke Session"}
                </button>
              )}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default SessionsSection;
