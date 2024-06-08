import { useContext } from "react";
import { SubjectService } from "../../services";
import { Group, User } from "../../types";
import { LanguageContext, Translator } from "../../store";

const subjectService = new SubjectService();

export async function editSubject(
  id: number,
  data: {
    name?: string;
    description?: string;
    teacher?: User;
    group?: Group;
  }
) {
  if (!data.description || !data.group || !data.name || !data.teacher) {
    return "fill_data_full";
  }
  const res = await subjectService.put(id, {
    ...data,
    groupEntityId: data.group.id,
    teacherId: data.teacher.id,
  });
  if (res.status !== 204) {
    return "error_in_data";
  } else {
    return "Created";
  }
}
