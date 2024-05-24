import {
  Modal,
  ModalBody,
  ModalContent,
  ModalFooter,
  ModalHeader,
  ModalOverlay,
  Text,
  VStack,
  Select,
} from "@chakra-ui/react";
import { AiOutlineCloseSquare } from "react-icons/ai";
import { Button, Input, colors } from "../ui-kit";
import { GradeService, SubjectService, UserService } from "../../services";
import { useEffect, useState } from "react";
import { Subject, User } from "../../types";

type GradeModalProps = {
  isOpen: boolean;
  onClose: () => void;
};

const userService = new UserService();
const subjectService = new SubjectService();
const gradeServcie = new GradeService();

export function GradeModal({ isOpen, onClose }: GradeModalProps) {
  const [studentsUsers, setStudentsUsers] = useState<User[]>();
  const [subjects, setSubjects] = useState<Subject[]>();
  const [data, setData] = useState<{
    value?: number;
    subjectId?: number;
    studentId?: string;
    subject?: Subject;
    student?: User;
  }>({});

  useEffect(() => {
    subjectService.get().then((subjects) => setSubjects(subjects));
    userService
      .get()
      .then((data) =>
        setStudentsUsers(
          data.filter((user: User) => user.roleList[0].name === "Student")
        )
      );
  }, []);


  const handleSubmit = () => {
    console.log(data);
    gradeServcie.post(data);
    onClose();
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose}>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader
          display="flex"
          justifyContent="space-between"
          alignItems="center"
        >
          <Text fontSize="24px" fontWeight="bold">
            Добавление оценки
          </Text>
          <AiOutlineCloseSquare
            fontSize="24px"
            color={colors.darkGreen}
            onClick={onClose}
          />
        </ModalHeader>
        <ModalBody>
          <VStack w="100%" align="stretch" gap="20px">
            <VStack align="stretch">
              <Text>Предмет:</Text>
              <Select
                borderColor={colors.darkGrey}
                _focusVisible={{ borderColor: colors.darkGrey }}
                value={data.subjectId}
                onChange={(e) =>
                  setData({ ...data, subjectId: +e.target.value })
                }
              >
                <option>Выберите предмет</option>
                {subjects?.map((subject) => (
                  <option value={subject.id}>{subject.name}</option>
                ))}
              </Select>
            </VStack>
            <VStack align="stretch">
              <Text>Учащийся:</Text>
              <Select
                borderColor={colors.darkGrey}
                _focusVisible={{ borderColor: colors.darkGrey }}
                onChange={(e) =>
                  setData({ ...data, studentId: e.target.value })
                }
              >
                <option>Выберите учащегося</option>
                {studentsUsers?.map((student) => (
                  <option value={student.id}>{student.firstName}</option>
                ))}
              </Select>
            </VStack>
            <VStack align="stretch">
              <Text>Оценка:</Text>
              <Input
                type="text"
                value={data.value}
                onChange={(e) => setData({ ...data, value: +e.target.value })}
              />
            </VStack>
          </VStack>
        </ModalBody>
        <ModalFooter w="100%">
          <VStack w="100%" align="stretch">
            <Button onClick={handleSubmit}>Добавить оценку</Button>
          </VStack>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
