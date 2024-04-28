import { Box, InputGroup, Text, VStack } from "@chakra-ui/react";
import { Button, Input, colors } from "../../ui-kit";

export function AuthForm() {
  return (
    <Box bg={colors.white} p="10px 10px 20px 10px" borderRadius="5px">
      <VStack align="stretch" gap="20px">
        <Text as="b" fontSize="24px">Авторизация</Text>
        <InputGroup flexDirection="column" gap="10px">
          <Input type="text" placeholder="Login"/>
          <Input type="password" placeholder="Password"/>
        </InputGroup>
        <Button>Войти</Button>
      </VStack>
    </Box>
  )
}