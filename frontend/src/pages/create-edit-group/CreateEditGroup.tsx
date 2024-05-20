import { Box, Flex, Text, Textarea, VStack } from "@chakra-ui/react";
import { Button, Input, colors } from "../../components/ui-kit";
import { BaseLayout } from "../../layouts";

export function CreateEditGroup() {
  return (
    <BaseLayout bg={colors.darkGrey}>
      <VStack
        align="stretch"
        gap="40px"
        minH="calc(100vh - 100px)"
        p="80px 0px"
      >
        <Text fontSize="32px" fontWeight="bold">
          Создание группы
        </Text>
        <VStack align="stretch" gap="20px">
          <VStack
            align="stretch"
            gap="20px"
            borderRadius="5px"
            bg={colors.white}
            p="20px"
          >
            <Text fontSize="24px" fontWeight="bold">
              Основная информация
            </Text>
            <Flex gap="10px" p="20px" bg={colors.darkGrey} borderRadius="5px">
              <VStack gap="22px" align="end" p="7px 0" whiteSpace="nowrap">
                <Text>Номер группы:</Text>
                <Text>Куратор:</Text>
                <Text>Аудитория:</Text>
              </VStack>
              <VStack gap="10px" align="start" w="100%">
                <Input placeholder="2120 best ever" />
                <Input placeholder="Платонова Тамара Юрьевна" />
                <Input placeholder="16" />
              </VStack>
            </Flex>
          </VStack>
          <VStack
            align="stretch"
            bg={colors.white}
            p="20px"
            gap="20px"
            borderRadius="5px"
          >
            <Text fontSize="24px" fontWeight="bold">
              Описание
            </Text>
            <Textarea
              borderColor={colors.darkGrey}
              _focusVisible={{ borderColor: colors.darkGreen }}
            />
          </VStack>
          <Box alignSelf="end">
            <Button>Создать группу</Button>
          </Box>
        </VStack>
      </VStack>
    </BaseLayout>
  );
}
