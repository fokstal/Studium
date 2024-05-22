import { Box, Flex, Text, VStack } from "@chakra-ui/react";
import { Chart } from "primereact/chart";
import { useContext, useEffect, useState } from "react";
import { GradeService } from "../../services";
import { Button, Select, colors } from "../ui-kit";
import { LanguageContext, Translator } from "../../store";

const gradeService = new GradeService();

type DiagramsProps = {
  id: number;
};

export function Diagrams({ id }: DiagramsProps) {
  const [chartDataGrades, setChartDataGrades] = useState({});
  const [chartDataCredits, setChartDataCredits] = useState({});
  const { red, green, darkGreen, darkRed, white, darkGrey } = colors;
  const { lang } = useContext(LanguageContext);

  useEffect(() => {
    const data = {
      labels: [
        Translator[lang.name]["marks_lower_3"],
        Translator[lang.name]["marks_highter_3"],
      ],
      datasets: [
        {
          data: [10, 2],
          backgroundColor: [darkRed, darkGreen],
          hoverBackgroundColor: [red, green],
        },
      ],
    };

    setChartDataGrades(data);

    const creditsData = {
      labels: ["Сдано работ", "Не сдано работ"],
      datasets: [
        {
          data: [10, 2],
          backgroundColor: ["#2555FF", "#AC43FF"],
          hoverBackgroundColor: ["#5376f4", "#ba66f9"],
        },
      ],
    };

    setChartDataCredits(creditsData);
  }, []);

  return (
    <VStack p="20px" gap="20px" align="stretch" bg={white} w="100%">
      <Text fontSize="24px" fontWeight="bold">
        {Translator[lang.name]["diagrams"]}
      </Text>
      <Flex gap="20px">
        <VStack align="center" w="100%" bg={darkGrey} p="20px" gap="20px">
          <Text fontSize="24px" fontWeight="bold">
            {Translator[lang.name]["marks"]}
          </Text>
          <Chart
            type="doughnut"
            data={chartDataGrades}
            options={{ cutout: "60%", borderColor: darkGrey }}
          />
        </VStack>

        <VStack align="center" w="100%" bg={darkGrey} p="20px" gap="20px">
          <Text fontSize="24px" fontWeight="bold">
            {Translator[lang.name]["credits"]}
          </Text>
          <Chart
            type="doughnut"
            data={chartDataCredits}
            options={{ cutout: "60%", borderColor: darkGrey }}
          />
        </VStack>
      </Flex>
      <Flex justify="space-between" align="center">
        <Select
          value={null}
          setValue={() => {}}
          placeholder={Translator[lang.name]["select_subject"]}
          options={[]}
        />
        <Button>{Translator[lang.name]["create_diagrams"]}</Button>
      </Flex>
    </VStack>
  );
}
