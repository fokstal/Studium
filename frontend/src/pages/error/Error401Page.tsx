import { Box, Flex, Image, Text, VStack } from "@chakra-ui/react";
import { Button, colors } from "../../components/ui-kit";
import { useNavigate } from "react-router-dom";
import { useContext } from "react";
import { LanguageContext, Translator } from "../../store";

const imagesUrl = process.env.PUBLIC_URL + "/images";

export function Error401Page() {
  const navigator = useNavigate();
  const { lang } = useContext(LanguageContext);
  document.body.style.overflow = "hidden";

  return (
    <Box w="100vw" h="100vh" bg={colors.white}>
      <Box m="auto" paddingTop="180px" w="max-content">
        <Flex
          gap="50px"
          direction={{ base: "column", md: "row" }}
          align="center"
        >
          <Flex gap="30px" direction="column">
            <Image src={`${imagesUrl}/logo.svg`} w="160px" h="30px" />
            <VStack maxW="300px" align="stretch">
              <Text as="h1" fontWeight="bold" fontSize="24px">
                {Translator[lang.name]["error_401"]}
              </Text>
              <Text>{Translator[lang.name]["user_not_auth"]}</Text>
            </VStack>
            <Button onClick={() => navigator("/")}>
              {Translator[lang.name]["back_to_main"]}
            </Button>
          </Flex>
          <Box>
            <Image
              display="block"
              src={`${imagesUrl}/error-robot.svg`}
              w="170px"
              h="215px"
            />
          </Box>
        </Flex>
      </Box>
    </Box>
  );
}
