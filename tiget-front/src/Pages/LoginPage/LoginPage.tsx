import React, { useState, useContext } from 'react';
import { UserContext } from '../../Components/UserProvider/UserProvider';
import Button from '../../Components/Button/Button';
import './LoginPage.css';
import { sag_Company, sina } from '../../FakeData/fakeData';
import { useNavigate } from 'react-router-dom';
const Login : React.FC<{loginType:boolean}> = ({loginType}) =>{
    const { login,userData } = useContext(UserContext);
    const navigate = useNavigate();
    const handleLogin = () => {
            if (loginType)
                login(sina);
            else
                login(sag_Company);
            navigate('/account');
      };
    return <form className='login-form'>
        <input type='text' placeholder='نام کاربری'/>
        <input type='text' placeholder='رمز عبور'/>
        <Button text='ورود' onClick={handleLogin}/>
    </form>
}
const SignUp : React.FC<{loginType:boolean}> = ({loginType}) => {
    const handleSignUp = () => {
        return true;
    }
    return <form className='login-form'>
        <input type='text' placeholder='نام کاربری'/>
        <input type='text' placeholder='رمز عبور'/>
        <input type='text' placeholder='تکرار رمز عبور'/>
        <input type='text' placeholder='ایمیل'/>
        <Button text='ساخت اکانت' onClick={handleSignUp}/>
    </form>
}
const LoginPage = () => 
{
    const [loginState,setLoginState] = useState<boolean>(true);
    // 0 means user login
    // 1 means company login
    const [loginType,setLoginType] = useState<boolean>(true);


    const toggleState = () => {
        setLoginState(!loginState);
    }
    const toggleType = () => {
        setLoginType(!loginType);
    }
    return (
        <div>
            <div className='title'>
                Tiget
            </div>
            <div className='form-holder'>
                {loginState? 
                (<Login loginType={loginType} />): 
                (<SignUp loginType={loginType} />)
                }
                <div className='login-signup-toggle'>
                    {loginState? 
                    <span onClick={toggleState}>ساخت حساب کاربری جدید</span>:
                    <span onClick={toggleState}>قبلا ثبت نام کرده‌اید</span>}
                </div>
                <div className='user-company-toggle'>
                    {
                        loginType?
                        <span onClick={toggleType}>ورود کاربر</span> :
                        <span onClick={toggleType}>ورود شرکت</span>
                    }
                </div>
            </div>
        </div>
    )
}

export default LoginPage