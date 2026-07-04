import { useEffect, useState } from "react";
import api from "./services/api";
import MapPage from "./pages/MapPage";

import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import { Message } from "primereact/message";

import "./App.css";

function App() {
  const [activeTab, setActiveTab] = useState("login");
  const [token, setToken] = useState("");

  const [loginEmail, setLoginEmail] = useState("test@example.com");
  const [loginPassword, setLoginPassword] = useState("123456");

  const [registerFullName, setRegisterFullName] = useState("");
  const [registerEmail, setRegisterEmail] = useState("");
  const [registerPassword, setRegisterPassword] = useState("");

  const [message, setMessage] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const savedToken = localStorage.getItem("token");

    if (savedToken) {
      setToken(savedToken);
    }
  }, []);

  const handleLogin = async () => {
    try {
      setLoading(true);
      setMessage(null);

      const response = await api.post("/Auth/login", {
        email: loginEmail,
        password: loginPassword,
      });

      const data = response.data;

      if (data.success && data.token) {
        localStorage.setItem("token", data.token);
        setToken(data.token);
        setLoading(false);
        return;
      }

      setMessage({
        severity: "error",
        text: data.message || "Giriş başarısız.",
      });

      setLoading(false);
    } catch (error) {
      console.log("LOGIN ERROR:", error);
      console.log("LOGIN ERROR RESPONSE:", error.response?.data);

      setMessage({
        severity: "error",
        text:
          error.response?.data?.message ||
          "Giriş işlemi sırasında hata oluştu.",
      });

      setLoading(false);
    }
  };

  const handleRegister = async () => {
    try {
      setLoading(true);
      setMessage(null);

      if (!registerFullName || !registerEmail || !registerPassword) {
        setMessage({
          severity: "warn",
          text: "Lütfen tüm alanları doldurun.",
        });

        setLoading(false);
        return;
      }

      if (!registerEmail.endsWith("@gmail.com")) {
        setMessage({
          severity: "warn",
          text: "Kayıt için Gmail adresi kullanmalısınız.",
        });

        setLoading(false);
        return;
      }

      const response = await api.post("/Auth/register", {
        fullName: registerFullName,
        email: registerEmail,
        password: registerPassword,
      });

      const data = response.data;

      if (data.success) {
        setMessage({
          severity: "success",
          text: data.message || "Kayıt başarılı. Giriş yapabilirsiniz.",
        });

        setActiveTab("login");
        setLoginEmail(registerEmail);
        setLoginPassword("");

        setRegisterFullName("");
        setRegisterEmail("");
        setRegisterPassword("");

        setLoading(false);
        return;
      }

      setMessage({
        severity: "error",
        text: data.message || "Kayıt başarısız.",
      });

      setLoading(false);
    } catch (error) {
      console.log("REGISTER ERROR:", error);
      console.log("REGISTER ERROR RESPONSE:", error.response?.data);

      setMessage({
        severity: "error",
        text:
          error.response?.data?.message ||
          "Kayıt işlemi sırasında hata oluştu.",
      });

      setLoading(false);
    }
  };

  const logout = () => {
    localStorage.removeItem("token");
    setToken("");
    setMessage(null);
    setActiveTab("login");
  };

  if (token) {
    return <MapPage token={token} onLogout={logout} />;
  }

  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="auth-icon">
          <i className="pi pi-user"></i>
        </div>

        <h1>Tekrar Hoş Geldiniz</h1>
        <p className="auth-subtitle">Hesabınıza giriş yaparak devam edin.</p>

        <div className="auth-tabs">
          <button
            type="button"
            className={activeTab === "login" ? "active" : ""}
            onClick={() => {
              setActiveTab("login");
              setMessage(null);
            }}
          >
            Giriş Yap
          </button>

          <button
            type="button"
            className={activeTab === "register" ? "active" : ""}
            onClick={() => {
              setActiveTab("register");
              setMessage(null);
            }}
          >
            Kayıt Ol
          </button>
        </div>

        {message && (
          <Message
            severity={message.severity}
            text={message.text}
            className="auth-message"
          />
        )}

        {activeTab === "login" && (
          <div className="auth-form">
            <div className="field">
              <label>E-posta</label>
              <InputText
                value={loginEmail}
                onChange={(e) => setLoginEmail(e.target.value)}
                placeholder="test@example.com"
              />
            </div>

            <div className="field">
              <label>Şifre</label>
              <Password
                value={loginPassword}
                onChange={(e) => setLoginPassword(e.target.value)}
                feedback={false}
                toggleMask
                placeholder="Şifrenizi girin"
              />
            </div>

            <Button
              label="Giriş Yap"
              icon="pi pi-sign-in"
              loading={loading}
              onClick={handleLogin}
              className="auth-button"
            />
          </div>
        )}

        {activeTab === "register" && (
          <div className="auth-form">
            <div className="field">
              <label>Ad Soyad</label>
              <InputText
                value={registerFullName}
                onChange={(e) => setRegisterFullName(e.target.value)}
                placeholder="Adınızı ve soyadınızı girin"
              />
            </div>

            <div className="field">
              <label>Gmail Adresi</label>
              <InputText
                value={registerEmail}
                onChange={(e) => setRegisterEmail(e.target.value)}
                placeholder="ornek@gmail.com"
              />
            </div>

            <div className="field">
              <label>Şifre</label>
              <Password
                value={registerPassword}
                onChange={(e) => setRegisterPassword(e.target.value)}
                feedback={false}
                toggleMask
                placeholder="Şifrenizi girin"
              />
            </div>

            <Button
              label="Kayıt Ol"
              icon="pi pi-user-plus"
              loading={loading}
              onClick={handleRegister}
              className="auth-button"
            />
          </div>
        )}
      </div>
    </div>
  );
}

export default App;