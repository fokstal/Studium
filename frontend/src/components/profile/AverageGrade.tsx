import { Box, Flex, Text, VStack } from "@chakra-ui/react";
import { Button, Select, colors } from "../ui-kit";
import { AiOutlineFrown, AiOutlineMeh, AiOutlineSmile } from "react-icons/ai";
import { useContext, useEffect, useState } from "react";
import { LanguageContext, Translator } from "../../store";
import { GradeService } from "../../services";

type AverageGradeProps = {
  id: string;
};

const gradeService = new GradeService();

export function AverageGrade({ id }: AverageGradeProps) {
  const { green, red, white, grey } = colors;
  const { lang } = useContext(LanguageContext);
  const [averageGrade, setAverageGrade] = useState<number>(0);

  useEffect(() => {
    gradeService.averageGrade(id).then((grade) => setAverageGrade(grade));
  }, []);

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
          {Translator[lang.name]["average_mark_calculation"]}
        </Text>
        <Flex gap="20px" align="center">
          <Text>{Translator[lang.name]["calculate_average_mark_for"]}</Text>
          <Select
            value={null}
            placeholder={Translator[lang.name]["select_subject"]}
            setValue={() => {}}
            options={[]}
          />
        </Flex>
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
            ? Translator[lang.name]["average_mark_good"]
            : averageGrade > 5
              ? Translator[lang.name]["average_mark_normal"]
              : Translator[lang.name]["average_mark_bad"]}
        </Text>
      </VStack>
    </Flex>
  );
}
