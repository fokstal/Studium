import { useEffect, useState } from "react";
import { Group, Person, Student } from "../../types";
import { GroupService, PersonService, StudentService } from "../../services";
import { Avatar, colors, Input } from "../../components/ui-kit";
import { Box, Flex, HStack, Text, VStack } from "@chakra-ui/react";
import { getAvatarPath } from "../../lib/";
import {
  AiFillCheckCircle,
  AiFillCloseCircle,
  AiOutlineEye,
} from "react-icons/ai";

const studentService = new StudentService();
const personService = new PersonService();
const groupService = new GroupService();

type StudentComponentProps = {
  id: string;
};

export function StudentComponent({ id }: StudentComponentProps) {
  const [student, setStudent] = useState<Person & Group & Student>();

  const getPersonData = async (personId: number): Promise<Person> => {
    return await personService.getById(personId);
  };
  
  const getGroupData = async (groupId: number): Promise<Group> => {
    return await groupService.getById(groupId);
  };
  
  const updateStudent = async () => {
    if (!id) return;
    const studentData = await studentService.getById(id);
    const [personData, groupData] = await Promise.all([
      getPersonData(studentData.personId),
      getGroupData(studentData.groupId),
    ]);
    const student = {
      ...personData,
      ...groupData,
      ...studentData
    };
    setStudent(student);
  };

  useEffect(() => {
    updateStudent();
  }, []);

  return (
    <>
      <Flex
        bg={colors.white}
        direction="column"
        gap="20px"
        p="20px"
        borderRadius="5px"
      >
        <Text fontSize="24px" fontWeight="bold">
          Основная информация
        </Text>
        <Flex align="center" gap="20px" justify="center">
          <Avatar
            size="lg"
            img={
              student?.avatarFileName
                ? getAvatarPath(student?.avatarFileName)
                : ""
            }
          />
          <Flex
            direction="column"
            p="20px"
            bg={colors.darkGrey}
            borderRadius="5px"
            gap="10px"
            w="max-content"
            align="stretch"
          >
            <Flex gap="10px" align="center">
              <Text>Имя:</Text>
              <Input
                value={`${student?.firstName} ${student?.middleName} ${student?.lastName}`}
                disabled
              />
            </Flex>
            <Flex gap="10px" align="center">
              <Text>Дата рождения:</Text>
              <Input
                value={student?.birthDate.toString().slice(0, 10)}
                disabled
              />
            </Flex>
            <Flex gap="10px" align="center">
              <Text>Группа:</Text>
              <Input value={student?.name} disabled />
            </Flex>
          </Flex>
        </Flex>
      </Flex>
      <Flex gap="20px">
        <Flex
          bg={colors.white}
          direction="column"
          gap="20px"
          p="20px"
          borderRadius="5px"
          w="calc(50% - 10px)"
        >
          <Text fontSize="24px" fontWeight="bold">
            Паспортные данные
          </Text>
          <HStack
            p="20px"
            bg={colors.darkGrey}
            borderRadius="5px"
            justify="space-between"
          >
            <Flex direction="column" gap="10px">
              <Text>Паспортные данные</Text>
              <Text>
                {student?.passport ? (
                  <Flex gap="10px">
                    <AiFillCheckCircle color="blue" size="24px" />
                    <Text>Данные успешно загружены</Text>
                  </Flex>
                ) : (
                  <Flex gap="10px">
                    <AiFillCloseCircle color="red" size="24px" />
                    <Text>Данные не загружены</Text>
                  </Flex>
                )}
              </Text>
            </Flex>
            <Box>
              <AiOutlineEye size="24px" />
            </Box>
          </HStack>
        </Flex>
        <Flex
            bg={colors.white}
            direction="column"
            gap="20px"
            p="20px"
            borderRadius="5px"
            w="calc(50% - 10px)"
          >
            <Text fontSize="24px" fontWeight="bold">
              Данные о зачислении
            </Text>
            <VStack
              p="20px"
              bg={colors.darkGrey}
              borderRadius="5px"
              justify="space-between"
              align="start"
            >
              <Flex gap="10px" whiteSpace="nowrap" align="center">
                <Text>Дата зачисления:</Text>
                <Input
                  type="text"
                  value={student?.addedDate?.toString().slice(0, 10)}
                  disabled
                />
              </Flex>
              <Flex gap="10px" whiteSpace="nowrap" align="center">
                <Text>Дата окончания:</Text>
                <Input
                  type="text"
                  value={student?.removedDate?.toString().slice(0, 10)}
                  disabled
                />
              </Flex>
            </VStack>
          </Flex>
      </Flex>
    </>
  );
}
