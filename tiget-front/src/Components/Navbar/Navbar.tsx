import React from 'react'
import Button from '../Button/Button';
import "./Navbar.css";
const Navbar = () => {
  return (
    <div className='navbar'>
        <Button text='ورود / ثبت نام' onClick={()=>true}/>
        <div className='navbar__text'><span>پشتیبانی</span></div>
        <div className='navbar__text'><span>بلیت هواپیما</span></div>
        <div className='navbar__text'><span>بلیت قطار</span></div>
        <div className='navbar__text'><span>بلیت اتوبوس</span></div>
        <div className='navbar__logo'>Tiget</div>
    </div>
  )
}

export default Navbar