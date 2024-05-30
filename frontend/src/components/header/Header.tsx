import { Flex, Text } from "@chakra-ui/react";
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

const authService = new AuthServise();
const studentService = new StudentService();
const personService = new PersonService();

export function Header() {
  const [isAuth, setIsAuth] = useState<boolean>(true);
  const [person, setPerson] = useState<Person>();
  const location = useLocation();
  const { lang, setLang } = useContext(LanguageContext);

  useEffect(() => {
    updateAvatar();
  }, []);

  const updateAvatar = async () => {
    const res = await authService.session();
    setIsAuth(!!res);
    const role = res?.roleList[0]?.name;
    localStorage.setItem("role", role);

    if (role !== "Student") return;
    const student = await studentService.getById(res.id);
    const person = await personService.getById(student.personId);
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
          {localStorage.getItem("role") === "Student" ? null : (
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
          {localStorage.getItem("role") === "Student" ? (
            <Link to="/profile">
              <Avatar
                img={
                  person?.avatarFileName
                    ? getAvatarPath(person?.avatarFileName)
                    : ""
                }
              />
            </Link>
          ) : null}
        </Flex>
      </Flex>
    </BaseLayout>
  );
}
