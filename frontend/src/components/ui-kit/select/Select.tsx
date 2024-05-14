import { Dropdown } from "primereact/dropdown";
import { SelectItemOptionsType } from "primereact/selectitem";
import { useState } from "react";
import styled from "styled-components";
import { colors } from "../variables";
import "./overide-select-panel.css";

type SelectProps = {
  options: SelectItemOptionsType;
  placeholder: string;
  value: any;
  setValue: Function;
  name?: string;
};

const SelectWrapper = styled.div`
  border: 1px solid ${colors.lightGrey};
  width: max-content;
  border-radius: 6px;

  & > .p-dropdown {
    gap: 10px;
    font-size: 16px;
    font-weight: 400;
    padding: 6px 10px;
  }
`;

const Option = styled.li`
  background: ${colors.white};
  transition: 0.2s;
  padding: 6px 10px;

  &:hover {
    background: ${colors.lightGrey};
    color: ${colors.black};
  }
`;

export function Select({
  options,
  placeholder,
  value,
  setValue,
  name = "name",
}: SelectProps) {
  const OptionlTemplate = (option: any) => {
    return (
      <Option style={{ color: value === option ? colors.green : colors.black }}>
        {option[name]}
      </Option>
    );
  };

  return (
    <SelectWrapper style={{ color: value ? colors.black : colors.lightGrey }}>
      <Dropdown
        style={{ background: colors.white }}
        value={value}
        onChange={(e) => setValue(e.value)}
        options={options}
        optionLabel={name}
        placeholder={placeholder}
        itemTemplate={OptionlTemplate}
      />
    </SelectWrapper>
  );
}
