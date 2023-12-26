// PurchaseInformation.tsx
import React, { useState } from "react";
import "./PurchaseInformation.css";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

const PurchaseInformation: React.FC<{
  onRemove: () => void;
  id: number;
  removable: boolean;
}> = ({ onRemove, id, removable }) => {
  const [latinFirstName, setLatinFirstName] = useState("");
  const [latinLastName, setLatinLastName] = useState("");
  const [gender, setGender] = useState("");
  const [nationalCode, setNationalCode] = useState("");
  const [persianFirstName, setPersianFirstName] = useState("");
  const [persianLastName, setPersianLastName] = useState("");
  const [birthDate, setBirthDate] = useState<Date | null>(null);

  const handleLatinFirstNameChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setLatinFirstName(event.target.value);
  };

  const handleLatinLastNameChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setLatinLastName(event.target.value);
  };

  const handleGenderChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setGender(event.target.value);
  };

  const handleNationalCodeChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setNationalCode(event.target.value);
  };

  const handlePersianFirstNameChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setPersianFirstName(event.target.value);
  };

  const handlePersianLastNameChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setPersianLastName(event.target.value);
  };

  const handleBirthDateChange = (date: Date | null) => {
    setBirthDate(date);
  };

  return (
    <div className="user-form">
      <div className="form-row">
        <input
          type="text"
          placeholder="نام لاتین"
          value={latinFirstName}
          onChange={handleLatinFirstNameChange}
          className="form-input"
        />
        <input
          type="text"
          placeholder="نام خانوادگی لاتین"
          value={latinLastName}
          onChange={handleLatinLastNameChange}
          className="form-input"
        />
        <select
          value={gender}
          onChange={handleGenderChange}
          className="form-input"
        >
          <option value="" disabled>
            جنسیت
          </option>
          <option value="مرد">مرد</option>
          <option value="زن">زن</option>
        </select>
        <input
          type="text"
          placeholder="کد ملی"
          value={nationalCode}
          onChange={handleNationalCodeChange}
          className="form-input"
        />
      </div>
      <div className="form-row">
        <input
          type="text"
          placeholder="نام"
          value={persianFirstName}
          onChange={handlePersianFirstNameChange}
          className="form-input"
        />
        <input
          type="text"
          placeholder="نام خانوادگی"
          value={persianLastName}
          onChange={handlePersianLastNameChange}
          className="form-input"
        />
        <DatePicker
          selected={birthDate}
          onChange={handleBirthDateChange}
          dateFormat="dd/MM/yyyy"
          placeholderText="تاریخ تولد"
          className="form-input"
          calendarClassName="datepicker-calendar"
          locale="fa"
        />
        
      </div>
    </div>
  );
};

export default PurchaseInformation;
