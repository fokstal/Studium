import { Meta, StoryObj } from "@storybook/react";
import { Input } from "../../components/ui-kit";

const meta: Meta<typeof Input> = {
  component: Input
}

export default meta;
type Story = StoryObj<typeof Input>;

export const disabledInput: Story = {
  args: {
    disabled: true,
    placeholder: "vk.com"
  }
}

export const defaultInput: Story = {
  args: {
    placeholder: "write me"
  }
}
