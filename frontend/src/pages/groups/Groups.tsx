import { Box, Flex, Text } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { colors } from "../../components/ui-kit";
import { GroupService } from "../../services";
import { GroupTable } from "../../components/groups";
import { useEffect, useState } from "react";
import { Group } from "../../types";

const groupService = new GroupService();

export function Groups() {
  const [groups, setGroups] = useState<Group[]>();

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
        <Text color={colors.black} fontSize="32px" as="h1" fontWeight="bold">
          Список групп
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
              <GroupTable data={groups || []} />
            </Box>
          </Flex>
        </Flex>
      </Flex>
    </BaseLayout>
  );
}
