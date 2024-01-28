import React, { useState, useContext } from "react";
import { UserContext } from "../../Components/UserProvider/UserProvider";
import Button from "../../Components/Button/Button";
import "./LoginPage.css";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { Role, Company, Customer } from "../../FakeData/fakeData";
const Login: React.FC<{ loginType: Role }> = ({ loginType }) => {
  const { login, userData } = useContext(UserContext);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleLogin = () => {
    axios.post("http://localhost:5120/api/customer/login", {
        email: email,
        password: password,
      })
      .then((Response) => {
        console.log(Response.data.role);
        console.log(Role.User);
        localStorage.setItem('token', Response.data.token.access_token)
        if (Response.data.role === Role.User)
        {
            let customerData : Customer = Response.data;
            console.log(Response);
            login(customerData,Role.User);
        }else if (Response.data.role === Role.Company)
        {
            let companyData : Company = Response.data;
            login(companyData,Role.Company);
        }
      })
      .catch((e) => {
        console.log(e);
      });
    navigate("/account");
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
const SignUp: React.FC<{ loginType: Role }> = ({ loginType }) => {
  const handleSignUp = () => {
    return true;
  };
  return (
    <form className="login-form">
      <input type="text" placeholder="نام کاربری" />
      <input type="text" placeholder="رمز عبور" />
      <input type="text" placeholder="تکرار رمز عبور" />
      <input type="text" placeholder="ایمیل" />
      <Button text="ساخت اکانت" onClick={handleSignUp} />
    </form>
  );
};
const LoginPage = () => {
  const [loginState, setLoginState] = useState<boolean>(true);
  // 0 means user login
  // 1 means company login
  const [loginType, setLoginType] = useState<Role>(Role.User);

  const toggleState = () => {
    setLoginState(!loginState);
  };
  const toggleType = () => {
    if (loginType === Role.User) setLoginType(Role.Company);
    else setLoginType(Role.User);
  };
  return (
    <div>
      <div className="title">Tiget</div>
      <div className="form-holder">
        {loginState ? (
          <Login loginType={loginType} />
        ) : (
          <SignUp loginType={loginType} />
        )}
        <div className="login-signup-toggle">
          {loginState ? (
            <span onClick={toggleState}>ساخت حساب کاربری جدید</span>
          ) : (
            <span onClick={toggleState}>قبلا ثبت نام کرده‌اید</span>
          )}
        </div>
        <div className="user-company-toggle">
          {loginType ? (
            <span onClick={toggleType}>ورود کاربر</span>
          ) : (
            <span onClick={toggleType}>ورود شرکت</span>
          )}
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
