import { Box, Flex, Text } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { colors } from "../../components/ui-kit";
import { GroupService, SubjectService } from "../../services";
import { useEffect, useState } from "react";
import { Subject } from "../../types";
import { SubjectTable } from "../../components/subjects";

const subjectService = new SubjectService();
const groupService = new GroupService();

export function Subjects() {
  const [subjects, setSubjects] = useState<Subject[]>();

  const updateSubjects = async () => {
    const subjects = await subjectService.get();
    const formatedSubjects = await subjects.map(async (s: Subject) => ({
      ...s,
      groupId: (await groupService.getById(+s.groupId)).name,
    }));
    setSubjects(formatedSubjects);
  }

  useEffect(() => {
    updateSubjects();
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
          Список предметов
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
              <SubjectTable data={subjects || []} />
            </Box>
          </Flex>
        </Flex>
      </Flex>
    </BaseLayout>
  );
}
