import { Flex, Text, VStack } from "@chakra-ui/react";
import { colors } from "../../components/ui-kit";
import { BaseLayout } from "../../layouts";
import {
  AiOutlineExclamationCircle,
  AiOutlineExport,
  AiOutlinePieChart,
  AiOutlineProfile,
} from "react-icons/ai";
import { useState } from "react";
import { StudentComponent } from "../../components/students";
import { AverageGrade, Diagrams } from "../../components/profile";

export function Profile() {
  const [currentPart, setCurrentPart] = useState("Информация");

  const profileLinks = [
    { name: "Информация", Icon: AiOutlineExclamationCircle },
    { name: "Успеваемость", Icon: AiOutlinePieChart },
    { name: "Оформить справку", Icon: AiOutlineProfile },
  ];

  return (
    <BaseLayout bg={colors.darkGrey}>
      <VStack
        p="80px 0"
        minH="calc(100vh - 100px)"
        align="stretch"
        gap="50px"
      >
        <Text fontSize="32px" fontWeight="bold">
          Личный кабинет
        </Text>
        <Flex gap="90px">
          <VStack fontSize="20px" align="stretch" w="max-content">
            {profileLinks.map(({ name, Icon }) => (
              <Flex
                cursor="pointer"
                gap="10px"
                align="center"
                color={currentPart === name ? colors.green : colors.black}
                onClick={() => setCurrentPart(name)}
              >
                <Icon size="24px" />
                <Text whiteSpace="pre">{name}</Text>
              </Flex>
            ))}
            <Flex gap="10px" color={colors.red} align="center">
              <AiOutlineExport size="24px" />
              <Text whiteSpace="pre">Выйти</Text>
            </Flex>
          </VStack>
          <VStack align="stretch" gap="20px" w="100%">
            {currentPart === "Информация" ? <StudentComponent id={1} /> : null}
            {currentPart === "Успеваемость" ? (
              <>
                <Diagrams id={1} />
                <AverageGrade id={1} />
              </>
            ) : null}
          </VStack>
        </Flex>
      </VStack>
    </BaseLayout>
  );
}
