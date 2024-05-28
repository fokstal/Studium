import { Flex, Text } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { colors } from "../../components/ui-kit";
import { useContext, useEffect, useState } from "react";
import { Group, Person, Subject } from "../../types";
import { JournalFilters, JournalTable } from "../../components/journal";
import { LanguageContext, Translator } from "../../store";
import { formatGrades } from "../../lib";

export function Journal() {
  const [filters, setFilters] = useState<{
    group?: Group;
    person?: Person;
    subject?: Subject;
  }>();
  const [data, setData] = useState<any[]>();
  const [columns, setColumns] = useState<{ field: string; header: string }[]>(
    []
  );
  const { lang } = useContext(LanguageContext);

  useEffect(() => {
    const fetchData = async () => {
      const {columns, tableData} = await formatGrades(filters || {});
      setColumns(columns);
      setData(tableData)
    };
    fetchData();
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
        <JournalTable data={data || []} columns={columns || []} />
      </Flex>
    </BaseLayout>
  );
}
