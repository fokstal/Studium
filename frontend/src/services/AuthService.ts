export class AuthServise {
  private url = "http://localhost:5141/user/";

  public async login(login: string, password: string) {
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

    if (!res.ok) throw new Error("Not correct password or login");

    return res;
  }
}
