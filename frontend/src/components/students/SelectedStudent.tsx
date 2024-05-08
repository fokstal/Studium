import { Box, Flex, Text, VStack } from "@chakra-ui/react";
import { Avatar, colors } from "../ui-kit";
import { useEffect, useState } from "react";
import { Person } from "../../types";
import {
  AiOutlineCloseSquare,
  AiOutlineGift,
  AiOutlinePushpin,
  AiOutlineTeam,
} from "react-icons/ai";
import { PersonService } from "../../services";
import { calculateAge } from "../../lib";

type SelectedStudentProps = {
  id: number;
  setSelectedStudent: Function;
  group: string;
};

const personService = new PersonService();

export function SelectedStudent({
  id,
  setSelectedStudent,
  group,
}: SelectedStudentProps) {
  const [person, setPerson] = useState<Person>();

  useEffect(() => {
    personService.getById(+id).then((person) => setPerson(person));
  }, [id]);

  return (
    <Box borderRadius="5px" p="20px" bg={colors.green} w="100%">
      <VStack align="stretch" gap="20px">
        <VStack align="stretch">
          <Flex justify="end" gap="5px">
            <Box borderRadius="5px" bg={colors.darkGreen} cursor="pointer">
              <AiOutlinePushpin color={colors.white} size="24px" />
            </Box>
            <Box cursor="pointer">
              <AiOutlineCloseSquare
                color={colors.white}
                onClick={() => setSelectedStudent(null)}
                size="24px"
              />
            </Box>
          </Flex>
          <Flex gap="20px">
            <Avatar
              // img={`http://localhost:5141/Pictures/Person/${person?.avatarFileName}`}
              size="lg"
            />
            {/* TODO: FIX PATH TO SERVER IMAGE MB CREATE CUSTOM HOOK TO GET PATH */}
            <VStack align="stretch" gap="12px">
              <Text
                color={colors.white}
                fontWeight="bold"
                fontSize="24px"
              >{`${person?.firstName} ${person?.lastName}`}</Text>
              <Flex borderRadius="5px" bg={colors.white} gap="10px" paddingInline="10px">
                <Flex gap="5px" align="center">
                  <AiOutlineGift size="24px" />
                  <Text color={colors.black}>
                    {calculateAge(person?.birthDate || "").toString()}
                  </Text>
                </Flex>
                <Box bg={colors.black} w="2px" h="34px"></Box>
                <Flex gap="5px" align="center">
                  <AiOutlineTeam size="24px"/>
                  <Text color={colors.black}>{group}</Text>
                </Flex>
              </Flex>
            </VStack>
          </Flex>
        </VStack>
      </VStack>
    </Box>
  );
}
