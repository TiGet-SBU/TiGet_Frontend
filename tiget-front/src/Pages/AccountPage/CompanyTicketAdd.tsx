import React from 'react'
import { Account } from '../../FakeData/fakeData'
import Button from '../../Components/Button/Button'

const CompanyTicketAdd : React.FC<{account : Account | null}> = ({account}) => {
  return (
    <>
      {account === null ? 
        <div></div> :
        <div className='add-card-form-holder'>
            <form className='ticket-info-form'>
                <input placeholder='مبدا'/>
                <input placeholder='مقصد'/>
                <input placeholder='هزینه'/>
                <input placeholder='زمان'/>
                <select name="states">
                  <option>اتوبوس</option>
                  <option>قطار</option>
                  <option>هواپیما</option>
                </select>
            </form>
            <div className='ticket-add'>
              <Button text='ساخت بلیت' onClick={()=>true}/>
            </div>
        </div>
        }  
    </>
  )
}

export default CompanyTicketAdd