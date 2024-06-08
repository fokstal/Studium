import { Service } from "./Service";

export class StudentService extends Service {
  protected url = process.env.REACT_APP_IP + "/student";
}