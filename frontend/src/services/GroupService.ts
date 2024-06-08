import { Service } from "./Service";

export class GroupService extends Service {
  protected url = process.env.REACT_APP_IP + "/group";
}