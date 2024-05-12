import { Box, Flex, Input, InputGroup, Text, VStack } from "@chakra-ui/react";
import { Button, Checkbox, Select, colors, groups } from "../ui-kit";
import { AiOutlineClose } from "react-icons/ai";

export function Filters() {
  return (
    <Box
      bg={colors.darkGrey}
      borderRadius="5px"
      h="min-content"
      p="20px"
      w="max-content"
    >
      <Flex direction="column" gap="20px" w="max-content">
        <VStack align="stretch">
          <Text fontWeight="bold">Учебная группа</Text>
          {/* <Select placeholder="Выберите группу" options={groups} /> */}
        </VStack>
        <VStack align="stretch">
          <Text fontWeight="bold">Дата зачисления</Text>
          <InputGroup maxW="200px">
            <Input
              borderColor={colors.lightGrey}
              _hover={{ borderColor: colors.green }}
              _focusVisible={{ boxShadow: "none" }}
              bg={colors.white}
              borderRadius="5px 0 0 5px"
              placeholder="От"
            />
            <Input
              borderColor={colors.lightGrey}
              _hover={{ borderColor: colors.green }}
              _focusVisible={{ boxShadow: "none" }}
              bg={colors.white}
              borderRadius="0 5px 5px 0"
              placeholder="До"
            />
          </InputGroup>
        </VStack>
        <VStack align="stretch">
          <Flex gap="10px" align="center">
            <Checkbox name="graduated" id="graduated" />
            <Text as="label" htmlFor="graduated">
              Выпуск этого года
            </Text>
          </Flex>
          <Flex gap="10px" align="center">
            <Checkbox name="enrollment" id="enrollment" />
            <Text as="label" htmlFor="enrollment">
              Зачисленные в этом году
            </Text>
          </Flex>
        </VStack>
        <VStack align="stretch">
          <Button>Поиск</Button>
          <Flex gap="10px" align="center" cursor="pointer">
            <Text fontWeight="bold">Очистить фильтры</Text>
            <AiOutlineClose size="20px" />
          </Flex>
        </VStack>
      </Flex>
    </Box>
  );
}
