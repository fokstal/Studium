import { DataTable } from "primereact/datatable";
import { TableWrapper } from "../data-tables";
import { Column } from "primereact/column";

type JournalTableProps = {
  data: any[];
  columns: {name: string, field: string}[];
}

export function JournalTable({data, columns}: JournalTableProps) {
  return (
    <TableWrapper>
      <DataTable value={data} scrollable>
        {columns.map(({name, field}) => (
          <Column key={field} field={field} header={name}></Column>
        ))}
      </DataTable>
    </TableWrapper>
  )
}
