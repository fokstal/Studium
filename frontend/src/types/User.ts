export type User = {
  login: string;
  id: string;
  firstName: string;
  middleName: string;
  lastName: string;
  password?: string;
  roleEntityList: {
    name: string;
  }[];
};
