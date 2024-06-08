import { Meta, StoryObj } from "@storybook/react";
import { Avatar } from "../components/ui-kit/";

const meta: Meta<typeof Avatar> = {
  component: Avatar
}

export default meta;
type Story = StoryObj<typeof Avatar>;

export const defaultAvatar: Story = {
}

export const sizedAvatars: Story = {
  render: () => (
    <div style={{display: "flex", gap: "15px"}}>
      <Avatar size="sm"/>
      <Avatar size="md"/>
      <Avatar size="lg"/>
    </div>
  )
}
