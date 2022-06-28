import { AddressUI } from "./addressui.model";
import { GenderUI } from "./genderui.model";

export interface StudentUI {
    id: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    email: string;
    mobile: number;
    profileImageUrl: string;
    genderId: string;
    gender: GenderUI;
    address: AddressUI;
}