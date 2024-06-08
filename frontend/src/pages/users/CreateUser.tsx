import { Text, VStack } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { colors } from "../../components/ui-kit";
import { CreateUserForm } from "../../components/users";

export function CreateUser() {
  return (
    <BaseLayout bg={colors.darkGrey}>
      <VStack
        gap="40px"
        align="stretch"
        minH="calc(100vh - 100px)"
        p="80px 0px"
      >
        <Text fontSize="32px" fontWeight="bold">
          Создание пользователя
        </Text>
        <CreateUserForm/>
      </VStack>
    </BaseLayout>
  );
}
