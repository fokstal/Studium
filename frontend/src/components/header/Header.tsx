import { Flex, Text } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { Select, Avatar } from "../ui-kit";
import { colors, languages } from "../ui-kit/variables";
import { AiOutlineEye } from "react-icons/ai";
import { Link, useLocation } from "react-router-dom";

export function Header() {
  const location = useLocation();

  return (
    <BaseLayout>
      <Flex align="center" justify="space-between" h="100px">
        <Link to="/">
          <img src={process.env.PUBLIC_URL + "/images/logo.svg"} alt="Studium" width="158" height="30"/>
        </Link>
        <Flex gap="20px">
          <Text>
            <Link style={
              {color: location.pathname.includes("/students") ? colors.green : colors.black, textDecoration: "none"}} 
              to="/students">
                Список учащихся
            </Link>
          </Text>
          <Text>|</Text>
          <Text>
            <Link style={
              {color: location.pathname.includes("/journal") ? colors.green : colors.black, textDecoration: "none"}}
              to="/journal">
                Электронный журнал
            </Link>
          </Text>
        </Flex>
        <Flex gap="20px" align="center">
          <AiOutlineEye size="24"/>
          <Select options={languages} placeholder="RU"/>
          <Link to="/profile">
            <Avatar/>
          </Link>
        </Flex>
      </Flex>
    </BaseLayout>
  )
}