import {
  Box,
  InputGroup,
  Text,
  VStack,
  cookieStorageManager,
} from "@chakra-ui/react";
import { Button, Input, PasswordInput, colors } from "../../ui-kit";
import { useContext, useState, useTransition } from "react";
import { AuthServise } from "../../../services/AuthService";
import { useNavigate } from "react-router-dom";
import { LanguageContext, Translator } from "../../../store";

const authService = new AuthServise();

export function AuthForm() {
  const [isPending, startTransaction] = useTransition();
  const [login, setLogin] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const navigator = useNavigate();
  const { lang } = useContext(LanguageContext);

  const authorize = () => {
    startTransaction(() => {
      if (!login || !password) return;
      authService.login(login, password).then((res) => {
        if (res.status === 200) return navigator("/students");
      });
    });
  };

  return (
    <Box bg={colors.white} p="10px 10px 20px 10px" borderRadius="5px">
      <VStack align="stretch" gap="20px">
        <Text as="b" fontSize="24px">
          {Translator[lang.name]["authorize"]}
        </Text>
        <InputGroup flexDirection="column" gap="10px">
          <Input
            type="text"
            placeholder="Login"
            value={login}
            onChange={(e: any) => setLogin(e.target.value)}
          />
          <PasswordInput
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </InputGroup>
        <Button onClick={authorize} disabled={isPending}>
          {Translator[lang.name]["enter"]}
        </Button>
      </VStack>
    </Box>
  );
}
