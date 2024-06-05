import { DataTable } from "primereact/datatable";
import { TableWrapper } from "../data-tables";
import { Group } from "../../types";
import { Column } from "primereact/column";
import { colors } from "../ui-kit";
import {
  Box,
  Flex,
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalFooter,
  ModalBody,
  ModalCloseButton,
  Text,
} from "@chakra-ui/react";
import { Button } from "../ui-kit";
import { FaEdit, FaPlus, FaTrash } from "react-icons/fa";
import { Link } from "react-router-dom";
import { useContext, useState } from "react";
import { AiOutlineCloseSquare } from "react-icons/ai";
import { LanguageContext, Translator } from "../../store";
import { GroupService } from "../../services";
import { useRoles } from "../../hooks";

type GroupTableProps = {
  data: Group[];
};

const groupService = new GroupService();

export function GroupTable({ data }: GroupTableProps) {
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [groupToDelete, setGroupToDelete] = useState<number>(0);
  const { lang } = useContext(LanguageContext);
  const roles = useRoles();

  const deleteGroupHandler = (id: number) => {
    setGroupToDelete(id);
    setIsDeleteModalOpen(true);
  };

  const confirmDeleteGroup = () => {
    groupService.delete(groupToDelete);
    setIsDeleteModalOpen(false);
    window.location.reload();
  };

  const cancelDeleteGroup = () => {
    setIsDeleteModalOpen(false);
    setGroupToDelete(0);
  };

  return (
    <>
      <TableWrapper>
        <DataTable
          value={data}
          resizableColumns
          paginator={data.length > 7}
          rows={7}
        >
          <Column
            field="name"
            header={Translator[lang.name]["group_name"]}
            sortable
          ></Column>
          <Column
            field="description"
            header={Translator[lang.name]["group_description"]}
            sortable
          ></Column>
          <Column
            field="auditoryName"
            header={Translator[lang.name]["group_auditory"]}
            sortable
          ></Column>
          {roles.includes("Admin") || roles.includes("Secretar") ? (
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
                    <Link to={`/group/edit/${rowData.id}`}>
                      <FaEdit color={colors.white} />
                    </Link>
                  </Box>
                </Flex>
              )}
            ></Column>
          ) : null}
        </DataTable>
      </TableWrapper>
      <Modal isOpen={isDeleteModalOpen} onClose={cancelDeleteGroup} isCentered>
        <ModalOverlay />
        <ModalContent>
          <ModalHeader
            display="flex"
            justifyContent="flex-end"
            alignItems="center"
          >
            <AiOutlineCloseSquare
              cursor="pointer"
              fontSize="24px"
              color={colors.darkGreen}
              onClick={cancelDeleteGroup}
            />
          </ModalHeader>
          <ModalBody justifyContent="center">
            <Text fontSize="24px" fontWeight="bold" textAlign="center">
              {Translator[lang.name]["confirm_delete"]}?
            </Text>
          </ModalBody>
          <ModalFooter justifyContent="center">
            <Button onClick={confirmDeleteGroup}>
              {Translator[lang.name]["confirm_delete"]}
            </Button>
          </ModalFooter>
        </ModalContent>
      </Modal>
    </>
  );
}
