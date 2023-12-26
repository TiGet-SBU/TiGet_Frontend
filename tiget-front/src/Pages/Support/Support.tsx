import React from 'react';
import './Support.css';
import Navbar from '../../Components/Navbar/Navbar';
import Button from '../../Components/Button/Button';
const Support = () => {
  return (
    <>
        <Navbar/>
        <div className='support-form-holder'>
          <form className='support-form'>
            <div className='support-form-right'>
              <input placeholder='نام و نام خانوادگی'/>
              <input placeholder='ایمیل'/>
              <select name="states">
                <option>بازگشت بلیت</option>
                <option>مشکل فنی در سایت</option>
                <option>شکایت به شرکت فروشنده</option>
              </select>
            </div>
            <div className='support-form-left'>
              <input placeholder='شرح مشکل'/>
              <div className='support-form-button-holder'>
                <Button text='ثبت مشکل' onClick={()=>true}/>
              </div>
            </div>
          </form>
        </div>
    </>
  )
}

export default Support