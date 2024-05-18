import { Box, Flex, Text, VStack } from "@chakra-ui/react";
import { Button, Select, colors } from "../ui-kit";
import { AiOutlineFrown, AiOutlineMeh, AiOutlineSmile } from "react-icons/ai";

type AverageGradeProps = {
  id: number;
};

export function AverageGrade({ id }: AverageGradeProps) {
  const averageGrade = 4.3;
  const { green, red, white, grey } = colors;
  return (
    <Flex
      align="center"
      justify="space-between"
      bg={white}
      p="20px"
      borderRadius="5px"
    >
      <VStack gap="20px" align="stretch">
        <Text fontSize="24px" fontWeight="bold">
          Расчёт среднего балла
        </Text>
        <Flex gap="20px" align="center">
          <Text>Расчитать средний балл для:</Text>
          <Select
            value={null}
            placeholder="Выберите предмет"
            setValue={() => {}}
            options={[]}
          />
        </Flex>
        <Box>
          <Button>Расчитать</Button>
        </Box>
      </VStack>
      <VStack gap="5px" align="end">
        <Flex
          gap="10px"
          fontSize="40px"
          align="center"
          color={averageGrade >= 9 ? green : averageGrade > 5 ? "yellow" : red}
        >
          <Text>{averageGrade}</Text>
          <Text>
            {averageGrade >= 9 ? (
              <AiOutlineSmile />
            ) : averageGrade > 5 ? (
              <AiOutlineMeh />
            ) : (
              <AiOutlineFrown />
            )}
          </Text>
        </Flex>
        <Text color={grey}>
          {averageGrade >= 9
            ? "Вы - отличник! Так держать!"
            : averageGrade > 5
              ? "Вы - хорошист! Отлично!"
              : "Вы - серьезный тип! Старайтесь лучше!"}
        </Text>
      </VStack>
    </Flex>
  );
}
