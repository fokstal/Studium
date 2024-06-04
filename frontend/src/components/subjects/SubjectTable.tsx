import { DataTable } from "primereact/datatable";
import { TableWrapper } from "../data-tables";
import { Subject } from "../../types";
import { Column } from "primereact/column";
import { SubjectService } from "../../services";
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
import { Button, colors } from "../ui-kit";
import { FaEdit, FaPlus, FaTrash } from "react-icons/fa";
import { Link } from "react-router-dom";
import { useContext, useState } from "react";
import { AiOutlineCloseSquare } from "react-icons/ai";
import { LanguageContext, Translator } from "../../store";

type SubjectTableProps = {
  data: Subject[];
};

const subjectService = new SubjectService();

export function SubjectTable({ data }: SubjectTableProps) {
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [subjectToDelete, setSubjectToDelete] = useState<number>(0);
  const { lang } = useContext(LanguageContext);

  const deleteSubjectHandler = (id: number) => {
    setSubjectToDelete(id);
    setIsDeleteModalOpen(true);
  };

  const confirmDeleteSubject = () => {
    subjectService.delete(subjectToDelete);
    setIsDeleteModalOpen(false);
    window.location.reload();
  };

  const cancelDeleteSubject = () => {
    setIsDeleteModalOpen(false);
    setSubjectToDelete(0);
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
            header={Translator[lang.name]["subject_name"]}
          ></Column>
          <Column
            field="description"
            header={Translator[lang.name]["subject_description"]}
          ></Column>
          <Column
            field="groupId"
            header={Translator[lang.name]["subject_group"]}
          ></Column>
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
              </Flex>
            )}
          ></Column>
        </DataTable>
      </TableWrapper>
      <Modal
        isOpen={isDeleteModalOpen}
        onClose={cancelDeleteSubject}
        isCentered
      >
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
              onClick={cancelDeleteSubject}
            />
          </ModalHeader>
          <ModalBody justifyContent="center">
            <Text fontSize="24px" fontWeight="bold" textAlign="center">
              {Translator[lang.name]["confirm_delete"]}?
            </Text>
          </ModalBody>
          <ModalFooter justifyContent="center">
            <Button onClick={confirmDeleteSubject}>
              {Translator[lang.name]["confirm_delete"]}
            </Button>
          </ModalFooter>
        </ModalContent>
      </Modal>
    </>
  );
}
