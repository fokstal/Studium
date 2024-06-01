import { useContext } from "react";
import { GroupService } from "../../services";
import { User } from "../../types";
import { LanguageContext, Translator } from "../../store";

const groupService = new GroupService();

export async function editGroup(
  id: number,
  data: {
    name?: string;
    curator?: User;
    auditoryName?: string;
    description?: string;
  }
) {
  const { lang } = useContext(LanguageContext);
  if (
    !data?.name ||
    !data?.curator ||
    !data?.description ||
    !data?.description
  ) {
    return "fill_data_full";
  }
  const res = await groupService.put(id, {
    ...data,
    curatorId: data.curator.id,
  });
  if (res.status !== 201) {
    return "error_in_data";
  } else {
    return "Created";
  }
}
