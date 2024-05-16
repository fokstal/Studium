import { Service } from "./Service";

export class GradeService extends Service {
  protected url = "http://localhost:5141/grade";

  public override async get() {
    return await this.baseGetMethod("list");
  }

  public async getByStudent(id: number) {
    return await this.baseGetMethod(`list-by-student/${id}`);
  }

  public async getBySubject(id: number) {
    return await this.baseGetMethod(`list-by-subject/${id}`);
  }

  public async getBySubjectByStudent(subjectId: number, studentId: number) {
    return await this.baseGetMethod(
      `list-by-student/${studentId}/by-subject/${subjectId}`
    );
  }

  private async baseGetMethod(additionalUrl: string) {
    const res = await fetch(`${this.url}/${additionalUrl}`);

    return await res.json();
  }
}
