import { ReactNode } from "react";
export type Preview = {
    name: string,
    Vehicle: string, 
    description : string,
    image : any, 
   };
export type Ticket = {
   src : string,
   dst : string,
   time : Date,
   vehicle : string,
   price : number
}
export type Account = {
  first_name : string,
  last_name : string,
  password : string,
  birth_date : Date,
  email : string,
  phone_number : string,
  balance : number
}

export interface Company extends Account {
  name : string,
  address : string,
  description : string,
  email : string,
}

export type MyComponentProps = {
  children: ReactNode;
};
const img = require("../Resources/destination.jpg");
export const sina : Account = {first_name : "سینا",
                               last_name : "طاهری بهروز",
                               password : "123456",
                               birth_date : new Date("2019-4-5"),
                               email : "sinatb.dev@gmail.com",
                               balance : 54648798,
                               phone_number : "+989999999999"};
export const sag_Company : Company = {
                                name: "شرکت هواپیمایی سگ",
                                address: "روستای بهروز",
                                description: "فرستادن سگ به روستا های مختلف کشور",
                                email: "sag@email.com",
                                password: "als;fkaslkf!",
                                first_name: "",
                                last_name: "",
                                birth_date: new Date("2020-2-3"),
                                phone_number: "",
                                balance: 0
                              };
export const fakePreview: Preview[] = [
   {name: "تهران", Vehicle : "اتوبوس", description : "پایتخت ایران",image : img },
   {name: "تبریز", Vehicle : "قطار", description : "شهر دل انگیز",image : img },
   {name: "شیراز", Vehicle : "هواپیما", description : "شهر شعر و ادب",image : img},
   {name: "اصفهان", Vehicle : "قطار", description : "نصف جهان",image : img}
 ];
 export const fakeTickets : Ticket[] = [
   {src: "تهران" , dst:"دبی",time: new Date("2018-8-9"), vehicle:"هواپیما", price:1785556687},
   {src: "تهران" , dst:"استانبول",time: new Date("2013-2-2"), vehicle:"اتوبوس", price:9978835457},
   {src: "تبریز" , dst:"تهران",time: new Date("2017-4-12"), vehicle:"قطار", price:1231548796},
   {src: "تهران" , dst:"علی آباد",time: new Date("2012-8-9"), vehicle:"هواپیما", price:1785978978},
 ];
 export const fakeAirLines: string[] = [
   "iran air",
   "mahan",
   "lufthansa",
   "Qatar Airlines",
   "United Airlines"
 ];
 export const types: string[] = [
   "مستقیم",
   "یک",
   "دو",
   "بیشتر از 2",
   "همه"
 ];
 export enum userType{
  user,
  company
 }