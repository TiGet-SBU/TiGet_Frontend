import React from 'react'
import Button from '../Button/Button';
import "./Navbar.css";
const Navbar = () => {
  return (
    <div className='navbar'>
        <Button text='ورود / ثبت نام' onClick={()=>true}/>
        <div className='navbar__text'><span>پشتیبانی</span></div>
        <div className='navbar__text'><span>سفر های من</span></div>
        <div className='navbar__text'><span>سبد خرید</span></div>
        <div className='navbar__text'><span>جستجوی بلیت ها</span></div>
        <div className='navbar__logo'>Tiget</div>
    </div>
  )
}

export default Navbar