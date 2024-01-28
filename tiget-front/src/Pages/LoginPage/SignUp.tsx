import React, { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { UserContext } from "../../Components/UserProvider/UserProvider";
import Button from "../../Components/Button/Button";
import { Role, Customer, Company } from "../../FakeData/fakeData";
import axios from "axios";

export const SignUp: React.FC<{
  loginType: Role;
  setError: (err: string) => void;
}> = ({ loginType, setError }) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [secPassword, setSecPassword] = useState("");
  const { login, userData } = useContext(UserContext);
  const navigate = useNavigate();

  const handleSignUp = () => {
    if (password === secPassword) {
      axios
        .post("http://localhost:5120/api/Auth/singup", {
          email: email,
          password: password,
          passwordRepeat: secPassword,
          role: loginType,
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
        .catch((e) => console.log(e));
    }else{
      setError("رمز های وارد شده یکی نیستند")
    }
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
        type="password"
        placeholder="رمز عبور"
      />
      <input
        value={secPassword}
        onChange={(e) => setSecPassword(e.target.value)}
        type="password"
        placeholder="تکرار رمز عبور"
      />
      <Button text="ساخت اکانت" onClick={handleSignUp} />
    </form>
  );
};
