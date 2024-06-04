import { Service } from "./Service";

export class SubjectService extends Service {
  protected url = process.env.REACT_APP_IP + "/subject";

  public async getSubjectsBySession() {
    const res = await fetch(`${this.url}/list-by-session`, {
      credentials: "include",
    });

    return await res.json();
  }
}
