import { ComponentProps } from "react";
import styled from "styled-components";
import { colors } from "../variables";

// TODO: remake component from accent color prop to more flexible prop ebuchi ie
export const Radio = styled.input<ComponentProps<"input">>`
  border: 1px solid ${colors.lightGrey};
  width: 20px;
  height: 20px;
  accent-color: ${colors.darkGreen};
`

Radio.defaultProps = {
  type: "radio",
}