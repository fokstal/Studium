import { Service } from "./Service";

export class UserService extends Service {
  protected url = process.env.REACT_APP_IP + "/user";
}