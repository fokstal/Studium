import { Box, Flex, Text, VStack } from "@chakra-ui/react";
import { LanguageContext, Translator } from "../../store";
import { useContext, useState } from "react";
import { Button, Select, Input, colors } from "../ui-kit";
import { AuthServise } from "../../services";
import { useNavigate } from "react-router-dom";

const authService = new AuthServise();

export function CreateUserForm() {
  const [data, setData] = useState<{
    login?: string;
    firstName?: string;
    middleName?: string;
    lastName?: string;
    password?: string;
    roleEnumList?: typeof roles;
  }>({});
  const { lang } = useContext(LanguageContext);
  const navigator = useNavigate();

  const roles = [
    { value: 2, name: Translator[lang.name]["secretar"] },
    { value: 3, name: Translator[lang.name]["curator"] },
    { value: 4, name: Translator[lang.name]["teacher"] },
    { value: 5, name: Translator[lang.name]["student"] },
  ];

  const handleSubmit = async () => {
    const res = await authService.register(data);

    if (res.status === 201) navigator("/users");
  };

  const handleRoleChange = (value: { value: number; name: string }) => {
    if (!data.roleEnumList) {
      setData({ ...data, roleEnumList: [value] });
    } else {
      setData({ ...data, roleEnumList: [...data?.roleEnumList, value] });
    }
  };

  return (
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
      <Text fontSize="20px" fontWeight="bold" color={colors.grey}>
        {Translator[lang.name]["selecting_role_help"]}
      </Text>
      <Flex gap="10px" p="20px" bg={colors.darkGrey} borderRadius="5px">
        <VStack gap="22px" align="end" p="7px 0" whiteSpace="nowrap">
          <Text>{Translator[lang.name]["login"]}:</Text>
          <Text>{Translator[lang.name]["first_name"]}:</Text>
          <Text>{Translator[lang.name]["middle_name"]}:</Text>
          <Text>{Translator[lang.name]["last_name"]}:</Text>
          <Text>{Translator[lang.name]["password"]}:</Text>
          <Text>{Translator[lang.name]["roles"]}:</Text>
        </VStack>
        <VStack gap="10px" align="start" w="100%">
          <Input
            placeholder="Fokstal"
            value={data?.login}
            onChange={(e) => setData({ ...data, login: e.target.value })}
          />
          <Input
            placeholder="Alexey"
            value={data?.firstName}
            onChange={(e) => setData({ ...data, firstName: e.target.value })}
          />
          <Input
            placeholder="Igorevich"
            value={data?.middleName}
            onChange={(e) => setData({ ...data, middleName: e.target.value })}
          />
          <Input
            placeholder="Karneichick"
            value={data?.lastName}
            onChange={(e) => setData({ ...data, lastName: e.target.value })}
          />
          <Input
            placeholder="password"
            value={data?.password}
            onChange={(e) => setData({ ...data, password: e.target.value })}
          />
          <Select
            options={roles}
            value={data?.roleEnumList}
            placeholder={Translator[lang.name]["select_role"]}
            setValue={(e: any) => handleRoleChange(e)}
          />
        </VStack>
        <Box alignSelf="end" onClick={handleSubmit}>
          <Button>{Translator[lang.name]["create"]}</Button>
        </Box>
      </Flex>
    </VStack>
  );
}
