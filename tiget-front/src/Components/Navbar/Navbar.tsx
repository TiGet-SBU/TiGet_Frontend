import React from 'react'
import Button from '../Button/Button';
import "./Navbar.css";
import { Link, useNavigate } from 'react-router-dom';

const Navbar = () => {
  const navigate = useNavigate();
  return (
    <div className='navbar'>
        <Button text='ورود / ثبت نام' onClick={()=>navigate("/login")}/>
        <div className='navbar__text'><Link className='link' to="/support"><span>پشتیبانی</span></Link></div>
        <div className='navbar__text'><Link className='link' to="/buypage"><span>سبد خرید</span></Link></div>
        <div className='navbar__text'><Link className='link' to="/search"><span>جستجوی بلیت ها</span></Link></div>
        <div className='navbar__logo'><Link className='link' to="/">Tiget</Link></div>
    </div>
  )
}

export default Navbar