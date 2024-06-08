import { Box, Flex, Link, Text } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { colors } from "../../components/ui-kit";
import { useContext, useEffect, useState } from "react";
import { User } from "../../types";
import { UserService } from "../../services";
import { UserTable } from "../../components/users";
import { LanguageContext, Translator } from "../../store";

const userService = new UserService();

export function Users() {
  const [users, setUsers] = useState<User[]>();
  const { lang } = useContext(LanguageContext);

  useEffect(() => {
    userService.get().then((users) => setUsers(users));
  }, []);

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
          {Translator[lang.name]["user_list"]}
        </Text>
        <Text color={colors.grey} fontSize="20px" as="b">
          <Link href="/users/new">
            {Translator[lang.name]["register_user"]}
          </Link>
        </Text>
        <Flex
          borderRadius="5px"
          bg={colors.white}
          w="100%"
          p="20px"
          direction="column"
          gap="20px"
        >
          <Flex gap="20px" align="start">
            <Box w="100%">
              <UserTable data={users || []} />
            </Box>
          </Flex>
        </Flex>
      </Flex>
    </BaseLayout>
  );
}
