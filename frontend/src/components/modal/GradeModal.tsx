import {
  Modal,
  ModalBody,
  ModalContent,
  ModalFooter,
  ModalHeader,
  ModalOverlay,
  Text,
  VStack,
} from "@chakra-ui/react";
import { AiOutlineCloseSquare } from "react-icons/ai";
import { Button, Input, colors } from "../ui-kit";

type GradeModalProps = {
  isOpen: boolean;
  onClose: () => void;
};

export function GradeModal({ isOpen, onClose }: GradeModalProps) {
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
              <Text>Дата:</Text>
              <Input type="date" />
            </VStack>
            <VStack align="stretch">
              <Text>Предмет:</Text>
              <Input type="text" />
            </VStack>
            <VStack align="stretch">
              <Text>Учащийся:</Text>
              <Input type="text" />
            </VStack>
            <VStack align="stretch">
              <Text>Оценка:</Text>
              <Input type="text" />
            </VStack>
          </VStack>
        </ModalBody>
        <ModalFooter w="100%">
          <VStack w="100%" align="stretch">
            <Button>Добавить оценку</Button>
          </VStack>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
