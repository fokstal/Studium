import { Box, Flex, Text, VStack } from "@chakra-ui/react";
import { Chart } from "primereact/chart";
import { useEffect, useState } from "react";
import { GradeService } from "../../services";
import { Button, Select, colors } from "../ui-kit";

const gradeService = new GradeService();

type DiagramsProps = {
  id: number;
};

export function Diagrams({ id }: DiagramsProps) {
  const [chartDataGrades, setChartDataGrades] = useState({});
  const [chartDataCredits, setChartDataCredits] = useState({});
  const { red, green, darkGreen, darkRed, white, darkGrey } = colors;

  useEffect(() => {
    const data = {
      labels: ["Отметки ниже 3", "Отметки выше 3"],
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
        Диаграммы
      </Text>
      <Flex gap="20px">
        <VStack align="center" w="100%" bg={darkGrey} p="20px" gap="20px">
          <Text fontSize="24px" fontWeight="bold">
            Отметки
          </Text>
          <Chart type="doughnut" data={chartDataGrades} options={{ cutout: "60%", borderColor: darkGrey }} />
        </VStack>

        <VStack align="center" w="100%" bg={darkGrey} p="20px" gap="20px">
          <Text fontSize="24px" fontWeight="bold">
            Зачеты
          </Text>
          <Chart type="doughnut" data={chartDataCredits} options={{ cutout: "60%", borderColor: darkGrey }} />
        </VStack>
      </Flex>
      <Flex justify="space-between" align="center">  
        <Select value={null} setValue={() =>{}} placeholder="Выбурите предмет" options={[]}/>
        <Button>Сформировать диаграммы</Button>
      </Flex>
    </VStack>
  );
}
