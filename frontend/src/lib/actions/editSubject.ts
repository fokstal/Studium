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
  const { lang } = useContext(LanguageContext);
  if (!data.description || !data.group || !data.name || !data.teacher) {
    return Translator[lang.name]["fill_data_full"];
  }
  const res = await subjectService.put(id, {
    ...data,
    groupId: data.group.id,
    teacherId: data.teacher.id,
  });
  if (res.status !== 204) {
    return Translator[lang.name]["error_in_data"];
  } else {
    return "Created";
  }
}
