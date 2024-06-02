import {
  Box,
  Flex,
  InputGroup,
  InputLeftElement,
  Text,
  Input as ChakraInput,
} from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { colors } from "../../components/ui-kit";
import { useContext, useEffect, useState } from "react";
import { AiOutlineFilter } from "react-icons/ai";
import { FaSearch } from "react-icons/fa";
import { Group, Student, StudentsTableData } from "../../types";
import { GroupService, PersonService, StudentService } from "../../services";
import {
  Filters,
  SelectedStudent,
  StudentsTable,
} from "../../components/students";
import { LanguageContext, Translator } from "../../store";
import { filterStudents } from "../../lib";

const studentService = new StudentService();
const groupService = new GroupService();
const personService = new PersonService();

export function Students() {
  const [isOpenToggleFilters, setIsOpenToggleFilters] =
    useState<boolean>(false);
  const [filters, setFilters] = useState<{
    group: Group | null;
    startDate: string;
    endDate: string;
    finish: boolean;
    start: boolean;
  }>({
    group: null,
    startDate: "",
    endDate: "",
    finish: false,
    start: false,
  });
  const [search, setSearch] = useState<string>("");
  const [students, setStudents] = useState<StudentsTableData[]>();
  const [selectedStudent, setSelectedStudent] = useState<
    (StudentsTableData & Student) | null
  >(null);
  const { lang } = useContext(LanguageContext);

  const handleOpenToggleColumns = () => {
    if (selectedStudent) setSelectedStudent(null);
    setIsOpenToggleFilters(!isOpenToggleFilters);
  };

  const handleOpenSelectedStudent = (
    value: (StudentsTableData & Student) | null
  ) => {
    if (isOpenToggleFilters) setIsOpenToggleFilters(false);
    setSelectedStudent(value);
  };

  const updateStudents = async () => {
    const students: Student[] = await studentService.get();

    const studentsForTable = await Promise.all(
      students.map(async (student) => {
        const person = await personService.getById(student.personEntityId || 0);
        const group = await groupService.getById(student.groupEntityId || 0);
        return {
          ...student,
          name: `${person.firstName} ${person.lastName} ${person.middleName}`,
          groupName: group.name,
          averageMark: 6.5,
        };
      })
    );
    setStudents(filterStudents(studentsForTable, filters, search));
  };

  useEffect(() => {
    updateStudents();
  }, [filters, search]);

  return (
    <BaseLayout bg={colors.darkGrey}>
      <Flex
        w="100%"
        minH="calc(100vh - 100px)"
        p="80px 0px"
        direction="column"
        gap="10px"
      >
        <Text color={colors.black} fontSize="32px" as="h1" fontWeight="bold">
          {Translator[lang.name]["student_list"]}
        </Text>
        <Text color={colors.grey} fontSize="20px" as="b">
          {Translator[lang.name]["click_student"]}
        </Text>
        <Flex
          borderRadius="5px"
          bg={colors.white}
          w="100%"
          p="20px"
          direction="column"
          gap="20px"
        >
          <Flex gap="10px">
            <Flex
              gap="5px"
              align="center"
              cursor="pointer"
              onClick={handleOpenToggleColumns}
            >
              <Text userSelect="none" fontSize="16px">
                {Translator[lang.name]["filters"]}
              </Text>
              <AiOutlineFilter size="16px" />
            </Flex>
            <InputGroup alignItems="center">
              <InputLeftElement>
                <FaSearch />
              </InputLeftElement>
              <ChakraInput
                placeholder={Translator[lang.name]["search"]}
                borderColor={colors.lightGrey}
                _hover={{ borderColor: colors.green }}
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                _focusVisible={{ boxShadow: "none" }}
              />
            </InputGroup>
          </Flex>
          <Flex gap="20px" align="start">
            {isOpenToggleFilters ? <Filters filters={filters} setFilters={setFilters}/> : null}
            <Box w="100%">
              <StudentsTable
                data={students!}
                setSelectedStudent={handleOpenSelectedStudent}
                selectedStudent={selectedStudent}
              />
            </Box>
            <Box w="340">
              {selectedStudent ? (
                <SelectedStudent
                  id={selectedStudent.personEntityId || 0}
                  studentId={selectedStudent.id || 0}
                  setSelectedStudent={handleOpenSelectedStudent}
                  group={selectedStudent.groupName}
                />
              ) : null}
            </Box>
          </Flex>
        </Flex>
      </Flex>
    </BaseLayout>
  );
}
