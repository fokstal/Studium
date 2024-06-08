import { Select } from "../components/ui-kit";
import { Meta, StoryObj } from "@storybook/react";

const meta: Meta<typeof Select> = {
  component: Select
}

export default meta;
type Story = StoryObj<typeof Select>;

export const defaultSelect: Story = {
  args: {
    options:  [
      { name: 'Australia', code: 'AU' },
      { name: 'Brazil', code: 'BR' },
      { name: 'China', code: 'CN' },
      { name: 'Egypt', code: 'EG' },
      { name: 'France', code: 'FR' },
      { name: 'Germany', code: 'DE' },
      { name: 'India', code: 'IN' },
      { name: 'Japan', code: 'JP' },
      { name: 'Spain', code: 'ES' },
      { name: 'United States', code: 'US' }
    ],
    placeholder: "Choose country"
  }
}
