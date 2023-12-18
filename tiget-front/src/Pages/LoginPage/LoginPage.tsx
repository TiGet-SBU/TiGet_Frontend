import React, { useState } from 'react';
import Button from '../../Components/Button/Button';
import './LoginPage.css';
const Login = () =>{
    return <form>
        <input type='text' placeholder='نام کاربری'/>
        <input type='text' placeholder='رمز عبور'/>
        <Button text='ورود' onClick={()=>true}/>
    </form>
}
const SignUp = () => {
    return <form>
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