import { Box, Flex, Text, Textarea, VStack } from "@chakra-ui/react";
import { Button, Input, Select, colors } from "../../components/ui-kit";
import { BaseLayout } from "../../layouts";

export function CreateEditSubject() {
  return (
    <BaseLayout bg={colors.darkGrey}>
      <VStack
        gap="40px"
        align="stretch"
        minH="calc(100vh - 100px)"
        p="80px 0px"
      >
        <Text fontSize="32px" fontWeight="bold">
          Создание предмета
        </Text>
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
              <Text>Название:</Text>
              <Text>Преподаватель:</Text>
              <Text>Группа:</Text>
            </VStack>
            <VStack gap="10px" align="start" w="100%">
              <Input placeholder="Математика" />
              <Input placeholder="Платонова Тамара Юрьевна" />
              <Select
                placeholder="2120"
                options={[]}
                value={null}
                setValue={() => {}}
              />
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
          <Button>Создать предмет</Button>
        </Box>
      </VStack>
    </BaseLayout>
  );
}
