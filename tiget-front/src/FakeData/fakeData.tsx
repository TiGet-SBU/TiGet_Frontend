export type Ticket = {
    name: string,
    Vehicle: string , 
    description : String, 
    price : number
   };
 
export const fakeTickets: Ticket[] = [
   {name: "تهران", Vehicle : "اتوبوس", description : "پایتخت ایران", price : 1780000 },
   {name: "تبریز", Vehicle : "قطار", description : "شهر دل انگیز", price : 98725532  },
   {name: "شیراز", Vehicle : "هواپیما", description : "شهر شعر و ادب", price : 1764487  },
   {name: "اصفهان", Vehicle : "قطار", description : "نصف جهان", price : 5542378965  }
 ];