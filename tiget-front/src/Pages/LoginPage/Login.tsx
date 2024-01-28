import React, { useState, useContext } from "react";
import { UserContext } from "../../Components/UserProvider/UserProvider";
import Button from "../../Components/Button/Button";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { Role, Company, Customer } from "../../FakeData/fakeData";

export const Login: React.FC<{
  loginType: Role;
  setError: (err: string) => void;
}> = ({ loginType, setError }) => {
  const { login, userData } = useContext(UserContext);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleLogin = () => {
    axios
      .post("http://localhost:5120/api/Auth/login", {
        email: email,
        password: password,
      })
      .then((Response) => {
        localStorage.setItem("token", Response.data.token.access_token);
        if (Response.data.role === Role.User) {
          let customerData: Customer = Response.data;
          login(customerData, Role.User);
        } else if (Response.data.role === Role.Company) {
          let companyData: Company = Response.data;
          login(companyData, Role.Company);
        }
        navigate("/account");
      })
      .catch((e) => {
        setError("ایمیل یا رمز عبور اشتباه است");
      });
  };
  return (
    <form className="login-form">
      <input
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        type="text"
        placeholder="ایمیل"
      />
      <input
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        type={"password"}
        placeholder="رمز عبور"
      />
      <Button text="ورود" onClick={handleLogin} />
    </form>
  );
};
