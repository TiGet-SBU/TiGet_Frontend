import React from 'react'
import './SearchForm.css'
import Button from '../Button/Button'
const SearchForm = () => {
  return (
    <div>
        <form>
            <div className='ticket-specification-form'>
                <input type='text' placeholder='مبدا'/>
                <input type='text' placeholder='مقصد'/>
                <input type='text' placeholder='تاریخ'/>
                <input type='text' placeholder='تعداد مسافر'/>
                <input type='text' placeholder='کلاس بلیت'/>
            </div>
            <div className='search-form_button-holder'>
                <Button text='جستجو' onClick={()=>true}/>
            </div>
        </form>
    </div>
  )
}

export default SearchForm