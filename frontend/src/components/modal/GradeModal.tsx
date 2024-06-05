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
import {
  GradeService,
  StudentService,
  SubjectService,
  UserService,
} from "../../services";
import { useContext, useEffect, useState } from "react";
import { Student, Subject, User } from "../../types";
import { LanguageContext, Translator } from "../../store";

type GradeModalProps = {
  isOpen: boolean;
  onClose: () => void;
};

const userService = new UserService();
const subjectService = new SubjectService();
const gradeServcie = new GradeService();
const studentService = new StudentService();

export function GradeModal({ isOpen, onClose }: GradeModalProps) {
  const [studentsUsers, setStudentsUsers] = useState<User[]>();
  const [subjects, setSubjects] = useState<Subject[]>();
  const [data, setData] = useState<{
    value?: number;
    subjectId?: number;
    studentId?: string;
    type?: number;
    setDate?: string;
  }>({});
  const { lang } = useContext(LanguageContext);

  useEffect(() => {
    subjectService.getSubjectsBySession().then((subjects) => setSubjects(subjects));
  }, []);
  
  const updateStudents = async () => {
    if (data.subjectId) {
      const res = await userService.getUsersBySubject(data.subjectId);
      setStudentsUsers(res);
    }
  };

  useEffect(() => {
    updateStudents();
  }, [data]);

  const handleSubmit = async () => {
    const res = await gradeServcie.post({
      subjectEntityId: data.subjectId,
      typeEnum: data.type,
      setDate: data.setDate || null,
      gradeDTOList: [
        {
          studentEntityId: data.studentId,
          value: data.value,
        },
      ],
    });
    if (res.status === 201) onClose();
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
            {Translator[lang.name]["adding_mark"]}
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
              <Text>{Translator[lang.name]["subject"]}:</Text>
              <Select
                borderColor={colors.darkGrey}
                _focusVisible={{ borderColor: colors.darkGrey }}
                value={data.subjectId}
                onChange={(e) =>
                  setData({ ...data, subjectId: +e.target.value })
                }
              >
                <option>{Translator[lang.name]["select_subject"]}</option>
                {subjects?.map((subject) => (
                  <option value={subject.id}>{subject.name}</option>
                ))}
              </Select>
            </VStack>
            <VStack align="stretch">
              <Text>{Translator[lang.name]["student"]}:</Text>
              <Select
                disabled={!data.subjectId}
                borderColor={colors.darkGrey}
                _focusVisible={{ borderColor: colors.darkGrey }}
                onChange={(e) =>
                  setData({ ...data, studentId: e.target.value })
                }
              >
                <option>{Translator[lang.name]["select_student"]}</option>
                {studentsUsers?.map((student) => (
                  <option value={student.id}>
                    {student.firstName} {student.lastName}
                  </option>
                ))}
              </Select>
            </VStack>
            <VStack align="stretch">
              <Text>{Translator[lang.name]["grade_type"]}:</Text>
              <Select
                borderColor={colors.darkGrey}
                _focusVisible={{ borderColor: colors.darkGrey }}
                onChange={(e) => setData({ ...data, type: +e.target.value })}
              >
                <option>{Translator[lang.name]["select_type"]}</option>
                <option value={1}>
                  {Translator[lang.name]["practical_work"]}
                </option>
                <option value={2}>
                  {Translator[lang.name]["control_work"]}
                </option>
                <option value={3}>{Translator[lang.name]["lecture"]}</option>
              </Select>
            </VStack>
            <VStack align="stretch">
              <Text>{Translator[lang.name]["mark"]}:</Text>
              <Input
                type="text"
                value={data.value}
                onChange={(e) => setData({ ...data, value: +e.target.value })}
              />
            </VStack>
            <VStack align="stretch">
              <Text>{Translator[lang.name]["date"]}:</Text>
              <Input
                type="date"
                value={data.setDate}
                onChange={(e) => setData({ ...data, setDate: e.target.value })}
              />
            </VStack>
          </VStack>
        </ModalBody>
        <ModalFooter w="100%">
          <VStack w="100%" align="stretch">
            <Button onClick={handleSubmit}>
              {Translator[lang.name]["adding_mark"]}
            </Button>
          </VStack>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
