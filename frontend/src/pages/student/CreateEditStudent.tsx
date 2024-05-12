import { Flex, HStack, Text } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { Button, Input, Select, colors } from "../../components/ui-kit";
import { Group, Person, Student } from "../../types";
import { useEffect, useState, useTransition } from "react";
import { GroupService } from "../../services";
import { createStudent } from "../../lib";

type CreateEditStudentProps = {
  studentToEdit?: Student & Person;
};

const groupService = new GroupService();

export function CreateEditStudent({ studentToEdit }: CreateEditStudentProps) {
  const [studentData, setStudentData] = useState<(Student & Person) | null>(
    null
  );
  const [groups, setGroups] = useState<Group[]>();
  const [isPending, startTransaction] = useTransition();

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

  const handleSubmit = () => {
    if (!studentData) return;
    startTransaction(() => {
      createStudent({
        ...studentData,
        sex: (studentData.sex as unknown as { name: string; code: boolean })
          .code,
      });
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
            {/* TODO: CREATE IMAGE PICKER COMPONENT */}
            <Flex
              direction="row"
              p="20px"
              bg={colors.darkGrey}
              borderRadius="5px"
              gap="10px"
              w="100%"
              justify="center"
            >
              <Flex direction="column" gap="12.5px" align="end">
                <Text p="6px 0">Имя:</Text>
                <Text p="6px 0">Отчество:</Text>
                <Text p="6px 0">Фамилия:</Text>
                <Text p="6px 0">Дата рождения:</Text>
                <Text p="6px 0">Пол:</Text>
                <Text p="6px 0">Учебная группа:</Text>
              </Flex>
              <Flex direction="column" gap="10px">
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
        <Flex>
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
                <Input type="file" />
              </Flex>
            </HStack>
          </Flex>
        </Flex>
        <Button onClick={handleSubmit} disabled={isPending}>
          {studentToEdit ? "Изменить" : "Создать"}
        </Button>
      </Flex>
    </BaseLayout>
  );
}
