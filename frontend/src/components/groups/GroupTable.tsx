import { DataTable } from "primereact/datatable";
import { TableWrapper } from "../data-tables";
import { Group } from "../../types";
import { Column } from "primereact/column";
import { colors } from "../ui-kit";
import { Box, Flex } from "@chakra-ui/react";
import { FaEdit, FaPlus, FaTrash } from "react-icons/fa";
import { Link } from "react-router-dom";
import { GroupService } from "../../services";

type GroupTableProps = {
  data: Group[];
};

const groupService = new GroupService();

export function GroupTable({ data }: GroupTableProps) {
  const deleteGroupHandler = (id: number) => {
    groupService.delete(id);
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
        <Column field="name" header="Название" sortable></Column>
        <Column field="description" header="Описание" sortable></Column>
        <Column field="curator" header="Куратор" sortable></Column>
        <Column field="auditoryName" header="Аудитория" sortable></Column>
        <Column
          body={(rowData) => (
            <Flex gap="10px" align="center">
              <Box
                onClick={() => deleteGroupHandler(rowData.id)}
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
                <Link to="/group/new">
                  <FaEdit color={colors.white} />
                </Link>
              </Box>
              <Box
                p="5px"
                bg={colors.green}
                borderRadius="5px"
                cursor="pointer"
              >
                <Link to="/group/new">
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
