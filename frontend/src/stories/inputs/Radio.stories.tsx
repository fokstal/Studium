import { Meta, StoryObj } from "@storybook/react";
import { Radio } from "../../components/ui-kit";

const meta: Meta<typeof Radio> = {
  component: Radio
}

export default meta;
type Story = StoryObj<typeof Radio>;


export const defaultRadio: Story = {
  render: () => {
    return (
      <div style={{display: "flex", gap: "10px", alignItems: "center"}}>
        <label htmlFor="rad1">Радио 1</label>
        <Radio name="rad" id="rad1"/>
        <label htmlFor="rad2">Радио 2</label>
        <Radio name="rad" id="rad2"/>
      </div>
    )
  }
}
