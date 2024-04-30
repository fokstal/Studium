export type Person = {
  id?: number;
  firstName: string;
  middleName: string;
  lastName: string;
  birthDate: string | Date;
  sex: boolean;
  avatarFileName: string;
}