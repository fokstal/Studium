import { Box, Flex, Input, InputGroup, Text, VStack } from "@chakra-ui/react";
import { Button, Checkbox, Select, colors } from "../ui-kit";
import { AiOutlineClose } from "react-icons/ai";
import { useContext, useEffect, useState } from "react";
import { LanguageContext, Translator } from "../../store";
import { Group } from "../../types";
import { GroupService } from "../../services";

const groupService = new GroupService();

type FiltersProps = {
  filters: {
    group: Group | null;
    startDate: string;
    endDate: string;
    finish: boolean;
    start: boolean;
  };
  setFilters: Function;
};

export function Filters({ filters, setFilters }: FiltersProps) {
  const [groups, setGroups] = useState<Group[]>([]);
  const { lang } = useContext(LanguageContext);

  useEffect(() => {
    groupService.get().then((groups) => setGroups(groups));
  }, []);

  const handleGroupChange = (group: Group | null) => {
    setFilters({ ...filters, group });
  };

  const handleSearch = () => {
    console.log("Filters:", filters);
  };

  const handleClearFilters = () => {
    setFilters({
      group: null,
      startDate: "",
      endDate: "",
      finish: false,
      start: false,
    });
  };

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
          <Select
            placeholder={Translator[lang.name]["select_group"]}
            options={groups}
            value={filters.group}
            setValue={handleGroupChange}
          />
        </VStack>
        <VStack align="stretch">
          <Text fontWeight="bold">
            {Translator[lang.name]["date_start_stude"]}
          </Text>
          <InputGroup maxW="200px">
            <Input
              type="date"
              value={filters.startDate}
              onChange={(e) =>
                setFilters({ ...filters, startDate: e.target.value })
              }
              borderColor={colors.lightGrey}
              _hover={{ borderColor: colors.green }}
              _focusVisible={{ boxShadow: "none" }}
              bg={colors.white}
              borderRadius="5px 0 0 5px"
              placeholder={Translator[lang.name]["from"]}
            />
            <Input
              type="date"
              borderColor={colors.lightGrey}
              value={filters.endDate}
              onChange={(e) =>
                setFilters({ ...filters, endDate: e.target.value })
              }
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
            <Checkbox
              name="graduated"
              id="graduated"
              checked={filters.finish}
              onChange={(e) =>
                setFilters({ ...filters, finish: e.target.checked })
              }
            />
            <Text as="label" htmlFor="graduated">
              {Translator[lang.name]["this_year_finish"]}
            </Text>
          </Flex>
          <Flex gap="10px" align="center">
            <Checkbox
              name="enrollment"
              id="enrollment"
              checked={filters.start}
              onChange={(e) =>
                setFilters({ ...filters, start: e.target.checked })
              }
            />
            <Text as="label" htmlFor="enrollment">
              {Translator[lang.name]["this_year_start"]}
            </Text>
          </Flex>
        </VStack>
        <VStack align="stretch">
          <Button onClick={handleSearch}>
            {Translator[lang.name]["search"]}
          </Button>
          <Flex
            gap="10px"
            align="center"
            cursor="pointer"
            onClick={handleClearFilters}
          >
            <Text fontWeight="bold" userSelect="none">
              {Translator[lang.name]["clear_filters"]}
            </Text>
            <AiOutlineClose size="20px" />
          </Flex>
        </VStack>
      </Flex>
    </Box>
  );
}
