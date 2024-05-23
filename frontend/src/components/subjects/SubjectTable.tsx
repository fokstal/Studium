import { DataTable } from "primereact/datatable";
import { TableWrapper } from "../data-tables";
import { Subject } from "../../types";
import { Column } from "primereact/column";

type SubjectTableProps = {
  data: Subject[];
};

export function SubjectTable({ data }: SubjectTableProps) {
  return (
    <TableWrapper>
      <DataTable
        value={data}
        resizableColumns
        paginator={data.length > 7}
        rows={7}
      >
        <Column field="name" header="Название"></Column>
        <Column field="description" header="Описание"></Column>
        <Column field="groupId" header="Группа"></Column>
      </DataTable>
    </TableWrapper>
  );
}
