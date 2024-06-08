import { Button } from "../components/ui-kit";
import { Meta, StoryObj } from "@storybook/react";

const meta: Meta<typeof Button> = {
  component: Button
}

export default meta;
type Story = StoryObj<typeof Button>;

export const disabledButton: Story = {
  args: {
    disabled: true,
    children: "Кнопка дефолт"
  }
}

export const defaultButton: Story = {
  args: {
    children: "Кнопка дефолт"
  }
}
