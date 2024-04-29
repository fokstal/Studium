import { Dropdown } from 'primereact/dropdown';
import { SelectItemOptionsType } from 'primereact/selectitem';
import { useState } from 'react';
import styled from 'styled-components';
import { colors } from '../variables';
import "./overide-select-panel.css";

type SelectProps = {
  options: SelectItemOptionsType;
  placeholder: string;
}

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
`

const Option = styled.li`
  background: ${colors.white};
  transition: 0.2s;
  padding: 6px 10px;

  &:hover {
    background: ${colors.lightGrey};
    color: ${colors.black};
  }
`

export function Select({options, placeholder}: SelectProps) {
  const [selectedItem, setSelectedItem] = useState(null);

  const OptionlTemplate = (option: any) => {
    return (
      <Option style={{color: selectedItem === option ? colors.green : colors.black}}>
        {option.name}
      </Option>
    )
  }

  return (
    <SelectWrapper style={{color: selectedItem ? colors.black : colors.lightGrey}}>
      <Dropdown 
      style={{background: colors.white}}
      value={selectedItem} 
      onChange={(e) => setSelectedItem(e.value)} 
      options={options} 
      optionLabel="name" 
      placeholder={placeholder} 
      itemTemplate={OptionlTemplate}
      />
    </SelectWrapper>
  )
};
