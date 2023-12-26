// BuyPage.tsx
import React, { useState } from "react";
import "./BuyPage.css";
import PurchaseInformation from "../../Components/PurchaseInformation/PurchaseInformation";
import Navbar from "../../Components/Navbar/Navbar";
import CreateTicket from "../../Components/Ticket/CreateTicket";
import { fakeTickets } from "../../FakeData/fakeData";
import { Link, useNavigate } from 'react-router-dom';

const BuyPage = () => {
  const [purchaseInfoComponents, setPurchaseInfoComponents] = useState<JSX.Element[]>(
    [<PurchaseInformation key={0} id={0} onRemove={() => {}} removable={false} />],
  );

  const navigate = useNavigate();

  const addNewPurchaseInfo = () => {
    const newId = purchaseInfoComponents.length;
    setPurchaseInfoComponents([
      ...purchaseInfoComponents,
      <PurchaseInformation
        key={newId}
        id={newId}
        onRemove={() => removePurchaseInfo(newId)}
        removable={true}
      />,
    ]);
  };

  const removePurchaseInfo = (idToRemove: number) => {
    if (purchaseInfoComponents.length === 1) return;
    const updatedComponents = purchaseInfoComponents.filter((_, index) => index !== idToRemove);
    setPurchaseInfoComponents(updatedComponents);
  };

  const handleConfirmation = () => {
    // Gather information from PurchaseInformation components
    const userNames = purchaseInfoComponents.map((component, index) => ({
      id: index,
      firstName: component.props.latinFirstName,
      lastName: component.props.latinLastName,
    }));
  
    // Construct the URL with user names as parameters
    const url = `/confirmationPage?${userNames.map((info) => `user=${encodeURIComponent(JSON.stringify(info))}`).join('&')}`;
  
    // Navigate to the confirmation page with user information
    navigate(url);
  };
  
  

  return (
    <div>
      <Navbar />
      <div className="buyPage-ticket">
        <CreateTicket ticket={fakeTickets[0]} />
      </div>
      {purchaseInfoComponents.map((component, index) => (
        <div key={index}>{component}</div>
      ))}
      <div className="button-group">
        <button onClick={addNewPurchaseInfo}>اضافه کردن مسافر جدید</button>
        <button onClick={() => removePurchaseInfo(purchaseInfoComponents.length - 1)}>
          حذف کردن
        </button>
      </div>
      {/* Add the confirmation button here */}
      <button onClick={handleConfirmation} className="confirmation-button">
        تایید و ادامه خرید
      </button>
    </div>
  );
};

export default BuyPage;
