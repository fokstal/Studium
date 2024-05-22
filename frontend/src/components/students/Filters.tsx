import { Box, Flex, Input, InputGroup, Text, VStack } from "@chakra-ui/react";
import { Button, Checkbox, Select, colors, groups } from "../ui-kit";
import { AiOutlineClose } from "react-icons/ai";
import { useContext } from "react";
import { LanguageContext, Translator } from "../../store";

export function Filters() {
  const { lang } = useContext(LanguageContext);
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
          <Text fontWeight="bold">
            {Translator[lang.name]["student_group"]}
          </Text>
          {/* <Select placeholder="Выберите группу" options={groups} /> */}
        </VStack>
        <VStack align="stretch">
          <Text fontWeight="bold">
            {Translator[lang.name]["date_start_stude"]}
          </Text>
          <InputGroup maxW="200px">
            <Input
              borderColor={colors.lightGrey}
              _hover={{ borderColor: colors.green }}
              _focusVisible={{ boxShadow: "none" }}
              bg={colors.white}
              borderRadius="5px 0 0 5px"
              placeholder={Translator[lang.name]["from"]}
            />
            <Input
              borderColor={colors.lightGrey}
              _hover={{ borderColor: colors.green }}
              _focusVisible={{ boxShadow: "none" }}
              bg={colors.white}
              borderRadius="0 5px 5px 0"
              placeholder={Translator[lang.name]["to"]}
            />
          </InputGroup>
        </VStack>
        <VStack align="stretch">
          <Flex gap="10px" align="center">
            <Checkbox name="graduated" id="graduated" />
            <Text as="label" htmlFor="graduated">
              {Translator[lang.name]["this_year_finish"]}
            </Text>
          </Flex>
          <Flex gap="10px" align="center">
            <Checkbox name="enrollment" id="enrollment" />
            <Text as="label" htmlFor="enrollment">
              {Translator[lang.name]["this_year_start"]}
            </Text>
          </Flex>
        </VStack>
        <VStack align="stretch">
          <Button>{Translator[lang.name]["search"]}</Button>
          <Flex gap="10px" align="center" cursor="pointer">
            <Text fontWeight="bold">
              {Translator[lang.name]["clear_filters"]}
            </Text>
            <AiOutlineClose size="20px" />
          </Flex>
        </VStack>
      </Flex>
    </Box>
  );
}
