import {
  Flex,
  IconButton,
  Menu,
  MenuButton,
  MenuItem,
  MenuList,
  Text,
} from "@chakra-ui/react";
import { BaseLayout } from "../../layouts";
import { Avatar, Select } from "../ui-kit";
import { colors } from "../ui-kit/variables";
import { AiOutlineEye } from "react-icons/ai";
import { Link, useLocation } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import { LanguageContext, Translator } from "../../store";
import userhelperlibrary from "userhelperlibrary";
import { AuthServise, PersonService, StudentService } from "../../services";
import { Error401Page } from "../../pages";
import { getAvatarPath } from "../../lib";
import { Person } from "../../types";
import { useRoles } from "../../hooks";
import { GiHamburgerMenu } from "react-icons/gi";
import { FaUserGroup } from "react-icons/fa6";
import { PiBookFill } from "react-icons/pi";
import { LuUserSquare } from "react-icons/lu";
import { IoMdExit } from "react-icons/io";

const authService = new AuthServise();
const studentService = new StudentService();
const personService = new PersonService();

export function Header() {
  const [isAuth, setIsAuth] = useState<boolean>(true);
  const [person, setPerson] = useState<Person>();
  const location = useLocation();
  const { lang, setLang } = useContext(LanguageContext);
  const roles = useRoles();

  useEffect(() => {
    updateAvatar();
  }, []);

  const updateAvatar = async () => {
    if (!roles[0]) setIsAuth(false);
    if (!roles.includes("Student")) return;
    const res = await authService.session();
    const student = await studentService.getById(res.id);
    const person = await personService.getById(student.personEntityId);
    setPerson(person);
  };

  if (!isAuth) {
    return <Error401Page />;
  }

  return (
    <BaseLayout>
      <Flex align="center" justify="space-between" h="100px">
        <Link to="/students">
          <img
            src={process.env.PUBLIC_URL + "/images/logo.svg"}
            alt="Studium"
            width="158"
            height="30"
          />
        </Link>
        <Flex gap="20px">
          {roles.includes("Student") ? null : (
            <>
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
            </>
          )}
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
          <AiOutlineEye size="24" onClick={() => userhelperlibrary()} />
          <Select
            options={[{ name: "RU" }, { name: "EN" }, { name: "BE" }]}
            placeholder="RU"
            value={lang}
            setValue={setLang}
          />
          {roles.includes("Student") ? (
            <Link to="/profile">
              <Avatar
                img={
                  person?.avatarFileName
                    ? getAvatarPath(person?.avatarFileName)
                    : ""
                }
              />
            </Link>
          ) : (
            <Menu>
              <MenuButton
                as={IconButton}
                aria-label="Options"
                icon={<GiHamburgerMenu />}
                variant="outline"
                _hover={{ background: colors.green }}
                _active={{ background: colors.darkGreen }}
              />
              <MenuList>
                <MenuItem _hover={{ background: colors.darkGrey }}>
                  <Link to="/group">
                    <Flex gap="10px" align="center">
                      <FaUserGroup />
                      <Text>{Translator[lang.name]["group_list"]}</Text>
                    </Flex>
                  </Link>
                </MenuItem>
                <MenuItem _hover={{ background: colors.darkGrey }}>
                  <Link to="/subject">
                    <Flex gap="10px" align="center">
                      <PiBookFill />
                      {Translator[lang.name]["subject_list"]}
                    </Flex>
                  </Link>
                </MenuItem>
                <MenuItem _hover={{ background: colors.darkGrey }}>
                  <Flex gap="10px" align="center">
                    <LuUserSquare />
                    <Link to="/users">
                      {Translator[lang.name]["user_list"]}
                    </Link>
                  </Flex>
                </MenuItem>
                <MenuItem
                  _hover={{ background: colors.darkGrey }}
                  color={colors.red}
                >
                  <Flex gap="10px" align="center">
                    <IoMdExit />
                    <Link to="/" onClick={() => authService.logout()}>
                      {Translator[lang.name]["exit"]}
                    </Link>
                  </Flex>
                </MenuItem>
              </MenuList>
            </Menu>
          )}
        </Flex>
      </Flex>
    </BaseLayout>
  );
}
