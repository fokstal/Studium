import { Box, InputGroup, Text, VStack } from "@chakra-ui/react";
import { Button, Input, colors } from "../../ui-kit";
import { useState, useTransition } from "react";
import { AuthServise } from "../../../services/AuthService";
import { useNavigate } from "react-router-dom";

const authService = new AuthServise();

export function AuthForm() {
  const [isPending, startTransaction] = useTransition();
  const [login, setLogin] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const navigator = useNavigate();

  const authorize = () => {
    startTransaction(() => {
      if (!login || !password) return;
      authService.login(login, password).then((status) => {
        if (status === 200) navigator("/students");
      });
    });
  };

  return (
    <Box bg={colors.white} p="10px 10px 20px 10px" borderRadius="5px">
      <VStack align="stretch" gap="20px">
        <Text as="b" fontSize="24px">
          Авторизация
        </Text>
        <InputGroup flexDirection="column" gap="10px">
          <Input
            type="text"
            placeholder="Login"
            value={login}
            onChange={(e) => setLogin(e.target.value)}
          />
          <Input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </InputGroup>
        <Button onClick={authorize} disabled={isPending}>
          Войти
        </Button>
      </VStack>
    </Box>
  );
}
