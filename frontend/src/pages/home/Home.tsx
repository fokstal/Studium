import { Box, Flex, Image, Text, VStack } from "@chakra-ui/react";
import { colors } from "../../components/ui-kit";
import { AuthForm } from "../../components/forms";

const imagesUrl = process.env.PUBLIC_URL + "/images";

export function Home() {
  return (
    <Flex 
    w="100%" 
    h="100vh" 
    background={`url(${imagesUrl}/home-bg.png)`}
    backgroundSize="cover"
    align="center"
    justify="center"
    direction={{base: "column", md: "row"}}
    paddingInline={{base : "20px", md: "60px", xl: "120px"}}
    gap={{base : "60px", xl: "120px"}}>
      <VStack align={{base: "center", md: "start"}} gap="20px">
        <Image src={`${imagesUrl}/logo-white.svg`} alt="Studium" width="242px" height="46px"/>
        <Text lineHeight="auto" fontSize={{base: "xl", md: "2xl", lg: "4xl"}} color={colors.white} fontWeight="bold" maxW="560px">
          удобный электронный журнал и база данных учащихся
        </Text>
      </VStack>
      <Box w="320px">
        <AuthForm/>
      </Box>
    </Flex>
  )
}