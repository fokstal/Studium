import { Meta, StoryObj } from "@storybook/react";
import { Checkbox } from "../../components/ui-kit";

const meta: Meta<typeof Checkbox> = {
  component: Checkbox
}

export default meta;
type Story = StoryObj<typeof Checkbox>;


export const defaultCheckbox: Story = {
  render: () => {
    return (
      <div style={{display: "flex", gap: "10px", alignItems: "center"}}>
        <label htmlFor="rad1">Чекбокс 1</label>
        <Checkbox name="rad" id="rad1"/>
        <label htmlFor="rad2">Чекбокс 2</label>
        <Checkbox name="rad" id="rad2"/>
      </div>
    )
  }
}
