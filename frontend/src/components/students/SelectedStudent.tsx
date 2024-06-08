import { Box, Flex, Link, Text, VStack } from "@chakra-ui/react";
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
import { calculateAge, getAvatarPath } from "../../lib";

type SelectedStudentProps = {
  id: number;
  setSelectedStudent: Function;
  group: string;
  studentId: number;
};

const personService = new PersonService();

export function SelectedStudent({
  id,
  setSelectedStudent,
  group,
  studentId,
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
              img={
                person?.avatarFileName
                  ? getAvatarPath(person?.avatarFileName)
                  : ""
              }
              size="lg"
            />
            <VStack align="stretch" gap="12px">
              <Link
                href={`/profile/${studentId}`}
                color={colors.white}
                fontWeight="bold"
                fontSize="24px"
              >{`${person?.firstName} ${person?.middleName}`}</Link>
              <Flex
                borderRadius="5px"
                bg={colors.white}
                gap="10px"
                paddingInline="10px"
              >
                <Flex gap="5px" align="center">
                  <AiOutlineGift size="24px" />
                  <Text color={colors.black}>
                    {calculateAge(person?.birthDate || "").toString()}
                  </Text>
                </Flex>
                <Box bg={colors.black} w="2px" h="34px"></Box>
                <Flex gap="5px" align="center">
                  <AiOutlineTeam size="24px" />
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
