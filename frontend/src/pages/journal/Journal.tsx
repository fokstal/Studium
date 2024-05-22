import { Flex, Text } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { colors } from "../../components/ui-kit";
import { useContext, useEffect, useState } from "react";
import { Grade, Group, Person, Subject } from "../../types";
import { JournalFilters, JournalTable } from "../../components/journal";
import { GradeService } from "../../services";
import { formatGrades } from "../../lib";
import { LanguageContext, Translator } from "../../store";

const gradeService = new GradeService();

export function Journal() {
  const [filters, setFilters] = useState<{
    group?: Group;
    person?: Person;
    subject?: Subject;
  }>();
  const [grades, setGrades] = useState<Grade[]>();
  const [data, setData] = useState<any[]>();
  const { lang } = useContext(LanguageContext);

  useEffect(() => {
    gradeService.get().then((grades) => setGrades(grades));
    formatGrades(filters!, grades).then((data) => setData(data));
  }, [filters]);

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
            {Translator[lang.name]["journal"]}
          </Text>
          <JournalFilters filters={filters || {}} setFilters={setFilters} />
        </Flex>
        <JournalTable
          data={data || []}
          columns={[
            { name: "Name", field: "name" },
            { field: "9.4_0", name: "9.4" },
            { field: "9.4_1", name: "9.4" },
            { field: "9.4_2", name: "9.4" },
            { field: "9.4_3", name: "9.4" },
          ]}
        />
      </Flex>
    </BaseLayout>
  );
}
