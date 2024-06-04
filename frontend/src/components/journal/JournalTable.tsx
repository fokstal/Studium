import { DataTable } from "primereact/datatable";
import { TableWrapper } from "../data-tables";
import { Column } from "primereact/column";

type JournalTableProps = {
  data: any[];
  columns: { header: string; field: string }[];
};

export function JournalTable({ data, columns }: JournalTableProps) {
  return (
    <TableWrapper>
      <DataTable value={data} scrollable paginator={data.length > 15} rows={15} resizableColumns>
        {columns.map(({ header, field }) => (
          <Column key={field} field={field} header={header} style={{padding: "10px 20px"}}></Column>
        ))}
      </DataTable>
    </TableWrapper>
  );
}
