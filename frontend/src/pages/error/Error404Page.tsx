import { Box, Flex, Image, Text, VStack } from "@chakra-ui/react";
import { Button, colors } from "../../components/ui-kit";
import { useNavigate } from "react-router-dom";

const imagesUrl = process.env.PUBLIC_URL + "/images";

export function Error404Page() {
  const navigator = useNavigate();

  return (
    <Box w="100vw" h="100vh" bg={colors.white}>
      <Box m="auto" paddingTop="180px" w="max-content">
        <Flex gap="50px" direction={{base: "column", md: "row"}} align="center">
          <Flex gap="30px" direction="column">
            <Image src={`${imagesUrl}/logo.svg`} w="160px" h="30px"/>
            <VStack maxW="300px" align="stretch">
              <Text as="h1" fontWeight="bold" fontSize="24px">Ошибка 404</Text>
              <Text>
                Страница, которую вы ищете, не найдена или не существует.
              </Text>
            </VStack>
            <Button onClick={() => navigator("/")}>Вернуться на главную</Button>
          </Flex>
          <Box>
            <Image display="block" src={`${imagesUrl}/error-robot.svg`} w="170px" h="215px" />
          </Box>
        </Flex>
      </Box>
    </Box>
  )
}