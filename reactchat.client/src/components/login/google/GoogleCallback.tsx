import React, { useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import Cookies from "js-cookie";

const GoogleCallback: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    const token = params.get("token");
    const refreshToken = params.get("refreshToken");

    if (token && refreshToken) {
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
      // Optionally, set any additional data like username
      navigate("/main");
    } else {
      // Handle error conditions, such as showing an error message
      console.error("Missing token(s) in Google callback.");
    }
  }, [location, navigate]);

  return <div>Processing Google authentication...</div>;
};

export default GoogleCallback;
