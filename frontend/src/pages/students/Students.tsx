import { Box, Flex, InputGroup, InputLeftElement, Text, Input as ChakraInput, VStack } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { Button, Checkbox, Input, Select, colors, groups } from "../../components/ui-kit";
import { useState } from "react";
import { AiOutlineClose, AiOutlineFilter } from "react-icons/ai";
import { FaSearch } from "react-icons/fa";
import { StudentsTable } from "../../components/data-tables";

export function Students() {
  const [isOpenToggleFilters, setIsOpenToggleFilters] = useState<boolean>(false);
  const [search, setSearch] = useState<string>();

  const handleOpenToggleColumns = () => {
    setIsOpenToggleFilters(!isOpenToggleFilters);
  };

  return (
    <BaseLayout bg={colors.darkGrey}>
      <Flex w="100%" minH="calc(100vh - 100px)" p="80px 0px" direction="column" gap="10px">
        <Text color={colors.black} fontSize="32px" as="h1" fontWeight="bold">Список учащихся</Text>
        <Text color={colors.grey} fontSize="20px" as="b">
          Нажмите на учащегося в таблице для подробной информации
        </Text>
        <Flex borderRadius="5px" bg={colors.white} w="100%" p="20px" direction="column" gap="20px">
          <Flex gap="10px">
            <Flex gap="5px" align="center" cursor="pointer" onClick={handleOpenToggleColumns}>
              <Text userSelect="none" fontSize="16px">Фильтры</Text>
              <AiOutlineFilter size="16px" />
            </Flex>
            <InputGroup alignItems="center">
              <InputLeftElement>
                <FaSearch/>
              </InputLeftElement>
              <ChakraInput 
              placeholder="Поиск" 
              borderColor={colors.lightGrey}
              _hover={{borderColor: colors.green}} 
              value={search} 
              onChange={(e) => setSearch(e.target.value)}
              _focusVisible={{boxShadow: "none"}}/>
            </InputGroup>
          </Flex>
          <Flex gap="20px">
            {isOpenToggleFilters ? (
              <Box bg={colors.darkGrey} borderRadius="5px" h="min-content" p="20px" w="max-content">
                <Flex direction="column" gap="20px" w="max-content">
                  <VStack align="stretch">
                    <Text fontWeight="bold">Учебная группа</Text>
                    <Select placeholder="Выберите группу" options={groups}/>
                  </VStack>
                  <VStack align="stretch">
                    <Text fontWeight="bold">Дата зачисления</Text>
                    <InputGroup maxW="200px">
                      <ChakraInput               
                        borderColor={colors.lightGrey}
                        _hover={{borderColor: colors.green}} 
                        _focusVisible={{boxShadow: "none"}}
                        bg={colors.white}
                        borderRadius="5px 0 0 5px"
                        placeholder="От"/>
                      <ChakraInput 
                        borderColor={colors.lightGrey}
                        _hover={{borderColor: colors.green}} 
                        _focusVisible={{boxShadow: "none"}}
                        bg={colors.white}
                        borderRadius="0 5px 5px 0"
                        placeholder="До"/>
                    </InputGroup>
                  </VStack>
                  <VStack align="stretch">
                    <Flex gap="10px" align="center">
                      <Checkbox name="graduated" id="graduated"/>
                      <Text as="label" htmlFor="graduated">Выпуск этого года</Text>
                    </Flex>
                    <Flex gap="10px" align="center">
                      <Checkbox name="enrollment" id="enrollment"/>
                      <Text as="label" htmlFor="enrollment">Зачисленные в этом году</Text>
                    </Flex>
                  </VStack>
                  <VStack align="stretch">
                    <Button>Поиск</Button>
                    <Flex gap="10px" align="center" cursor="pointer">
                      <Text fontWeight="bold">Очистить фильтры</Text>
                      <AiOutlineClose size="20px"/>
                    </Flex>
                  </VStack>
                </Flex>
              </Box>
            ) : null}
            <Box w="100%">
              <StudentsTable/>
            </Box>
          </Flex>
        </Flex>
      </Flex>
    </BaseLayout>
  )
}