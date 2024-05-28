export abstract class Service {
  protected url: string = "";

  public async get() {
    const res = await fetch(this.url, { credentials: "include" });

    return await res.json();
  }

  public async getById(id: number | string) {
    const res = await fetch(`${this.url}/${id}`, { credentials: "include" });

    return await res.json();
  }

  public async post(data: any) {
    const res = await fetch(this.url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
      body: JSON.stringify(data),
    });

    return res;
  }

  public async put(id: number | string, data: any) {
    const res = await fetch(`${this.url}/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
      body: JSON.stringify(data),
    });

    return res;
  }

  public async delete(id: number | string) {
    const res = await fetch(`${this.url}/${id}`, {
      method: "DELETE",
      credentials: "include",
    });

    return res;
  }
}
