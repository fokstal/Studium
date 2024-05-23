import { Box, Flex, Text, Textarea, VStack } from "@chakra-ui/react";
import { Button, Input, Select, colors } from "../../components/ui-kit";
import { BaseLayout } from "../../layouts";
import { GroupService, UserService } from "../../services";
import { useContext, useEffect, useState } from "react";
import { Group, User } from "../../types";
import { useNavigate } from "react-router-dom";
import { LanguageContext, Translator } from "../../store";
import { createSubject } from "../../lib";

const groupService = new GroupService();
const userService = new UserService();

export function CreateEditSubject() {
  const [errorMessage, setErrorMessage] = useState<string | null>();
  const [groups, setGroups] = useState<Group[]>();
  const [teachers, setTeachers] = useState<User[]>();
  const [data, setData] = useState<{
    name?: string;
    description?: string;
    teacher?: User;
    group?: Group;
  }>({});
  const navigator = useNavigate();
  const { lang } = useContext(LanguageContext);

  useEffect(() => {
    groupService.get().then((value) => setGroups(value));
    userService
      .get()
      .then((data) =>
        setTeachers(
          data.filter((user: User) => user.roleList[0].name === "Teacher")
        )
      );
  }, []);

  useEffect(() => {
    setErrorMessage(null);
  }, [data]);

  const handleTeacherSelect = (value: User) => {
    setData({ ...data, teacher: value });
  };

  const handleGroupSelect = (value: Group) => {
    setData({ ...data, group: value });
  };

  const handleSubmit = async () => {
    const res = await createSubject(data);
    if (res === "Created") return navigator("/subject");
    setErrorMessage(res);
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
          {Translator[lang.name]["subject_create"]}
        </Text>
        <VStack
          align="stretch"
          gap="20px"
          borderRadius="5px"
          bg={colors.white}
          p="20px"
        >
          <Text fontSize="24px" fontWeight="bold">
            {Translator[lang.name]["main_information"]}
          </Text>
          <Flex gap="10px" p="20px" bg={colors.darkGrey} borderRadius="5px">
            <VStack gap="22px" align="end" p="7px 0" whiteSpace="nowrap">
              <Text>{Translator[lang.name]["name"]}:</Text>
              <Text>{Translator[lang.name]["teacher"]}:</Text>
              <Text>{Translator[lang.name]["group"]}:</Text>
            </VStack>
            <VStack gap="10px" align="start" w="100%">
              <Input
                placeholder="Математика"
                value={data.name}
                onChange={(e) => setData({ ...data, name: e.target.value })}
              />
              <Select
                placeholder="Платонова Тамара Юрьевна"
                value={data.teacher}
                options={teachers || []}
                setValue={handleTeacherSelect}
                name="firstName"
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
              {Translator[lang.name]["descripition"]}
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
          <Button>{Translator[lang.name]["create"]}</Button>
        </Box>
      </VStack>
    </BaseLayout>
  );
}
