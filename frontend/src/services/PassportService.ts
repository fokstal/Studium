import { Service } from "./Service";

export class PassportService extends Service {
  protected url = process.env.REACT_APP_IP + "/passport";

  public async post(data: any) {
    const res = await fetch(this.url, {
      method: "POST",
      body: data,
      credentials: "include",
    });
    
    return res;
  }
}