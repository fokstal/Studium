import { Service } from "./Service";

export class PersonService extends Service {
  protected url = "http://localhost:5141/person";

  public async post(data: any) {
    const res = await fetch(this.url, {
      method: "POST",
      body: data,
    });
    
    return await res.json();
  }
}