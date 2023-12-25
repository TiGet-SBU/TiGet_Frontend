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

  return (
    <div>
      <Navbar />
      <CreateTicket ticket={fakeTickets[0]} />
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
    </div>
  );
};

export default BuyPage;
