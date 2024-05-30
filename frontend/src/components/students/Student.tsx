import { useContext, useEffect, useState } from "react";
import { Group, Person, Student } from "../../types";
import {
  GroupService,
  PassportService,
  PersonService,
  StudentService,
} from "../../services";
import { Avatar, colors, Input } from "../../components/ui-kit";
import { Flex, HStack, Link, Text, VStack } from "@chakra-ui/react";
import { getAvatarPath, getPassportPath } from "../../lib/";
import {
  AiFillCheckCircle,
  AiFillCloseCircle,
  AiOutlineEye,
} from "react-icons/ai";
import { LanguageContext, Translator } from "../../store";

const studentService = new StudentService();
const personService = new PersonService();
const groupService = new GroupService();
const passportService = new PassportService();

type StudentComponentProps = {
  id: string;
};

export function StudentComponent({ id }: StudentComponentProps) {
  const [student, setStudent] = useState<Person & Group & Student>();
  const [passport, setPassport] = useState<string>();
  const { lang } = useContext(LanguageContext);

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
      ...studentData,
    };
    setStudent(student);

    if (localStorage.getItem("role") === "Student") return;
    const passports = await passportService.get();
    const passport = passports.find((p: any) => p.personId === personData.id);
    setPassport(passport?.scanFileName ?? null);
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
          {Translator[lang.name]["main_information"]}
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
          <Flex gap="10px" p="20px" bg={colors.darkGrey} borderRadius="5px" w="100%">
            <VStack gap="22px" align="end" p="7px 0" whiteSpace="nowrap">
              <Text>{Translator[lang.name]["name_student"]}:</Text>
              <Text>{Translator[lang.name]["date_birthday"]}:</Text>
              <Text>{Translator[lang.name]["student_group"]}:</Text>
            </VStack>
            <VStack gap="10px" align="start" w="100%">
              <Input
                value={`${student?.firstName} ${student?.middleName} ${student?.lastName}`}
                disabled
              />
              <Input
                value={student?.birthDate?.toString().slice(0, 10)}
                disabled
              />
              <Input value={student?.name} disabled />
            </VStack>
          </Flex>
        </Flex>
      </Flex>
      <Flex gap="20px">
        {localStorage.getItem("role") !== "Student" ? (
          <Flex
            bg={colors.white}
            direction="column"
            gap="20px"
            p="20px"
            borderRadius="5px"
            w="calc(50% - 10px)"
          >
            <Text fontSize="24px" fontWeight="bold">
              {Translator[lang.name]["passport_information"]}
            </Text>
            <HStack
              p="20px"
              bg={colors.darkGrey}
              borderRadius="5px"
              justify="space-between"
            >
              <Flex direction="column" gap="10px">
                <Text> {Translator[lang.name]["passport_information"]}</Text>
                <Text>
                  {passport ? (
                    <Flex gap="10px">
                      <AiFillCheckCircle color="blue" size="24px" />
                      <Text>{Translator[lang.name]["passport_load"]}</Text>
                    </Flex>
                  ) : (
                    <Flex gap="10px">
                      <AiFillCloseCircle color="red" size="24px" />
                      <Text>{Translator[lang.name]["passport_not_load"]}</Text>
                    </Flex>
                  )}
                </Text>
              </Flex>
              {passport ? (
                <Link href={getPassportPath(passport)} target="blank">
                  <AiOutlineEye size="24px" />
                </Link>
              ) : null}
            </HStack>
          </Flex>
        ) : null}
        <Flex
          bg={colors.white}
          direction="column"
          gap="20px"
          p="20px"
          borderRadius="5px"
          w={
            localStorage.getItem("role") === "Student"
              ? "100%"
              : "calc(50% - 10px)"
          }
        >
          <Text fontSize="24px" fontWeight="bold">
            {Translator[lang.name]["data_about_start"]}
          </Text>
          <VStack
            p="20px"
            bg={colors.darkGrey}
            borderRadius="5px"
            justify="space-between"
            align="start"
          >
            <Flex gap="10px" whiteSpace="nowrap" align="center" w="100%">
              <Text>{Translator[lang.name]["date_start"]}:</Text>
              <Input
                type="text"
                value={student?.addedDate?.toString().slice(0, 10)}
                disabled
              />
            </Flex>
            <Flex gap="10px" whiteSpace="nowrap" align="center" w="100%">
              <Text>{Translator[lang.name]["date_finish"]}:</Text>
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
