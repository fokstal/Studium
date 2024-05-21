import { DataTable } from "primereact/datatable";
import { TableWrapper } from "../data-tables";
import { Group } from "../../types";
import { Column } from "primereact/column";

type GroupTableProps = {
  data: Group[];
}

export function GroupTable({data}: GroupTableProps) {
  return (
    <TableWrapper>
      <DataTable value={data} resizableColumns>
        <Column field="name" header="Название"></Column>
        <Column field="description" header="Описание"></Column>
        <Column field="curator" header="Куратор"></Column>
        <Column field="auditoryName" header="Аудитория"></Column>
      </DataTable>
    </TableWrapper>
  );
}
