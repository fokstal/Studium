import { Flex, Input } from "@chakra-ui/react";
import { ChangeEvent, useRef } from "react";
import { colors } from "../variables";
import { AiOutlineCamera } from "react-icons/ai";

type ImagePickerProps = {
  setFile: Function; 
};

export function ImagePicker({ setFile }: ImagePickerProps) {
  const fileInputRef = useRef<HTMLInputElement>(null);

  const handleFileUploadClick = () => {
    fileInputRef.current?.click();
  };

  const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      setFile(file);
    }
  };

  return (
    <Flex
      bg={colors.darkGreen}
      align="center"
      justify="center"
      borderRadius="50%"
      w="150px"
      h="150px"
      onClick={handleFileUploadClick}
      transition="0.4s"
      _hover={{bg: colors.green}}
    >
      <Input
        type="file"
        visibility="hidden"
        w="0"
        display="none"
        onChange={(e) => handleFileChange(e)}
        ref={fileInputRef}
      />
      <AiOutlineCamera size="48px" color={colors.white}/>
    </Flex>
  );
}
