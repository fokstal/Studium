import { Box, Flex, Text } from "@chakra-ui/react";
import { Input, colors } from "../ui-kit";
import { useContext, useEffect, useState } from "react";
import { LanguageContext, Translator } from "../../store";
import { TableWrapper } from "../data-tables";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { GradeService } from "../../services";

type SemestrGradesType = {
  id: string;
};

const gradeService = new GradeService();

export function SemestrGrades({ id }: SemestrGradesType) {
  const [startDate, setStartDate] = useState<string>();
  const [endDate, setEndDate] = useState<string>();
  const [data, setData] = useState<
    { subjectName: string; averageGrade: number }[]
  >([]);
  const { lang } = useContext(LanguageContext);

  useEffect(() => {
    gradeService
      .getAverageForSemestr({
        studentEntityId: id,
        startDate: startDate ?? "2023-09-01",
        endDate: endDate ?? "2024-06-30",
      })
      .then((data) => setData(data));
  }, [startDate, endDate]);

  return (
    <Flex
      bg={colors.white}
      p="20px"
      borderRadius="5px"
      direction="column"
      gap="20px"
    >
      <Text fontSize="24px" fontWeight="bold">
        {Translator[lang.name]["semester_grades"]}
      </Text>
      <Text fontSize="18px" fontWeight="bold" color={colors.lightGrey}>
        {Translator[lang.name]["fill_semester_date_in_input"]}
      </Text>
      <Flex gap="10px">
        <Box w="100px">
          <Input
            type="date"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
          />
        </Box>
        <Box w="100px">
          <Input
            type="date"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
          />
        </Box>
      </Flex>
      <TableWrapper>
        <DataTable value={data} paginator={data.length > 7} rows={7}>
          <Column
            field="subjectName"
            header={Translator[lang.name]["subject"]}
          ></Column>
          <Column
            field="averageGrade"
            header={Translator[lang.name]["average_grade"]}
          ></Column>
        </DataTable>
      </TableWrapper>
    </Flex>
  );
}
