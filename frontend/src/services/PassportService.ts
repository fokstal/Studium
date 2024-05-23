import { Service } from "./Service";

export class PassportService extends Service {
  protected url = "http://localhost:5141/passport";

  public async post(data: any) {
    const res = await fetch(this.url, {
      method: "POST",
      body: data,
      credentials: "include",
    });
    
    return res;
  }
}