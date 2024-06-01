export class AuthServise {
  private url = process.env.REACT_APP_IP + "/user/";

  public async login(login: string, password: string) {
    console.log(this.url);
    const res = await fetch(`${this.url}login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        login,
        password,
      }),
      credentials: "include",
    });

    return res;
  }

  public async register(data: any) {
    const res = await fetch(`${this.url}register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
      body: JSON.stringify(data),
    });

    return res;
  }

  public async session() {
    const res = await fetch(`${this.url}session`, {
      credentials: "include"
    });

    if (!res.ok) return false;
    return await res.json();
  }
}
