import { Service } from "./Service";

export class PersonService extends Service {
  protected url = "http://localhost:5141/person";

  public async post(data: any) {
    const res = await fetch(this.url, {
      method: "POST",
      body: data,
      credentials: "include",
    });
    
    return await res.json();
  }

  public async put(id: number, data: any) {
    const res = await fetch(`${this.url}/${id}`, {
      method: "PUT",
      body: data,
      credentials: "include",
    });

    return res;
  }
}