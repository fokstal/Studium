import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { TableWrapper } from "../data-tables/TableWrapper";
import { Student, StudentsTableData } from "../../types";
import {
  Box,
  Flex,
  Link,
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
import { FaTrash, FaPlus, FaEdit } from "react-icons/fa";
import { GradeService, PersonService, StudentService } from "../../services";
import { useContext, useEffect, useState } from "react";
import { AiOutlineCloseSquare } from "react-icons/ai";
import { LanguageContext, Translator } from "../../store";

type StudentsTableProps = {
  data: StudentsTableData[];
  setSelectedStudent: Function;
  selectedStudent: (StudentsTableData & Student) | null;
};

const studentService = new StudentService();
const gradeService = new GradeService();

export function StudentsTable({
  data,
  setSelectedStudent,
  selectedStudent,
}: StudentsTableProps) {
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [studentToDelete, setStudentToDelete] = useState<string>("");
  const { lang } = useContext(LanguageContext);

  const handleDeleteStudent = (e: MouseEvent, id: string) => {
    e.preventDefault();
    setStudentToDelete(id);
    setIsDeleteModalOpen(true);
  };

  const handleSelection = (student: StudentsTableData) => {
    setSelectedStudent(student);
  };

  const confirmDeleteStudent = () => {
    studentService.delete(studentToDelete);
    setIsDeleteModalOpen(false);
    window.location.reload();
  };

  const cancelDeleteStudent = () => {
    setIsDeleteModalOpen(false);
    setStudentToDelete("");
  };

  return (
    <>
      <TableWrapper>
        <DataTable
          value={data}
          paginator
          rows={7}
          resizableColumns
          onSelectionChange={(e) =>
            handleSelection(e.value as StudentsTableData)
          }
          selection={selectedStudent}
          selectionMode="single"
        >
          <Column
            field="name"
            header={Translator[lang.name]["name_student"]}
            sortable
          ></Column>
          <Column
            field="groupName"
            header={Translator[lang.name]["student_group"]}
            sortable
          ></Column>
          <Column
            header={Translator[lang.name]["average_grade"]}
            sortable
            body={(rowData) => {
              const [averageGrade, setAverageGrade] = useState<number | null>(
                null
              );

              useEffect(() => {
                const fetchAverageGrade = async () => {
                  const grade = await gradeService.averageGrade(rowData.id);
                  setAverageGrade(grade);
                };
                fetchAverageGrade();
              }, [rowData.id]);

              return (
                <>{averageGrade !== null ? averageGrade : "Calculating"}</>
              );
            }}
          ></Column>
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

      <Modal
        isOpen={isDeleteModalOpen}
        onClose={cancelDeleteStudent}
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
              onClick={cancelDeleteStudent}
            />
          </ModalHeader>
          <ModalBody justifyContent="center">
            <Text fontSize="24px" fontWeight="bold" textAlign="center">
              {Translator[lang.name]["confirm_delete"]}?
            </Text>
          </ModalBody>
          <ModalFooter justifyContent="center">
            <Button onClick={confirmDeleteStudent}>
              {Translator[lang.name]["confirm_delete"]}
            </Button>
          </ModalFooter>
        </ModalContent>
      </Modal>
    </>
  );
}
