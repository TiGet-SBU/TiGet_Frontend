import React from 'react';
import './Button.css';
const Button = ({text,onClick} : {text:string,onClick:any}) => 
{
  return (
    <div className='Button' onClick={onClick}>
      {text}
    </div>
  )
}

export default Button