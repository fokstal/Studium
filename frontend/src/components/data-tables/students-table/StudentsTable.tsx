import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { TableWrapper } from '../TableWrapper';
import { StudentsTableData } from '../../../types';

export function StudentsTable({data} : {data: StudentsTableData[]}) {
  return (
    <TableWrapper>
      <DataTable value={data} paginator rows={7} resizableColumns>
          <Column field="name" header="Имя" sortable></Column>
          <Column field="groupName" header="Возраст" sortable></Column>
          <Column field="averageMark" header="Средний балл" sortable></Column>
      </DataTable>
    </TableWrapper>
  )
}