import React, { useState } from "react";
import "./BuyPage.css";
import PurchaseInformation from "../../Components/PurchaseInformation/PurchaseInformation";
import Navbar from "../../Components/Navbar/Navbar";
import CreateTicket from "../../Components/Ticket/CreateTicket";
import { Ticket } from "../../FakeData/fakeData";
import { fakeTickets } from "../../FakeData/fakeData";

const BuyPage = () => {
  const [purchaseInfoComponents, setPurchaseInfoComponents] = useState<
    JSX.Element[]
  >([
    <PurchaseInformation
      key={0}
      id={0}
      onRemove={() => {}}
      removable={false}
    />,
  ]);

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
    const updatedComponents = purchaseInfoComponents.filter(
      (_, index) => index !== idToRemove
    );
    setPurchaseInfoComponents(updatedComponents);
  };

  const handleConfirmation = () => {
    // Perform actions for confirming and continuing the purchase
    // For instance, you might want to gather all the purchase information
    // and proceed to the next step or finalize the transaction here
    // You can also add validation logic before proceeding further
    // This is a placeholder function, modify it according to your requirements
    console.log("Purchase confirmed and continued!");
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
        <button
          onClick={() => removePurchaseInfo(purchaseInfoComponents.length - 1)}
        >
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
