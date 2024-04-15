import { Flex } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { Heading1 } from "../ui-kit/typoghraphy/typoghraphy";
import { Select, Avatar } from "../ui-kit";
import { languages } from "../ui-kit/variables";
import { AiOutlineEye } from "react-icons/ai";

export function Header() {
  return (
    <BaseLayout>
      <Flex align="center" justify="space-between" h="100%">
        <img src={process.env.PUBLIC_URL + "/images/logo.svg"} alt="Studium" width="158" height="30"/>
        <Flex>
          {/*TODO add links after add react router*/}
          <Heading1>LINKS</Heading1>
        </Flex>
        <Flex gap="20px" align="center">
          <AiOutlineEye size="24"/>
          <Select options={languages} placeholder="RU"/>
          <Avatar/>
        </Flex>
      </Flex>
    </BaseLayout>
  )
}