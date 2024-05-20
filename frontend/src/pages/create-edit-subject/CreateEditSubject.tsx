import { Box, Flex, Text, Textarea, VStack } from "@chakra-ui/react";
import { Button, Input, Select, colors } from "../../components/ui-kit";
import { BaseLayout } from "../../layouts";
import { GroupService, SubjectService } from "../../services";
import { useEffect, useState } from "react";
import { Group } from "../../types";
import { useNavigate } from "react-router-dom";

const groupService = new GroupService();
const subjectService = new SubjectService();

export function CreateEditSubject() {
  const [errorMessage, setErrorMessage] = useState<string | null>();
  const [groups, setGroups] = useState<Group[]>();
  const [data, setData] = useState<{
    name?: string;
    description?: string;
    teacherName?: string;
    group?: Group;
  }>({});
  const navigator = useNavigate();

  useEffect(() => {
    groupService.get().then((value) => setGroups(value));
  }, []);

  useEffect(() => {
    setErrorMessage(null);
  }, [data]);

  const handleGroupSelect = (value: Group) => {
    setData({ ...data, group: value });
  };

  const handleSubmit = () => {
    if (!data.description || !data.group || !data.name || !data.teacherName) {
      return setErrorMessage("Заполните данные полностью");
    }
    subjectService.post({ ...data, groupId: data.group.id }).then((res) => {
      if (res.status !== 201) {
        setErrorMessage(
          "Данные введены не коректно, пожалуйста проверьте формат данных"
        );
      } else {
        navigator("/students");
      }
    });
  };

  return (
    <BaseLayout bg={colors.darkGrey}>
      <VStack
        gap="40px"
        align="stretch"
        minH="calc(100vh - 100px)"
        p="80px 0px"
      >
        <Text fontSize="32px" fontWeight="bold">
          Создание предмета
        </Text>
        <VStack
          align="stretch"
          gap="20px"
          borderRadius="5px"
          bg={colors.white}
          p="20px"
        >
          <Text fontSize="24px" fontWeight="bold">
            Основная информация
          </Text>
          <Flex gap="10px" p="20px" bg={colors.darkGrey} borderRadius="5px">
            <VStack gap="22px" align="end" p="7px 0" whiteSpace="nowrap">
              <Text>Название:</Text>
              <Text>Преподаватель:</Text>
              <Text>Группа:</Text>
            </VStack>
            <VStack gap="10px" align="start" w="100%">
              <Input
                placeholder="Математика"
                value={data.name}
                onChange={(e) => setData({ ...data, name: e.target.value })}
              />
              <Input
                placeholder="Платонова Тамара Юрьевна"
                value={data.teacherName}
                onChange={(e) =>
                  setData({ ...data, teacherName: e.target.value })
                }
              />
              <Select
                placeholder="2120"
                options={groups || []}
                value={data.group}
                setValue={handleGroupSelect}
              />
            </VStack>
          </Flex>
        </VStack>
        <VStack align="stretch" w="100%">
          <VStack
            align="stretch"
            bg={colors.white}
            p="20px"
            gap="20px"
            borderRadius="5px"
          >
            <Text fontSize="24px" fontWeight="bold">
              Описание
            </Text>
            <Textarea
              value={data.description}
              onChange={(e) =>
                setData({ ...data, description: e.target.value })
              }
              borderColor={colors.darkGrey}
              _focusVisible={{ borderColor: colors.darkGreen }}
            />
          </VStack>
          <Text color={colors.red} fontSize="24px">
            {errorMessage}
          </Text>
        </VStack>
        <Box alignSelf="end" onClick={handleSubmit}>
          <Button>Создать предмет</Button>
        </Box>
      </VStack>
    </BaseLayout>
  );
}
