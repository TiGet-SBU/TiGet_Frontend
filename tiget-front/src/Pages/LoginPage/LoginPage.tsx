import React, { useState, useContext } from 'react';
import { UserContext } from '../../Components/UserProvider/UserProvider';
import Button from '../../Components/Button/Button';
import './LoginPage.css';
import { sina } from '../../FakeData/fakeData';
import { useNavigate } from 'react-router-dom';
const Login = () =>{
    const { login,userData } = useContext(UserContext);
    const navigate = useNavigate();
    const handleLogin = () => {
            login(sina);
            navigate('/account');
      };
    return <form className='login-form'>
        <input type='text' placeholder='نام کاربری'/>
        <input type='text' placeholder='رمز عبور'/>
        <Button text='ورود' onClick={handleLogin}/>
    </form>
}
const SignUp = () => {
    return <form className='login-form'>
        <input type='text' placeholder='نام کاربری'/>
        <input type='text' placeholder='رمز عبور'/>
        <input type='text' placeholder='تکرار رمز عبور'/>
        <input type='text' placeholder='ایمیل'/>
        <Button text='ساخت اکانت' onClick={()=>true}/>
    </form>
}
const LoginPage = () => 
{
    const [loginState,setLoginState] = useState<boolean>(true);

    function toggleState(){
        setLoginState(!loginState);
    }
    return (
        <div>
            <div className='title'>
                Tiget
            </div>
            <div className='form-holder'>
                {loginState? 
                (<Login/>): 
                (<SignUp/>)
                }
                <div className='login-signup-toggle'>
                    {loginState? 
                    <span onClick={toggleState}>ساخت حساب کاربری جدید</span>:
                    <span onClick={toggleState}>قبلا ثبت نام کرده‌اید</span>}
                </div>
            </div>
        </div>
    )
}

export default LoginPage