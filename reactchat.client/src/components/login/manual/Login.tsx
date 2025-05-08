import { useState, useEffect, useMemo, useCallback } from "react";
import "./Login.css";
import Cookies from "js-cookie";
import { useNavigate } from "react-router-dom";
import { LoginRepository } from "../../../Repositories/LoginRepository";

interface LoginResponse {
  token: string;
  refreshToken: string;
}

const Login: React.FC = () => {
  const [username, setUsername] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [message, setMessage] = useState<string | null>(null);
  const [captcha, setCaptcha] = useState<string | null>("");
  const [captchaImg, setCaptchaImg] = useState<string>("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const repo = useMemo(() => new LoginRepository(), []);

  const fetchCaptcha = useCallback(async () => {
    try {
      const res = await repo.getCaptchaImage();
      if (res.status == 200) {
        setCaptchaImg(res.data);
      }
    } catch (err) {
      setError("Failed to load captcha : " + err);
    }
  }, [repo]);

  useEffect(() => {
    const token = Cookies.get("token");
    if (token) {
      navigate("/main");
    }

    fetchCaptcha();
  }, [fetchCaptcha, navigate]);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError(null);
    setMessage(null);

    try {
      const response = await repo.login(username, password, captcha);

      if (response.status !== 200) {
        fetchCaptcha();
        throw new Error("Invalid username or password");
      }
      const usernameToken = username;
      Cookies.set("username", usernameToken, {
        secure: true,
        sameSite: "Strict",
      });
      const { token, refreshToken } = response.data as LoginResponse;
      Cookies.set("token", token, {
        expires: 1,
        secure: true,
        sameSite: "Strict",
      });
      Cookies.set("refreshToken", refreshToken, {
        expires: 2,
        secure: true,
        sameSite: "Strict",
      });
      console.log("Login successful");
      setMessage("Login successful! Redirecting...");

      setTimeout(() => {
        navigate("/main");
      }, 500);
    } catch (err) {
      fetchCaptcha();
      setError("Invalid username or password");
      console.log("Error in login: " + err);
      setTimeout(() => setError(null), 3000);
    }
  };

  const handleGoogleLogin = () => {
    const backendUrl: string = import.meta.env.VITE_BACKEND_URL;
    window.location.href = backendUrl + "api/ExternalAuth/google-login";
  };

  const handleRefreshCaptcha = (e: React.MouseEvent) => {
    e.preventDefault();
    fetchCaptcha();
  };

  return (
    <div className="login-container">
      {message && <div className="info-message">{message}</div>}
      {error && <div className="error-message">{error}</div>}
      <h1 className="login-title">Login</h1>
      <form className="login-form" onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="username">Username</label>
          <input
            type="text"
            id="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <div className="form-group captcha-group">
          <label htmlFor="captcha">Captcha</label>
          <div className="captcha-row">
            {captchaImg && (
              <img
                className="captcha-img"
                src={`data:image/png;base64,${captchaImg}`}
                alt="captcha"
              />
            )}
            <button
              type="button"
              className="refresh-captcha-button"
              onClick={handleRefreshCaptcha}
              aria-label="Refresh captcha"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="20"
                height="20"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <path d="M21 2v6h-6" />
                <path d="M3 12a9 9 0 0 1 15-7.36L21 8" />
                <path d="M3 22v-6h6" />
                <path d="M21 12a9 9 0 0 1-15 7.36L3 16" />
              </svg>
            </button>
          </div>
          <div className="captcha-input-wrapper">
            <input
              type="text"
              id="captcha"
              value={captcha ?? ""}
              onChange={(e) => setCaptcha(e.target.value)}
              required
              autoComplete="off"
              placeholder="Enter captcha"
              className="captcha-input"
            />
          </div>
        </div>
        <div className="button-container">
          <button type="submit" className="login-button">
            Login
          </button>
          <button
            type="button"
            className="google-login-button"
            onClick={handleGoogleLogin}
          >
            Continue with Google
          </button>
        </div>
        <div className="login-footer">
          <p>
            Don't have an account? <a href="/register">Sign Up</a>
          </p>
        </div>
      </form>
    </div>
  );
};

export default Login;
