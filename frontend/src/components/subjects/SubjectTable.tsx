import { DataTable } from "primereact/datatable";
import { TableWrapper } from "../data-tables";
import { Subject } from "../../types";
import { Column } from "primereact/column";
import { SubjectService } from "../../services";
import { Box, Flex } from "@chakra-ui/react";
import { colors } from "../ui-kit";
import { FaEdit, FaPlus, FaTrash } from "react-icons/fa";
import { Link } from "react-router-dom";

type SubjectTableProps = {
  data: Subject[];
};

const subjectService = new SubjectService();

export function SubjectTable({ data }: SubjectTableProps) {
  const deleteSubjectHandler = (id: number) => {
    subjectService.delete(id);
    window.location.reload();
  };
  return (
    <TableWrapper>
      <DataTable
        value={data}
        resizableColumns
        paginator={data.length > 7}
        rows={7}
      >
        <Column field="name" header="Название"></Column>
        <Column field="description" header="Описание"></Column>
        <Column field="groupId" header="Группа"></Column>
        <Column
          body={(rowData) => (
            <Flex gap="10px" align="center">
              <Box
                onClick={() => deleteSubjectHandler(rowData.id)}
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
                <Link to={`/subject/edit/${rowData.id}`}>
                  <FaEdit color={colors.white} />
                </Link>
              </Box>
              <Box
                p="5px"
                bg={colors.green}
                borderRadius="5px"
                cursor="pointer"
              >
                <Link to="/subject/new">
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
