import { GroupService } from "../../services";
import { User } from "../../types";

const groupService = new GroupService();

export async function editGroup(id: number, data: {
  name?: string;
  curator?: User;
  auditoryName?: string;
  description?: string;
}) {
  if (
    !data?.name ||
    !data?.curator ||
    !data?.description ||
    !data?.description
  ) {
    return "Заполните данные полностью";
  }
  const res = await groupService.put(id, { ...data, curatorId: data.curator.id });
  if (res.status !== 201) {
    return "Данные введены не коректно, пожалуйста проверьте формат данных";
  } else {
    return "Created";
  }
}
