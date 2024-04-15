import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { useEffect, useState } from "react";
import StudentService from "../../../services/StudentService";
import { TableWrapper } from '../TableWrapper';

export function StudentsTable() {
  const [students, setStudents] = useState([]);

  useEffect(() => {
      StudentService.getStudents().then((value) => setStudents(value));
  }, []);

  return (
    <TableWrapper>
      <DataTable value={students} paginator rows={7} resizableColumns>
          <Column field="name" header="Имя" sortable></Column>
          <Column field="age" header="Возраст" sortable></Column>
          <Column field="gender" header="Пол" sortable></Column>
          <Column field="gpa" header="Средний балл" sortable></Column>
      </DataTable>
    </TableWrapper>
  )
}