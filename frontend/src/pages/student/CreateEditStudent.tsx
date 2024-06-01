import { Box, Flex, HStack, Text, VStack } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import {
  Button,
  ImagePicker,
  Input,
  Select,
  colors,
} from "../../components/ui-kit";
import { Group, Person, Student } from "../../types";
import {
  useEffect,
  useState,
  useTransition,
  ChangeEvent,
  useContext,
} from "react";
import { GroupService, StudentService, PersonService } from "../../services";
import { createStudent, editStudent } from "../../lib";
import { useNavigate, useParams } from "react-router-dom";
import { LanguageContext, Translator } from "../../store";

const groupService = new GroupService();
const studentService = new StudentService();
const personService = new PersonService();

export function CreateEditStudent() {
  const [studentData, setStudentData] = useState<(Student & Person) | null>(
    null
  );
  const [avatar, setAvatar] = useState<any>();
  const [groups, setGroups] = useState<Group[]>();
  const [isPending, startTransaction] = useTransition();
  const [passport, setPassport] = useState<File>();
  const [errorMessage, setErrorMessage] = useState<string>("");
  const navigator = useNavigate();
  const { lang } = useContext(LanguageContext);
  const { id } = useParams();

  const sexOptions = [
    { name: Translator[lang.name]["woman"], code: 0 },
    { name: Translator[lang.name]["man"], code: 1 },
  ];

  const updateData = async () => {
    const groups = await groupService.get();
    if (id) {
      const student = await studentService.getById(id);
      const person = await personService.getById(student.personId);

      setStudentData({
        ...student,
        ...person,
        sex: sexOptions[person.sex],
        group: groups.find((g: Group) => g.id === student.groupId),
        birthDate: person.birthDate.split("T")[0],
        addedDate: student.addedDate.split("T")[0],
        removedDate: student.removedDate.split("T")[0],
      });
    }
    setGroups(groups);
  };

  useEffect(() => {
    updateData();
  }, []);

  const handleSexSelectChange = (value: { name: string; code: boolean }) => {
    handleInputChange("sex", value);
  };

  const handleGroupSelectChange = (value: Group) => {
    handleInputChange("group", value);
  };

  const handleInputChange = (field: string, value: any) => {
    setStudentData((prevData: any) => ({
      ...prevData,
      [field]: value,
    }));
  };

  const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      setPassport(file);
    }
  };

  const handleSubmit = () => {
    if (!studentData) return;
    startTransaction(() => {
      if (id) {
        editStudent(
          {
            ...studentData,
            sex: (studentData.sex as unknown as { name: string; code: boolean })
              .code,
            avatarFileName: avatar,
          },
          passport
        )
          .then(() => navigator("/students"))
          .catch(() => setErrorMessage(Translator[lang.name]["error_in_data"]));
      } else {
        createStudent(
          {
            ...studentData,
            sex: (studentData.sex as unknown as { name: string; code: boolean })
              .code,
            avatarFileName: avatar,
          },
          passport
        )
          .then(() => navigator("/students"))
          .catch(() => setErrorMessage(Translator[lang.name]["error_in_data"]));
      }
    });
  };

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
          {id
            ? Translator[lang.name]["edit_student"]
            : Translator[lang.name]["create_new_student"]}
        </Text>
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
          <Flex
            align="center"
            gap="20px"
            p="20px"
            bg={colors.darkGrey}
            borderRadius="5px"
          >
            <Box w="max-content">
              <ImagePicker setFile={setAvatar} />
            </Box>
            <Flex
              direction="row"
              p="20px"
              bg={colors.darkGrey}
              borderRadius="5px"
              gap="10px"
              w="100%"
              justify="center"
            >
              <Flex
                direction="column"
                gap="12.5px"
                align="end"
                whiteSpace="nowrap"
              >
                <Text p="6px 0">{Translator[lang.name]["first_name"]}:</Text>
                <Text p="6px 0">{Translator[lang.name]["middle_name"]}:</Text>
                <Text p="6px 0">{Translator[lang.name]["last_name"]}:</Text>
                <Text p="6px 0">{Translator[lang.name]["date_birthday"]}:</Text>
                <Text p="6px 0">{Translator[lang.name]["sex"]}:</Text>
                <Text p="6px 0">{Translator[lang.name]["student_group"]}:</Text>
              </Flex>
              <Flex direction="column" gap="10px" w="100%">
                <Input
                  value={studentData?.firstName}
                  onChange={(e) =>
                    handleInputChange("firstName", e.target.value)
                  }
                />
                <Input
                  value={studentData?.middleName}
                  onChange={(e) =>
                    handleInputChange("middleName", e.target.value)
                  }
                />
                <Input
                  value={studentData?.lastName}
                  onChange={(e) =>
                    handleInputChange("lastName", e.target.value)
                  }
                />
                <Input
                  value={studentData?.birthDate?.toLocaleString()}
                  type="date"
                  onChange={(e) =>
                    handleInputChange("birthDate", e.target.value)
                  }
                />
                <Select
                  value={studentData?.sex}
                  setValue={handleSexSelectChange}
                  placeholder={Translator[lang.name]["select_sex"]}
                  options={sexOptions}
                />
                <Select
                  value={studentData?.group}
                  setValue={handleGroupSelectChange}
                  placeholder={Translator[lang.name]["select_group"]}
                  options={groups ?? []}
                />
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
              {Translator[lang.name]["passport_information"]}
            </Text>
            <HStack
              p="20px"
              bg={colors.darkGrey}
              borderRadius="5px"
              justify="space-between"
            >
              <Flex direction="column" gap="10px">
                <Text>{Translator[lang.name]["passport_information"]}:</Text>
                <Input type="file" onChange={(e) => handleFileChange(e)} />
              </Flex>
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
              {Translator[lang.name]["data_about_start"]}
            </Text>
            <VStack
              p="20px"
              bg={colors.darkGrey}
              borderRadius="5px"
              justify="space-between"
              align="start"
            >
              <Flex gap="10px" whiteSpace="nowrap" align="center">
                <Text>{Translator[lang.name]["date_start"]}:</Text>
                <Input
                  type="date"
                  value={studentData?.addedDate}
                  onChange={(e) =>
                    setStudentData({
                      ...studentData,
                      addedDate: e.target.value,
                    })
                  }
                />
              </Flex>
              <Flex gap="10px" whiteSpace="nowrap">
                <Text>{Translator[lang.name]["date_finish"]}:</Text>
                <Input
                  type="date"
                  value={studentData?.removedDate}
                  onChange={(e) =>
                    setStudentData({
                      ...studentData,
                      removedDate: e.target.value,
                    })
                  }
                />
              </Flex>
            </VStack>
          </Flex>
        </Flex>
        <Box fontSize="24px" color={colors.red}>
          {errorMessage}
        </Box>
        <Button onClick={handleSubmit} disabled={isPending}>
          {id ? Translator[lang.name]["edit"] : Translator[lang.name]["create"]}
        </Button>
      </Flex>
    </BaseLayout>
  );
}
