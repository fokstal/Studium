import { Box, Flex, Link, Text } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { colors } from "../../components/ui-kit";
import { GroupService } from "../../services";
import { GroupTable } from "../../components/groups";
import { useContext, useEffect, useState } from "react";
import { Group } from "../../types";
import { LanguageContext, Translator } from "../../store";
import { FaPlus } from "react-icons/fa";
import { useRoles } from "../../hooks";

const groupService = new GroupService();

export function Groups() {
  const [groups, setGroups] = useState<Group[]>();
  const { lang } = useContext(LanguageContext);
  const roles = useRoles();

  useEffect(() => {
    groupService.get().then((groups) => setGroups(groups));
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
        <Flex align="center" justify="space-between">
          <Text color={colors.black} fontSize="32px" as="h1" fontWeight="bold">
            {Translator[lang.name]["group_list"]}
          </Text>
          {roles.includes("Admin") || roles.includes("Secretar") ? (
            <Box p="5px" bg={colors.green} borderRadius="5px" cursor="pointer">
              <Link href="/group/new">
                <FaPlus color={colors.white} />
              </Link>
            </Box>
          ) : null}
        </Flex>
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
              <GroupTable data={groups || []} />
            </Box>
          </Flex>
        </Flex>
      </Flex>
    </BaseLayout>
  );
}
