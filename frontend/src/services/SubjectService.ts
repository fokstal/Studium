import { Service } from "./Service";

export class SubjectService extends Service {
  protected url = process.env.REACT_APP_IP + "/subject";
}