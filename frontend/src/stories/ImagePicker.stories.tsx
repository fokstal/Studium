import { Meta, StoryObj } from "@storybook/react";
import { ImagePicker } from "../components/ui-kit";
import { useRef, useState } from "react";
import { Box, Text } from "@chakra-ui/react";

const meta: Meta<typeof ImagePicker> = {
  component: ImagePicker
}

export default meta;
type Story = StoryObj<typeof ImagePicker>;

export const ImagePickerBase: Story = {
  render: () => {
    const [file, setFile] = useState<File | null>();

    return (
      <Box>
        <ImagePicker setFile={setFile}/>
        <Text>{file?.name}</Text>
      </Box>
    )
  }
}
