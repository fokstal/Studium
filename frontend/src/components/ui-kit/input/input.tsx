import { ComponentProps } from "react";
import styled from "styled-components";
import { colors } from "../variables";

export const Input = styled.input<ComponentProps<"input">>`
  border: 1px solid  ${colors.lightGrey};
  border-radius: 5px;
  padding: 6px 10px;
  outline: none;
  font-size: 16px;
  &:focus {
    border-color: ${colors.green}
  }
  &:disabled {
    cursor: not-allowed;
  }
`
