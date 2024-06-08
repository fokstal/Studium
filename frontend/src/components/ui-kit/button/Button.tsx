import { ComponentProps } from "react";
import styled from "styled-components";
import { colors } from "../variables";

export const Button = styled.button<ComponentProps<"button">>`
  border: 2px solid ${colors.green};
  color: ${colors.green};
  font-size: 1rem;
  border-radius: 5px;
  padding: 15px 20px;
  background: ${colors.white};
  transition: 0.5s;
  cursor: pointer;

  &:hover {
    background: ${colors.green};
    color: ${colors.white};
  }
  &:active {
    background: ${colors.darkGreen};
  }
  &:disabled {
    border-color: ${colors.lightGrey};
    color: ${colors.lightGrey};
    background: ${colors.white};
    cursor: not-allowed;
  }
`