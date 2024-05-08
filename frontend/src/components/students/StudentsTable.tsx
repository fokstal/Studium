import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { TableWrapper } from "../data-tables/TableWrapper";
import { Student, StudentsTableData } from "../../types";

type StudentsTableProps = {
  data: StudentsTableData[];
  setSelectedStudent: Function;
  selectedStudent: (StudentsTableData & Student) | null;
};

export function StudentsTable({
  data,
  setSelectedStudent,
  selectedStudent,
}: StudentsTableProps) {
  const handleSelection = (student: StudentsTableData) => {
    setSelectedStudent(student);
  };

  return (
    <TableWrapper>
      <DataTable
        value={data}
        paginator
        rows={7}
        resizableColumns
        onSelectionChange={(e) => handleSelection(e.value as StudentsTableData)}
        selection={selectedStudent}
        selectionMode="single"
      >
        <Column field="name" header="Имя" sortable></Column>
        <Column field="groupName" header="Возраст" sortable></Column>
        <Column field="averageMark" header="Средний балл" sortable></Column>
      </DataTable>
    </TableWrapper>
  );
}
