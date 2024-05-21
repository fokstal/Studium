import { DataTable } from "primereact/datatable";
import { TableWrapper } from "../data-tables";
import { Subject } from "../../types";
import { Column } from "primereact/column";

type SubjectTableProps = {
  data: Subject[];
}

export function SubjectTable({data}: SubjectTableProps) {
  return (
    <TableWrapper>
      <DataTable value={data} resizableColumns>
        <Column field="name" header="Название"></Column>
        <Column field="description" header="Описание"></Column>
        <Column field="teacherId" header="Учитель"></Column>
        <Column field="groupId" header="Группа"></Column>
      </DataTable>
    </TableWrapper>
  );
}
