import { useContext } from "react";
import { GroupService } from "../../services";
import { LanguageContext, Translator } from "../../store";
import { User } from "../../types";

const groupService = new GroupService();

export async function createGroup(data: {
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
    return "fill_data_full";
  }
  const res = await groupService.post({ ...data, curatorId: data.curator.id });
  if (res.status !== 201) {
    return "error_in_data";
  } else {
    return "Created";
  }
}
