import { Flex, Text } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { colors } from "../../components/ui-kit";
import {
  GroupService,
  PersonService,
  StudentService,
  SubjectService,
} from "../../services";
import { useState } from "react";
import { Group, Person, Subject } from "../../types";
import { JournalFilters } from "../../components/journal";

const groupService = new GroupService();
const studentService = new StudentService();
const personService = new PersonService();
const subjectService = new SubjectService();

export function Journal() {
  const [filters, setFilters] = useState<{
    group?: Group;
    person?: Person;
    subject?: Subject;
  }>();

  return (
    <BaseLayout bg={colors.darkGrey}>
      <Flex
        w="100%"
        minH="calc(100vh - 100px)"
        p="80px 0px"
        direction="column"
        gap="10px"
      >
        <Flex gap="20px">
          <Text fontSize="32px" fontWeight="bold" as="h1">
            Журнал
          </Text>
          <JournalFilters filters={filters || {}} setFilters={setFilters}/>
        </Flex>
      </Flex>
    </BaseLayout>
  );
}
