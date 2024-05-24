import { DataTable } from "primereact/datatable";
import { TableWrapper } from "../data-tables";
import { Column } from "primereact/column";

type JournalTableProps = {
  data: any[];
  columns: { name: string; field: string }[];
};

export function JournalTable({ data, columns }: JournalTableProps) {
  console.log(data);
  return (
    <TableWrapper>
      <DataTable value={data} scrollable paginator={data.length > 7} rows={7}>
        <Column field="name" header="Имя"></Column>
        <Column field="24.4_0" header="24.04"></Column>
        <Column field="24.4_1" header="24.04"></Column>
        {columns.map(({ name, field }) => (
          <Column key={field} field={field} header={name}></Column>
        ))}
      </DataTable>
    </TableWrapper>
  );
}
