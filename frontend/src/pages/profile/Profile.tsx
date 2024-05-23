import { Flex, Text, VStack } from "@chakra-ui/react";
import { colors } from "../../components/ui-kit";
import { BaseLayout } from "../../layouts";
import {
  AiOutlineExclamationCircle,
  AiOutlineExport,
  AiOutlinePieChart,
} from "react-icons/ai";
import { useContext, useState } from "react";
import { StudentComponent } from "../../components/students";
import { AverageGrade, Diagrams } from "../../components/profile";
import { LanguageContext, Translator } from "../../store";

export function Profile() {
  const [currentPart, setCurrentPart] = useState("Информация");
  const { lang } = useContext(LanguageContext);

  const profileLinks = [
    {
      name: Translator[lang.name]["information"],
      Icon: AiOutlineExclamationCircle,
    },
    { name: Translator[lang.name]["performance"], Icon: AiOutlinePieChart },
  ];

  return (
    <BaseLayout bg={colors.darkGrey}>
      <VStack p="80px 0" minH="calc(100vh - 100px)" align="stretch" gap="50px">
        <Text fontSize="32px" fontWeight="bold">
        {Translator[lang.name]["profile"]}
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
              <Text whiteSpace="pre">{Translator[lang.name]["exit"]}</Text>
            </Flex>
          </VStack>
          <VStack align="stretch" gap="20px" w="100%">
            {currentPart === Translator[lang.name]["information"] ? (
              <StudentComponent id="292bea68-3b33-40af-8833-4b29c9d536a9" />
            ) : null}
            {currentPart === Translator[lang.name]["performance"] ? (
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
