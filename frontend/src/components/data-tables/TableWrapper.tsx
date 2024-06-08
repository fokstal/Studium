import styled from "styled-components";
import { colors } from "../ui-kit/variables";

export const TableWrapper = styled.div`
  background-color: ${colors.white};
  width: 100%;
  font-size: 16px;
  color: ${colors.black};
  font-weight: 700;

  & th {
    padding: 15px;
    border: 1px solid ${colors.lightGrey};

    & > .p-column-header-content {
      gap: 10px;
    }
  }

  & td {
    padding: 25px 15px;
    border: 1px solid ${colors.lightGrey};
  }

  & .p-datatable-thead {
    background-color: ${colors.darkGrey};
  }

  & .p-paginator {
    padding: 20px 0;
    gap: 10px;

    & .p-paginator-prev, 
    .p-paginator-next, 
    .p-paginator-last, 
    .p-paginator-first,
    .p-paginator-page {
      width: 46px;
      height: 46px;
      border-radius: 50%;

      &.p-disabled {
        border: 1px solid ${colors.grey};
      }

      &:not(.p-disabled) {
        border: 1px solid ${colors.green};
        color: ${colors.green};
        transition: 0.4s;

        &:hover {
          background-color: ${colors.green};
          color: ${colors.white}
        }
      }
    }

    & > .p-paginator-pages {
      display: flex;
      gap: 10px;
      & > .p-highlight {
        background-color: ${colors.green};
        color: ${colors.white};
      }
    }
  }
`