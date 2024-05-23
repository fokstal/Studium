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
import { useEffect, useState, useTransition, ChangeEvent } from "react";
import { GroupService } from "../../services";
import { createStudent, editStudent } from "../../lib";

type CreateEditStudentProps = {
  studentToEdit?: Student & Person;
};

const groupService = new GroupService();

export function CreateEditStudent({ studentToEdit }: CreateEditStudentProps) {
  const [studentData, setStudentData] = useState<(Student & Person) | null>(
    null
  );
  const [avatar, setAvatar] = useState<any>();
  const [groups, setGroups] = useState<Group[]>();
  const [isPending, startTransaction] = useTransition();
  const [passport, setPassport] = useState<File>();
  const [additionalDate, setAdditionalDate] = useState<{
    addedDate?: string;
    removeDate?: string;
  }>({});

  useEffect(() => {
    groupService.get().then((groups) => setGroups(groups));
    if (!studentToEdit) return;
    setStudentData(studentToEdit);
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
      if (studentToEdit) {
        editStudent({
          ...studentData,
          sex: (studentData.sex as unknown as { name: string; code: boolean })
            .code,
          avatarFileName: avatar,
        });
      } else {
        createStudent({
          ...studentData,
          sex: (studentData.sex as unknown as { name: string; code: boolean })
            .code,
          avatarFileName: avatar,
        }, additionalDate, passport,);
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
          {studentToEdit
            ? "Изменение данных о студенте"
            : "Cоздание нового студента"}
        </Text>
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
                <Text p="6px 0">Имя:</Text>
                <Text p="6px 0">Отчество:</Text>
                <Text p="6px 0">Фамилия:</Text>
                <Text p="6px 0">Дата рождения:</Text>
                <Text p="6px 0">Пол:</Text>
                <Text p="6px 0">Учебная группа:</Text>
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
                  placeholder=""
                  options={[
                    { name: "Мужской", code: 1 },
                    { name: "Женский", code: 0 },
                  ]}
                />
                <Select
                  value={studentData?.group}
                  setValue={handleGroupSelectChange}
                  placeholder="Выберите учебную группу"
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
              Паспортные данные
            </Text>
            <HStack
              p="20px"
              bg={colors.darkGrey}
              borderRadius="5px"
              justify="space-between"
            >
              <Flex direction="column" gap="10px">
                <Text>Паспортные данные:</Text>
                <Input type="file" onChange={(e) => handleFileChange(e)}/>
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
                  type="date"
                  value={additionalDate.addedDate}
                  onChange={(e) =>
                    setAdditionalDate({
                      ...additionalDate,
                      addedDate: e.target.value,
                    })
                  }
                />
              </Flex>
              <Flex gap="10px" whiteSpace="nowrap">
                <Text>Дата окончания:</Text>
                <Input
                  type="date"
                  value={additionalDate.removeDate}
                  onChange={(e) =>
                    setAdditionalDate({
                      ...additionalDate,
                      removeDate: e.target.value,
                    })
                  }
                />
              </Flex>
            </VStack>
          </Flex>
        </Flex>
        <Button onClick={handleSubmit} disabled={isPending}>
          {studentToEdit ? "Изменить" : "Создать"}
        </Button>
      </Flex>
    </BaseLayout>
  );
}
