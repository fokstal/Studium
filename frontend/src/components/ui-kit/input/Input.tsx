import { useState } from "react";
import { FaEye, FaEyeSlash } from "react-icons/fa";
import styled from "styled-components";
import { colors } from "../variables";

interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {}

const InputWrapper = styled.div`
  position: relative;
  width: 100%;

  input {
    border: 1px solid ${colors.lightGrey};
    border-radius: 5px;
    padding: 6px 10px;
    outline: none;
    font-size: 1rem;
    background: ${colors.white};
    width: 100%;

    &:focus {
      border-color: ${colors.green};
    }

    &:disabled {
      cursor: not-allowed;
    }
  }

  button {
    position: absolute;
    top: 50%;
    right: 10px;
    transform: translateY(-50%);
    background: transparent;
    border: none;
    cursor: pointer;
    color: ${colors.grey};

    &:hover {
      color: ${colors.black};
    }
  }
`;

export const Input = styled.input<React.ComponentProps<"input">>`
  border: 1px solid ${colors.lightGrey};
  border-radius: 5px;
  padding: 6px 10px;
  outline: none;
  font-size: 1rem;
  background: ${colors.white};
  width: 100%;

  &:focus {
    border-color: ${colors.green};
  }
  &:disabled {
    cursor: not-allowed;
  }
`;

export const PasswordInput: React.FC<InputProps> = (props) => {
  const [showPassword, setShowPassword] = useState(false);

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  return (
    <InputWrapper>
      <Input {...props} type={showPassword ? "text" : "password"} />
      <button onClick={togglePasswordVisibility}>
        {showPassword ? <FaEyeSlash /> : <FaEye />}
      </button>
    </InputWrapper>
  );
};
