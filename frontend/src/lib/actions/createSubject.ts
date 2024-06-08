import { useContext } from "react";
import { SubjectService } from "../../services";
import { LanguageContext, Translator } from "../../store";
import { Group, User } from "../../types";

const subjectService = new SubjectService();

export async function createSubject(data: {
  name?: string;
  description?: string;
  teacher?: User;
  group?: Group;
}) {
  if (!data.description || !data.group || !data.name || !data.teacher) {
    return "fill_data_full";
  }
  const res = await subjectService.post({
    ...data,
    groupEntityId: data.group.id,
    teacherId: data.teacher.id,
  });
  if (res.status !== 201) {
    return "error_in_data";
  } else {
    return "Created";
  }
}
