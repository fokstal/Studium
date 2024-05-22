import { Flex, Text } from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { Avatar, Select } from "../ui-kit";
import { colors } from "../ui-kit/variables";
import { AiOutlineEye } from "react-icons/ai";
import { Link, useLocation } from "react-router-dom";
import { useContext } from "react";
import { LanguageContext, Translator } from "../../store";
import userhelperlibrary from "userhelperlibrary";

export function Header() {
  const location = useLocation();
  const { lang, setLang } = useContext(LanguageContext);

  return (
    <BaseLayout>
      <Flex align="center" justify="space-between" h="100px">
        <Link to="/">
          <img
            src={process.env.PUBLIC_URL + "/images/logo.svg"}
            alt="Studium"
            width="158"
            height="30"
          />
        </Link>
        <Flex gap="20px">
          <Text>
            <Link
              style={{
                color: location.pathname.includes("/students")
                  ? colors.green
                  : colors.black,
                textDecoration: "none",
              }}
              to="/students"
            >
              {Translator[lang.name]["student_list"]}
            </Link>
          </Text>
          <Text>|</Text>
          <Text>
            <Link
              style={{
                color: location.pathname.includes("/journal")
                  ? colors.green
                  : colors.black,
                textDecoration: "none",
              }}
              to="/journal"
            >
              {Translator[lang.name]["online_journal"]}
            </Link>
          </Text>
        </Flex>
        <Flex gap="20px" align="center">
          <AiOutlineEye size="24" onClick={() => userhelperlibrary()}/>
          <Select
            options={[{ name: "RU" }, { name: "EN" }, { name: "BE" }]}
            placeholder="RU"
            value={lang}
            setValue={setLang}
          />
          <Link to="/profile">
            <Avatar />
          </Link>
        </Flex>
      </Flex>
    </BaseLayout>
  );
}
