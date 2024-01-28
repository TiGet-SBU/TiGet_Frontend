import React, { useState } from "react";
import { Customer, Company, Role } from "../../FakeData/fakeData";
import Button from "../../Components/Button/Button";
import axios from "axios";

export const UserDataCard: React.FC<{ account: Customer | Company | null }> = ({
  account,
}) => {
  const [name, setName] = useState(
    account === null ? "" : (account as Customer).firstName
  );
  const [lastName, setLastName] = useState(
    account === null ? "" : (account as Customer).lastName
  );
  const [email, setEmail] = useState(account === null ? "" : account.email);
  const [phoneNumber, setPhoneNumber] = useState(
    account === null ? "" : account.phoneNumber
  );
  const handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setName(e.target.value);
  };
  const handleLastNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setLastName(e.target.value);
  };
  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
  };
  const handlePhoneNumberChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPhoneNumber(e.target.value);
  };

  const handleChange = () => {
    axios
      .put("http://localhost:5120/api/customer/update", {
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        email: email,
        phoneNumber: phoneNumber,
        firstName: name,
        lastName: lastName,
      })
      .then(() => console.log("change done"))
      .catch();
  };
  return (
    <>
      {account === null ? (
        <div></div>
      ) : (
        <div className="data-card">
          <form className="data-card-form">
            <div className="data-card-holder">
              {account.role === Role.User ? (
                <div>نام </div>
              ) : (
                <div>نام شرکت</div>
              )}
              <input
                className="data-card-input"
                value={name}
                onChange={handleNameChange}
              />
            </div>
            {account.role === Role.Company ? (
              <div></div>
            ) : (
              <div className="data-card-holder">
                <div>نام خانوادگی </div>
                <input
                  className="data-card-input"
                  value={lastName}
                  onChange={handleLastNameChange}
                />
              </div>
            )}
            <div className="data-card-holder">
              <div>ایمیل</div>
              <input
                className="data-card-input"
                value={email}
                onChange={handleEmailChange}
              />
            </div>
            <div className="data-card-holder">
              <div>شماره همراه</div>
              <input
                className="data-card-input"
                value={phoneNumber}
                onChange={handlePhoneNumberChange}
                style={{ direction: "ltr" }}
              />
            </div>
          </form>
          <div className="account-acceptance-button">
            <Button text="اعمال تغییرات" onClick={handleChange} />
          </div>
        </div>
      )}
    </>
  );
};
