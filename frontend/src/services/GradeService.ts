import { Service } from "./Service";

export class GradeService extends Service {
  protected url = process.env.REACT_APP_IP + "/grade";

  public async averageGrade(id: string) {
    return await this.baseGetMethod(`average-by-student/${id}`);
  }
  public override async get() {
    return await this.baseGetMethod("list");
  }

  public async getByStudent(id: string) {
    return await this.baseGetMethod(`list-by-student/${id}`);
  }

  public async getBySubject(id: number) {
    return await this.baseGetMethod(`list-by-subject/${id}`);
  }

  public async getBySubjectByStudent(subjectId: number, studentId: string) {
    return await this.baseGetMethod(
      `list-by-student/${studentId}/by-subject/${subjectId}`
    );
  }

  private async baseGetMethod(additionalUrl: string) {
    const res = await fetch(`${this.url}/${additionalUrl}`, {
      credentials: "include",
    });

    return await res.json();
  }

  public async getAverageForSemestr(data: any) {
    const res = await fetch(`${this.url}/average-to-subject-list-by-student`, {
      method: "POST",
      credentials: "include",
      body: JSON.stringify(data),
      headers: {
        "Content-Type": "application/json",
      },
    });

    return await res.json();
  }
}
