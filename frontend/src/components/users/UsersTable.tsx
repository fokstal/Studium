import { DataTable } from "primereact/datatable";
import { TableWrapper } from "../data-tables";
import { User } from "../../types";
import { Column } from "primereact/column";
import { useContext } from "react";
import { LanguageContext, Translator } from "../../store";

type UserTableProps = {
  data: User[];
};

export function UserTable({ data }: UserTableProps) {
  const { lang } = useContext(LanguageContext);
  return (
    <TableWrapper>
      <DataTable
        value={data}
        resizableColumns
        paginator={data.length > 7}
        rows={7}
      >
        <Column
          header={Translator[lang.name]["login"]}
          body={({ login }) => <>{login}</>}
        ></Column>
        <Column
          header={Translator[lang.name]["name"]}
          body={({ firstName, middleName, lastName }) => (
            <>{`${firstName} ${middleName} ${lastName}`}</>
          )}
        ></Column>
        <Column
          header={Translator[lang.name]["roles"]}
          body={({ roleList }) => (
            <>
              {roleList.map((role: { name: string }) => role.name).join(", ")}
            </>
          )}
        ></Column>
      </DataTable>
    </TableWrapper>
  );
}
