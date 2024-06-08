import { useContext, useEffect, useState } from "react";
import { Group, Person, Student } from "../../types";
import {
  GroupService,
  PassportService,
  PersonService,
  StudentService,
} from "../../services";
import { Avatar, colors, Input } from "../../components/ui-kit";
import {
  Box,
  Flex,
  HStack,
  Popover,
  PopoverBody,
  PopoverContent,
  PopoverTrigger,
  Text,
  VStack,
} from "@chakra-ui/react";
import { getAvatarPath } from "../../lib/";
import {
  AiFillCheckCircle,
  AiFillCloseCircle,
  AiOutlineEye,
} from "react-icons/ai";
import { LanguageContext, Translator } from "../../store";
import { useRoles } from "../../hooks";
import { Image } from "primereact/image";

const studentService = new StudentService();
const personService = new PersonService();
const groupService = new GroupService();
const passportService = new PassportService();

type StudentComponentProps = {
  id: string;
};

export function StudentComponent({ id }: StudentComponentProps) {
  const [student, setStudent] = useState<Person & Group & Student>();
  const [passport, setPassport] = useState<Blob>();
  const roles = useRoles();
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
      getPersonData(studentData.personEntityId),
      getGroupData(studentData.groupEntityId),
    ]);
    const student = {
      ...personData,
      ...groupData,
      ...studentData,
    };
    setStudent(student);

    if (roles.includes("Student")) return;
    const scan = await passportService.getScanFile(
      personData.passportEntity?.scanFileName || "",
      personData.id || 0
    );
    setPassport(scan || "");
    console.log(scan);
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
          <Flex
            gap="10px"
            p="20px"
            bg={colors.darkGrey}
            borderRadius="5px"
            w="100%"
          >
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
        {!roles.includes("Student") ? (
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
                <Popover>
                  <PopoverTrigger>
                    <Box onClick={() => {}}>
                      <AiOutlineEye size="24px" />
                    </Box>
                  </PopoverTrigger>
                  <PopoverContent>
                    <PopoverBody>
                      <Image 
                        preview
                        src={URL.createObjectURL(passport)}
                      />
                    </PopoverBody>
                  </PopoverContent>
                </Popover>
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
          w={roles.includes("Student") ? "100%" : "calc(50% - 10px)"}
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
