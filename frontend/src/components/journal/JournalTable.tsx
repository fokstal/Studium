import { DataTable } from "primereact/datatable";
import { TableWrapper } from "../data-tables";
import { Column } from "primereact/column";

type JournalTableProps = {
  data: any[];
  columns: { header: string; field: string }[];
};

export function JournalTable({ data, columns }: JournalTableProps) {
  console.log(data);
  return (
    <TableWrapper>
      <DataTable value={data} scrollable paginator={data.length > 7} rows={7}>
        {columns.map(({ header, field }) => (
          <Column key={field} field={field} header={header}></Column>
        ))}
      </DataTable>
    </TableWrapper>
  );
}
