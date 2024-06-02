import { Service } from "./Service";

export class UserService extends Service {
  protected url = process.env.REACT_APP_IP + "/user";

  public async getUsersBySubject(subjectId: number) {
    const res = await fetch(`${this.url}/list-by-subject/${subjectId}`);

    return await res.json()
  }
}