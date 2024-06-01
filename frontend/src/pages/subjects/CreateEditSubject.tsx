import { Box, Flex, Text, Textarea, VStack } from "@chakra-ui/react";
import { Button, Input, Select, colors } from "../../components/ui-kit";
import { BaseLayout } from "../../layouts";
import { GroupService, SubjectService, UserService } from "../../services";
import { useContext, useEffect, useState } from "react";
import { Group, User } from "../../types";
import { useNavigate, useParams } from "react-router-dom";
import { LanguageContext, Translator } from "../../store";
import { createSubject, editSubject } from "../../lib";

const groupService = new GroupService();
const userService = new UserService();
const subjectService = new SubjectService();

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
  const { id } = useParams();

  const updateData = async () => {
    const groups = await groupService.get();
    setGroups(groups);
    const users = await userService.get();

    setTeachers(
      users.filter((user: User) =>
        user.roleList.find((role) => role.name === "Teacher")
      )
    );
    if (id) {
      const data = await subjectService.getById(id);
      setData({
        ...data,
        group: groups.find((g: Group) => g.id === data.groupId),
        teacher: users.find((u: User) => u.id === data.teacherId),
      });
    }
  };

  useEffect(() => {
    updateData();
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
    if (id) {
      const res = await editSubject(+id, data);
      if (res === "Created") return navigator("/subject");
      setErrorMessage(Translator[lang.name][res]);
    } else {
      const res = await createSubject(data);
      if (res === "Created") return navigator("/subject");
      setErrorMessage(Translator[lang.name][res]);
    }
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
          {!id
            ? Translator[lang.name]["subject_create"]
            : Translator[lang.name]["subject_edit"]}
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
                name="lastName"
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
          <Button>
            {id
              ? Translator[lang.name]["edit"]
              : Translator[lang.name]["create"]}
          </Button>
        </Box>
      </VStack>
    </BaseLayout>
  );
}
