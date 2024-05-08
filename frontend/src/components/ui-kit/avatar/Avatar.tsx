import styled from "styled-components";
import { colors } from "../variables";

type AvatarProps = {
  img?: string;
  size?: "sm" | "md" | "lg";
} 

export const Avatar = styled.div<AvatarProps>`
  border: 2px solid ${colors.green};
  border-radius: 50%;
  width: ${props => props.size === "lg" ? "150px" : props.size === "md" ? "100px" : "50px"};
  height: ${props => props.size === "lg" ? "150px" : props.size === "md" ? "100px" : "50px"};
  background-image: url(${props => props.img});
  background-size: cover;
  flex-shrink: 0;
`

Avatar.defaultProps = {
  size: "sm",
  img: process.env.PUBLIC_URL + "/images/default-avatar.svg",
}