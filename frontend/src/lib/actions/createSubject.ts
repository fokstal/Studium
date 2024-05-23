import { SubjectService } from "../../services";
import { Group, User } from "../../types";

const subjectService = new SubjectService();

export async function createSubject(data: {
  name?: string;
  description?: string;
  teacher?: User;
  group?: Group;
}) {
  if (!data.description || !data.group || !data.name || !data.teacher) {
    return "Заполните данные полностью";
  }
  const res = await subjectService.post({ ...data, groupId: data.group.id, teacherId: data.teacher.id });
  if (res.status !== 201) {
    return "Данные введены не коректно, пожалуйста проверьте формат данных";
  } else {
    return "Created";
  }
}
