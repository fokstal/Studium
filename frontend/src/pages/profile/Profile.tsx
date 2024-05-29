import { Flex, Link, Text, VStack } from "@chakra-ui/react";
import { colors } from "../../components/ui-kit";
import { BaseLayout } from "../../layouts";
import {
  AiOutlineExclamationCircle,
  AiOutlineExport,
  AiOutlinePieChart,
} from "react-icons/ai";
import { useContext, useEffect, useState } from "react";
import { StudentComponent } from "../../components/students";
import { AverageGrade, Diagrams } from "../../components/profile";
import { LanguageContext, Translator } from "../../store";
import { AuthServise } from "../../services";

const authService = new AuthServise();

export function Profile() {
  const [id, setId] = useState<string>();
  const { lang } = useContext(LanguageContext);
  const [currentPart, setCurrentPart] = useState(
    Translator[lang.name]["information"]
  );

  const profileLinks = [
    {
      name: Translator[lang.name]["information"],
      Icon: AiOutlineExclamationCircle,
    },
    { name: Translator[lang.name]["performance"], Icon: AiOutlinePieChart },
  ];

  useEffect(() => {
    authService.session().then((user) => setId(user.id));
  }, []);

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
              <Link _hover={{textDecoration: "none"}} href="/">
                <Text whiteSpace="pre">{Translator[lang.name]["exit"]}</Text>
              </Link>
            </Flex>
          </VStack>
          <VStack align="stretch" gap="20px" w="100%">
            {currentPart === Translator[lang.name]["information"] ? (
              <StudentComponent id={id ?? ""} />
            ) : null}
            {currentPart === Translator[lang.name]["performance"] ? (
              <>
                <Diagrams id={id || ""} />
                <AverageGrade id={id || ""} />
              </>
            ) : null}
          </VStack>
        </Flex>
      </VStack>
    </BaseLayout>
  );
}
