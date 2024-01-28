import React, { useState } from "react";
import "./LoginPage.css";
import { Role } from "../../FakeData/fakeData";
import { Login } from "./Login";
import { SignUp } from "./SignUp";
const LoginPage = () => {
  const [loginState, setLoginState] = useState<boolean>(true);
  // 2 means user login
  // 1 means company login
  const [loginType, setLoginType] = useState<Role>(Role.User);
  const [error, setError] = useState("");

  const toggleState = () => {
    setError("");
    setLoginState(!loginState);
  };
  const toggleType = () => {
    if (loginType === Role.User) setLoginType(Role.Company);
    else setLoginType(Role.User);
    setError("");
  };
  return (
    <div>
      <div className="title">Tiget</div>
      <div className="form-holder">
        {loginState ? (
          <Login
            loginType={loginType}
            setError={(err: string) => setError(err)}
          />
        ) : (
          <SignUp
            loginType={loginType}
            setError={(err: string) => setError(err)}
          />
        )}
        <div className="login-signup-toggle">
          {loginState ? (
            <span onClick={toggleState}>ساخت حساب کاربری جدید</span>
          ) : (
            <span onClick={toggleState}>قبلا ثبت نام کرده‌اید</span>
          )}
        </div>
        <div className="user-company-toggle">
          {loginType === 2 ? (
            <span onClick={toggleType}>ورود کاربر</span>
          ) : (
            <span onClick={toggleType}>ورود شرکت</span>
          )}
        </div>
        <div className="error">
          {error}
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
