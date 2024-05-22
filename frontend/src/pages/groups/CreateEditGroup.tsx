import { Box, Flex, Text, Textarea, VStack } from "@chakra-ui/react";
import { Button, Input, Select, colors } from "../../components/ui-kit";
import { BaseLayout } from "../../layouts";
import { UserService } from "../../services";
import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { LanguageContext, Translator } from "../../store";
import { User } from "../../types";
import { createGroup } from "../../lib";

const userService = new UserService();

export function CreateGroup() {
  const [data, setData] = useState<{
    name?: string;
    curator?: User;
    auditoryName?: string;
    description?: string;
  }>({});
  const [curators, setCurators] = useState<User[]>();
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const navigator = useNavigate();
  const { lang } = useContext(LanguageContext);

  const handleSubmit = async () => {
    const res = await createGroup(data);
    if (res === "Created") return navigator("/group");
    setErrorMessage(res);
  };

  const handleCuratorChange = (value: User) => {
    setData({ ...data, curator: value });
  };

  useEffect(() => {
    setErrorMessage(null);
  }, [data]);

  useEffect(() => {
    userService
      .get()
      .then((data) =>
        setCurators(
          data.filter((user: User) => user.roleList[0].name === "Curator")
        )
      );
  }, []);

  return (
    <BaseLayout bg={colors.darkGrey}>
      <VStack
        align="stretch"
        gap="40px"
        minH="calc(100vh - 100px)"
        p="80px 0px"
      >
        <Text fontSize="32px" fontWeight="bold">
          {Translator[lang.name]["group_create"]}
        </Text>
        <VStack align="stretch" gap="20px">
          <VStack
            align="stretch"
            gap="20px"
            borderRadius="5px"
            bg={colors.white}
            p="20px"
            borderColor={errorMessage ? colors.red : "none"}
          >
            <Text fontSize="24px" fontWeight="bold">
              {Translator[lang.name]["main_information"]}
            </Text>
            <Flex gap="10px" p="20px" bg={colors.darkGrey} borderRadius="5px">
              <VStack gap="22px" align="end" p="7px 0" whiteSpace="nowrap">
                <Text>{Translator[lang.name]["group_number"]}:</Text>
                <Text>{Translator[lang.name]["curator"]}:</Text>
                <Text>{Translator[lang.name]["auditory"]}:</Text>
              </VStack>
              <VStack gap="10px" align="start" w="100%">
                <Input
                  placeholder="2120"
                  value={data?.name}
                  onChange={(e) => setData({ ...data, name: e.target.value })}
                />
                <Select
                  placeholder="Платонова Тамара Юрьевна"
                  options={curators || []}
                  value={data?.curator}
                  setValue={handleCuratorChange}
                  name="firstName"
                />
                <Input
                  placeholder="16"
                  value={data?.auditoryName}
                  onChange={(e) =>
                    setData({ ...data, auditoryName: e.target.value })
                  }
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
              borderColor={errorMessage ? colors.red : "none"}
            >
              <Text fontSize="24px" fontWeight="bold">
                {Translator[lang.name]["descripition"]}
              </Text>
              <Textarea
                borderColor={colors.darkGrey}
                _focusVisible={{ borderColor: colors.darkGreen }}
                value={data?.description}
                onChange={(e) =>
                  setData({ ...data, description: e.target.value })
                }
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
      </VStack>
    </BaseLayout>
  );
}
