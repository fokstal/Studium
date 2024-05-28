import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { TableWrapper } from "../data-tables/TableWrapper";
import { Student, StudentsTableData } from "../../types";
import { Box, Flex, Link } from "@chakra-ui/react";
import { colors } from "../ui-kit";
import { FaTrash, FaPlus, FaEdit } from "react-icons/fa";
import { PersonService, StudentService } from "../../services";

type StudentsTableProps = {
  data: StudentsTableData[];
  setSelectedStudent: Function;
  selectedStudent: (StudentsTableData & Student) | null;
};

const studentService = new StudentService();

export function StudentsTable({
  data,
  setSelectedStudent,
  selectedStudent,
}: StudentsTableProps) {
  const handleSelection = (student: StudentsTableData) => {
    setSelectedStudent(student);
  };

  const handleDeleteStudent = (e: MouseEvent, id: string) => {
    e.preventDefault();
    studentService.delete(id);
    window.location.reload();
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
        <Column
          body={(rowData) => (
            <Flex gap="10px" align="center">
              <Box
                onClick={(e) =>
                  handleDeleteStudent(e as unknown as MouseEvent, rowData.id)
                }
                p="5px"
                bg={colors.red}
                borderRadius="5px"
                cursor="pointer"
              >
                <FaTrash color={colors.white} />
              </Box>
              <Box
                p="5px"
                bg={colors.green}
                borderRadius="5px"
                cursor="pointer"
              >
                <Link href={`/students/edit/${rowData.id}`}>
                  <FaEdit color={colors.white} />
                </Link>
              </Box>
              <Box
                p="5px"
                bg={colors.green}
                borderRadius="5px"
                cursor="pointer"
              >
                <Link href="/students/new">
                  <FaPlus color={colors.white} />
                </Link>
              </Box>
            </Flex>
          )}
        ></Column>
      </DataTable>
    </TableWrapper>
  );
}
